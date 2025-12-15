using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Staffinity.Personal.Domain.Modules.AiIntelligence.CoreModels;
using Staffinity.Personal.Domain.Modules.AiIntelligence.Ports.Out;

namespace Staffinity.Personal.Infrastructure.Adapters.Ai;

public sealed class GeminiAiClient : IAiModelClient
{
    private const string ApiKeyHeader = "x-goog-api-key";

    private readonly HttpClient _http;
    private readonly GeminiOptions _options;
    private readonly JsonSerializerOptions _json;

    public GeminiAiClient(HttpClient httpClient, GeminiOptions options)
    {
        _http = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _options = options ?? throw new ArgumentNullException(nameof(options));

        _json = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false,
        };
    }

    public async Task<AiInsight> AskAsync(
        AiModelRequest request,
        CancellationToken cancellationToken = default
    )
    {
        if (string.IsNullOrWhiteSpace(request.Question))
            return AiInsight.CreateBasic(request.Intent, "No question provided.");

        var endpoint = new Uri(_options.BaseUri, $"models/{_options.Model}:generateContent");

        var contextJson = JsonSerializer.Serialize(request.Context, _json);

        // Nota: evitamos features “exóticas” del payload para máxima compatibilidad.
        var prompt =
            $"{request.Question}\n\n"
            + $"ContextSnapshot(JSON): {contextJson}\n\n"
            + "Return ONLY valid JSON with this shape:\n"
            + "{ \"severity\": \"info|warning|critical\", \"summary\": \"...\", \"recommendations\": [{\"title\":\"...\",\"rationale\":\"...\",\"suggestedAction\":\"...\"}] }\n"
            + "Do not include markdown fences.";

        var payload = new
        {
            contents = new[] { new { role = "user", parts = new[] { new { text = prompt } } } },
            generationConfig = new
            {
                temperature = _options.Temperature,
                maxOutputTokens = _options.MaxOutputTokens,
            },
        };

        using var httpRequest = new HttpRequestMessage(HttpMethod.Post, endpoint)
        {
            Content = new StringContent(
                JsonSerializer.Serialize(payload, _json),
                Encoding.UTF8,
                "application/json"
            ),
        };

        httpRequest.Headers.TryAddWithoutValidation(ApiKeyHeader, _options.ApiKey);

        if (!string.IsNullOrWhiteSpace(request.CorrelationId))
            httpRequest.Headers.TryAddWithoutValidation("X-Correlation-ID", request.CorrelationId);

        using var response = await SendWithRetryAsync(httpRequest, cancellationToken);
        var responseText = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
            throw MapError(response.StatusCode, responseText);

        var modelText = ExtractCandidateText(responseText);
        var parsed = TryParseInsightJson(modelText);

        return new AiInsight(
            Intent: request.Intent,
            Severity: parsed.Severity,
            Summary: parsed.Summary,
            Recommendations: parsed.Recommendations,
            CreatedAt: DateTimeOffset.UtcNow
        );
    }

    private async Task<HttpResponseMessage> SendWithRetryAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken
    )
    {
        var buffered = await CloneRequestAsync(request, cancellationToken);

        for (var attempt = 0; attempt <= _options.MaxRetries; attempt++)
        {
            using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            cts.CancelAfter(_options.RequestTimeout);

            using var attemptRequest = await CloneRequestAsync(buffered, cancellationToken);

            try
            {
                var response = await _http.SendAsync(
                    attemptRequest,
                    HttpCompletionOption.ResponseHeadersRead,
                    cts.Token
                );

                if (IsTransientStatus(response.StatusCode) && attempt < _options.MaxRetries)
                {
                    response.Dispose();
                    await BackoffAsync(attempt, cancellationToken);
                    continue;
                }

                return response;
            }
            catch (TaskCanceledException ex) when (!cancellationToken.IsCancellationRequested)
            {
                if (attempt < _options.MaxRetries)
                {
                    await BackoffAsync(attempt, cancellationToken);
                    continue;
                }

                throw new GeminiAiClientException(
                    "Gemini request timed out.",
                    null,
                    true,
                    null,
                    ex
                );
            }
            catch (HttpRequestException ex)
            {
                if (attempt < _options.MaxRetries)
                {
                    await BackoffAsync(attempt, cancellationToken);
                    continue;
                }

                throw new GeminiAiClientException("Gemini network error.", null, true, null, ex);
            }
        }

        throw new GeminiAiClientException("Gemini request failed after retries.");
    }

    private async Task BackoffAsync(int attempt, CancellationToken ct)
    {
        var jitterMs = Random.Shared.Next(0, 100);
        var delay = TimeSpan.FromMilliseconds(
            _options.RetryBaseDelay.TotalMilliseconds * Math.Pow(2, attempt) + jitterMs
        );
        if (delay > TimeSpan.Zero)
            await Task.Delay(delay, ct);
    }

    private static bool IsTransientStatus(HttpStatusCode statusCode) =>
        statusCode
            is HttpStatusCode.TooManyRequests
                or HttpStatusCode.InternalServerError
                or HttpStatusCode.BadGateway
                or HttpStatusCode.ServiceUnavailable
                or HttpStatusCode.GatewayTimeout;

    private static GeminiAiClientException MapError(HttpStatusCode statusCode, string body)
    {
        string? providerMessage = null;
        try
        {
            using var doc = JsonDocument.Parse(body);
            if (doc.RootElement.TryGetProperty("error", out var err))
            {
                if (err.TryGetProperty("message", out var msg))
                    providerMessage = msg.GetString();
                else if (err.TryGetProperty("status", out var st))
                    providerMessage = st.GetString();
            }
        }
        catch
        {
            // Ignore parse errors
        }

        var isTransient = IsTransientStatus(statusCode);

        var message = statusCode switch
        {
            HttpStatusCode.Unauthorized or HttpStatusCode.Forbidden =>
                "Gemini auth failed. Check API key/permissions.",
            HttpStatusCode.BadRequest => "Gemini rejected the request (bad request).",
            HttpStatusCode.TooManyRequests => "Gemini rate limit exceeded (429).",
            _ => $"Gemini request failed with {(int)statusCode} ({statusCode}).",
        };

        if (!string.IsNullOrWhiteSpace(providerMessage))
            message = $"{message} Provider: {providerMessage}";

        return new GeminiAiClientException(message, statusCode, isTransient, providerMessage);
    }

    private string ExtractCandidateText(string responseJson)
    {
        using var doc = JsonDocument.Parse(responseJson);

        if (
            !doc.RootElement.TryGetProperty("candidates", out var candidates)
            || candidates.ValueKind != JsonValueKind.Array
        )
            throw new GeminiAiClientException("Gemini response missing candidates.");

        var first = candidates.EnumerateArray().FirstOrDefault();
        if (first.ValueKind == JsonValueKind.Undefined)
            throw new GeminiAiClientException("Gemini returned no candidates.");

        if (!first.TryGetProperty("content", out var content))
            throw new GeminiAiClientException("Gemini candidate missing content.");

        if (
            !content.TryGetProperty("parts", out var parts)
            || parts.ValueKind != JsonValueKind.Array
        )
            throw new GeminiAiClientException("Gemini content missing parts.");

        var part0 = parts.EnumerateArray().FirstOrDefault();
        if (part0.ValueKind == JsonValueKind.Undefined)
            throw new GeminiAiClientException("Gemini returned empty parts.");

        if (!part0.TryGetProperty("text", out var textEl))
            throw new GeminiAiClientException("Gemini part missing text.");

        return textEl.GetString() ?? string.Empty;
    }

    private (
        AiInsightSeverity Severity,
        string Summary,
        IReadOnlyList<AiRecommendation> Recommendations
    ) TryParseInsightJson(string modelText)
    {
        if (string.IsNullOrWhiteSpace(modelText))
            return (
                AiInsightSeverity.Info,
                "No response from model.",
                Array.Empty<AiRecommendation>()
            );

        //  Sometimes the model returns text with spaces/new lines; we try to parse it anyway.
        modelText = modelText.Trim();

        try
        {
            using var doc = JsonDocument.Parse(modelText);

            var summary = doc.RootElement.TryGetProperty("summary", out var s)
                ? (s.GetString() ?? "")
                : "";

            var severity = AiInsightSeverity.Info;
            if (doc.RootElement.TryGetProperty("severity", out var sev))
            {
                var sevStr = (sev.GetString() ?? "").Trim().ToLowerInvariant();
                severity = sevStr switch
                {
                    "critical" => AiInsightSeverity.Critical,
                    "warning" => AiInsightSeverity.Warning,
                    _ => AiInsightSeverity.Info,
                };
            }

            var recs = new List<AiRecommendation>();
            if (
                doc.RootElement.TryGetProperty("recommendations", out var arr)
                && arr.ValueKind == JsonValueKind.Array
            )
            {
                foreach (var item in arr.EnumerateArray())
                {
                    var title = item.TryGetProperty("title", out var t)
                        ? (t.GetString() ?? "")
                        : "";
                    var rationale = item.TryGetProperty("rationale", out var r)
                        ? (r.GetString() ?? "")
                        : "";
                    var suggested = item.TryGetProperty("suggestedAction", out var a)
                        ? a.GetString()
                        : null;

                    if (!string.IsNullOrWhiteSpace(title) && !string.IsNullOrWhiteSpace(rationale))
                        recs.Add(new AiRecommendation(title, rationale, suggested));
                }
            }

            if (string.IsNullOrWhiteSpace(summary))
                summary = modelText;

            return (severity, summary, recs);
        }
        catch
        {
            return (AiInsightSeverity.Info, modelText, Array.Empty<AiRecommendation>());
        }
    }

    private static async Task<HttpRequestMessage> CloneRequestAsync(
        HttpRequestMessage original,
        CancellationToken ct
    )
    {
        var clone = new HttpRequestMessage(original.Method, original.RequestUri);

        foreach (var h in original.Headers)
            clone.Headers.TryAddWithoutValidation(h.Key, h.Value);

        if (original.Content is not null)
        {
            var contentBytes = await original.Content.ReadAsByteArrayAsync(ct);
            clone.Content = new ByteArrayContent(contentBytes);

            foreach (var h in original.Content.Headers)
                clone.Content.Headers.TryAddWithoutValidation(h.Key, h.Value);
        }

        return clone;
    }
}
