using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Staffinity.Personal.Domain.Modules.AiIntelligence.CoreModels;
using Staffinity.Personal.Domain.Modules.AiIntelligence.Ports.Out;
using Staffinity.Personal.Infrastructure.Adapters.Ai;
using Xunit;

public class GeminiAiClientTests
{
    [Fact]
    public async Task Retries_On429_ThenSucceeds()
    {
        var handler = new QueueHttpMessageHandler();

        handler.Enqueue(req =>
        {
            Assert.True(req.Headers.Contains("x-goog-api-key"));
            return new HttpResponseMessage(HttpStatusCode.TooManyRequests)
            {
                Content = new StringContent(
                    "{\"error\":{\"message\":\"rate limited\"}}",
                    Encoding.UTF8,
                    "application/json"
                ),
            };
        });

        handler.Enqueue(_ => new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(
                GoodGeminiResponse(
                    "{\"severity\":\"info\",\"summary\":\"ok\",\"recommendations\":[]}"
                ),
                Encoding.UTF8,
                "application/json"
            ),
        });

        var http = new HttpClient(handler);
        var opt = new GeminiOptions(
            BaseUri: new Uri("https://generativelanguage.googleapis.com/v1beta/"),
            Model: "gemini-2.5-flash",
            ApiKey: "test-key",
            RequestTimeout: TimeSpan.FromSeconds(5),
            MaxRetries: 2,
            RetryBaseDelay: TimeSpan.FromMilliseconds(1),
            MaxOutputTokens: 256,
            Temperature: 0.2m
        );

        var client = new GeminiAiClient(http, opt);

        var req = new AiModelRequest(
            Question: "hello",
            Intent: AiIntent.HrKpiSummary,
            RequestorRole: AiUserRole.Hr,
            Context: AiContextSnapshot.Empty(AiIntent.HrKpiSummary, AiUserRole.Hr),
            CorrelationId: "c1"
        );

        var insight = await client.AskAsync(req);

        Assert.Equal("ok", insight.Summary);
        Assert.Equal(2, handler.CallCount);
    }

    [Fact]
    public async Task DoesNotRetry_On401_ThrowsMappedException()
    {
        var handler = new QueueHttpMessageHandler();
        handler.Enqueue(_ => new HttpResponseMessage(HttpStatusCode.Unauthorized)
        {
            Content = new StringContent(
                "{\"error\":{\"message\":\"invalid api key\"}}",
                Encoding.UTF8,
                "application/json"
            ),
        });

        var http = new HttpClient(handler);
        var opt = new GeminiOptions(
            BaseUri: new Uri("https://generativelanguage.googleapis.com/v1beta/"),
            Model: "gemini-2.5-flash",
            ApiKey: "bad-key",
            RequestTimeout: TimeSpan.FromSeconds(5),
            MaxRetries: 3,
            RetryBaseDelay: TimeSpan.FromMilliseconds(1),
            MaxOutputTokens: 256,
            Temperature: 0.2m
        );

        var client = new GeminiAiClient(http, opt);

        var req = new AiModelRequest(
            Question: "hello",
            Intent: AiIntent.HrKpiSummary,
            RequestorRole: AiUserRole.Admin,
            Context: AiContextSnapshot.Empty(AiIntent.HrKpiSummary, AiUserRole.Admin)
        );

        var ex = await Assert.ThrowsAsync<GeminiAiClientException>(() => client.AskAsync(req));
        Assert.Equal(HttpStatusCode.Unauthorized, ex.StatusCode);
        Assert.False(ex.IsTransient);
        Assert.Equal(1, handler.CallCount);
    }

    private static string GoodGeminiResponse(string candidateTextJson)
    {
        // candidates[0].content.parts[0].text = "<json>"
        return $@"
{{
  ""candidates"": [
    {{
      ""content"": {{
        ""parts"": [
          {{ ""text"": {System.Text.Json.JsonSerializer.Serialize(candidateTextJson)} }}
        ]
      }}
    }}
  ]
}}";
    }
}
