using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Staffinity.Personal.Application.Modules.AiIntelligence.Services;
using Staffinity.Personal.Domain.Modules.AiIntelligence.CoreModels;

namespace Staffinity.Personal.Infrastructure.Adapters.Ai;

public sealed class LlmIntentDetector : IIntentDetector
{
    private const string ApiKeyHeader = "x-goog-api-key";
    private readonly HttpClient _http;
    private readonly GeminiOptions _options;
    private readonly JsonSerializerOptions _json;

    public LlmIntentDetector(HttpClient httpClient, GeminiOptions options)
    {
        _http = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _options = options ?? throw new ArgumentNullException(nameof(options));
        _json = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };
    }

    public AiIntent Detect(string question)
    {
        // We use GetAwaiter().GetResult() because the interface is synchronous.
        // In a real scenario, we should refactor the interface to be async,
        // but the requirements say "Application layer must remain untouched".
        return DetectAsync(question).GetAwaiter().GetResult();
    }

    private async Task<AiIntent> DetectAsync(string question)
    {
        if (string.IsNullOrWhiteSpace(question))
            return AiIntent.HrKpiSummary;

        var endpoint = new Uri(_options.BaseUri, $"models/{_options.Model}:generateContent");

        var prompt =
            $"Classify the user question into one of these intents:\n" +
            $"- HrKpiSummary (general HR metrics)\n" +
            $"- EmployeeHeadcountSnapshot (headcount, employees, staff)\n" +
            $"- VacationRequestsOverview (vacations, leave, time off)\n" +
            $"- TurnoverRiskSignals (turnover, attrition, retention)\n" +
            $"- WorkforceAnomalies (anomalies, unusual patterns)\n" +
            $"- VacationPolicyCompliance (policy, compliance, rules)\n" +
            $"- ActionRecommendations (recommendations, actions, what to do)\n\n" +
            $"User Question: {question}\n\n" +
            $"Return ONLY a JSON object: {{ \"intent\": \"IntentName\" }}";

        var payload = new
        {
            contents = new[] { new { role = "user", parts = new[] { new { text = prompt } } } },
            generationConfig = new
            {
                temperature = 0.0, // Deterministic
                maxOutputTokens = 50,
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

        try
        {
            using var response = await _http.SendAsync(httpRequest);
            if (!response.IsSuccessStatusCode)
            {
                // Fallback on error
                return AiIntent.HrKpiSummary;
            }

            var responseText = await response.Content.ReadAsStringAsync();
            var modelText = ExtractCandidateText(responseText);

            return ParseIntent(modelText);
        }
        catch
        {
            // Fallback on exception
            return AiIntent.HrKpiSummary;
        }
    }

    private string ExtractCandidateText(string responseJson)
    {
        try
        {
            using var doc = JsonDocument.Parse(responseJson);
            if (doc.RootElement.TryGetProperty("candidates", out var candidates) &&
                candidates.GetArrayLength() > 0)
            {
                var first = candidates[0];
                if (first.TryGetProperty("content", out var content) &&
                    content.TryGetProperty("parts", out var parts) &&
                    parts.GetArrayLength() > 0)
                {
                    return parts[0].GetProperty("text").GetString() ?? string.Empty;
                }
            }
        }
        catch
        {
            // Ignore
        }
        return string.Empty;
    }

    private AiIntent ParseIntent(string json)
    {
        try
        {
            // Clean up markdown code blocks if present
            json = json.Replace("```json", "").Replace("```", "").Trim();

            using var doc = JsonDocument.Parse(json);
            if (doc.RootElement.TryGetProperty("intent", out var prop))
            {
                var intentStr = prop.GetString();
                if (Enum.TryParse<AiIntent>(intentStr, true, out var intent))
                {
                    return intent;
                }
            }
        }
        catch
        {
            // Ignore
        }
        return AiIntent.HrKpiSummary;
    }
}
