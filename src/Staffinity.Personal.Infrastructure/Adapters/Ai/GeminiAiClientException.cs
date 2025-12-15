using System;
using System.Net;

namespace Staffinity.Personal.Infrastructure.Adapters.Ai;

public sealed class GeminiAiClientException : Exception
{
    public HttpStatusCode? StatusCode { get; }
    public bool IsTransient { get; }
    public string? ProviderError { get; }

    public GeminiAiClientException(
        string message,
        HttpStatusCode? statusCode = null,
        bool isTransient = false,
        string? providerError = null,
        Exception? inner = null
    )
        : base(message, inner)
    {
        StatusCode = statusCode;
        IsTransient = isTransient;
        ProviderError = providerError;
    }
}
