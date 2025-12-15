namespace Staffinity.Personal.Api.Modules.AiIntelligence.Dtos;

public sealed class AiQueryResponseDto
{
    public string Summary { get; init; } = default!;
    public string Severity { get; init; } = default!;
    public IReadOnlyList<AiRecommendationDto> Recommendations { get; init; } = [];
}

public sealed class AiRecommendationDto
{
    public string Title { get; init; } = default!;
    public string Rationale { get; init; } = default!;
    public string? SuggestedAction { get; init; }
}