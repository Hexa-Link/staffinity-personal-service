namespace Staffinity.Personal.Domain.Modules.AiIntelligence.CoreModels
{
    public sealed record AiMetric(string Key, decimal Value, string? Unit = null);

    public sealed record AiContextSnapshot(
        DateTimeOffset CapturedAt,
        AiIntent Intent,
        AiUserRole RequestorRole,
        IReadOnlyList<AiMetric> Metrics,
        IReadOnlyDictionary<string, string> Tags
    )
    {
        public static AiContextSnapshot Empty(AiIntent intent, AiUserRole role) =>
            new(
                CapturedAt: DateTimeOffset.UtcNow,
                Intent: intent,
                RequestorRole: role,
                Metrics: Array.Empty<AiMetric>(),
                Tags: new Dictionary<string, string>()
            );
    }
}
