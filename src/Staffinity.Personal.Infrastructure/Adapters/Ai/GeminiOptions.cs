using System;

namespace Staffinity.Personal.Infrastructure.Adapters.Ai;

public sealed record GeminiOptions(
    Uri BaseUri,
    string Model,
    string ApiKey,
    TimeSpan RequestTimeout,
    int MaxRetries,
    TimeSpan RetryBaseDelay,
    int MaxOutputTokens,
    decimal Temperature
)
{
    public const string EnvApiKeyPrimary = "GEMINI_API_KEY";
    public const string EnvApiKeyStaffinity = "STAFFINITY_AI_GEMINI_API_KEY";
    public const string EnvBaseUrl = "STAFFINITY_AI_GEMINI_BASE_URL";
    public const string EnvModel = "STAFFINITY_AI_GEMINI_MODEL";
    public const string EnvTimeoutSeconds = "STAFFINITY_AI_GEMINI_TIMEOUT_SECONDS";
    public const string EnvMaxRetries = "STAFFINITY_AI_GEMINI_MAX_RETRIES";
    public const string EnvRetryBaseDelayMs = "STAFFINITY_AI_GEMINI_RETRY_BASE_DELAY_MS";
    public const string EnvMaxOutputTokens = "STAFFINITY_AI_GEMINI_MAX_OUTPUT_TOKENS";
    public const string EnvTemperature = "STAFFINITY_AI_GEMINI_TEMPERATURE";

    public static GeminiOptions FromEnvironment()
    {
        var apiKey =
            Environment.GetEnvironmentVariable(EnvApiKeyStaffinity)
            ?? Environment.GetEnvironmentVariable(EnvApiKeyPrimary);

        if (string.IsNullOrWhiteSpace(apiKey))
            throw new InvalidOperationException(
                $"Missing Gemini API key. Set {EnvApiKeyStaffinity} or {EnvApiKeyPrimary}."
            );

        var baseUrl =
            Environment.GetEnvironmentVariable(EnvBaseUrl)
            ?? "https://generativelanguage.googleapis.com/v1beta/";

        var model = Environment.GetEnvironmentVariable(EnvModel) ?? "gemini-2.5-flash";

        var timeoutSeconds = TryInt(EnvTimeoutSeconds, 30);
        var maxRetries = TryInt(EnvMaxRetries, 2);
        var retryBaseDelayMs = TryInt(EnvRetryBaseDelayMs, 250);
        var maxOutputTokens = TryInt(EnvMaxOutputTokens, 512);
        var temperature = TryDecimal(EnvTemperature, 0.2m);

        return new GeminiOptions(
            BaseUri: new Uri(baseUrl),
            Model: model,
            ApiKey: apiKey,
            RequestTimeout: TimeSpan.FromSeconds(timeoutSeconds),
            MaxRetries: Math.Max(0, maxRetries),
            RetryBaseDelay: TimeSpan.FromMilliseconds(Math.Max(0, retryBaseDelayMs)),
            MaxOutputTokens: Math.Max(1, maxOutputTokens),
            Temperature: Clamp(temperature, 0m, 2m)
        );
    }

    private static int TryInt(string env, int fallback) =>
        int.TryParse(Environment.GetEnvironmentVariable(env), out var v) ? v : fallback;

    private static decimal TryDecimal(string env, decimal fallback) =>
        decimal.TryParse(Environment.GetEnvironmentVariable(env), out var v) ? v : fallback;

    private static decimal Clamp(decimal v, decimal min, decimal max) =>
        v < min ? min : (v > max ? max : v);
}
