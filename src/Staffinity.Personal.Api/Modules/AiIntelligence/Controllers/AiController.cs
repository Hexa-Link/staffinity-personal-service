using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Staffinity.Personal.Application.Modules.AiIntelligence.UseCases;
using Staffinity.Personal.Application.Modules.AiIntelligence.Contracts;
using Staffinity.Personal.Api.Modules.AiIntelligence.Dtos;
using Staffinity.Personal.Domain.Modules.AiIntelligence.CoreModels;

namespace Staffinity.Personal.Api.Modules.AiIntelligence.Controllers;

[ApiController]
[Route("ai")]
[Authorize]
public sealed class AiController : ControllerBase
{
    private readonly AskAiWithContextUseCase _useCase;

    public AiController(AskAiWithContextUseCase useCase)
    {
        _useCase = useCase;
    }

    [HttpPost("query")]
    public async Task<IActionResult> Query(
        [FromBody] AiQueryRequestDto request,
        CancellationToken cancellationToken)
    {
        var role = User.FindFirstValue(ClaimTypes.Role);

        if (role is not ("HR" or "Admin"))
            return Forbid();

        var userRole = role == "Admin"
            ? AiUserRole.Admin
            : AiUserRole.Hr;

        var query = new AiOrchestrationQuery(
            Question: request.Question,
            RequestorRole: userRole
        );

        var result = await _useCase.ExecuteAsync(query, cancellationToken);

        return Ok(new AiQueryResponseDto
        {
            Summary = result.Summary,
            Severity = result.Severity.ToString(),
            Recommendations = result.Recommendations
                .Select(r => new AiRecommendationDto
                {
                    Title = r.Title,
                    Rationale = r.Rationale,
                    SuggestedAction = r.SuggestedAction
                })
                .ToList()
        });
    }
}