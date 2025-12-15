using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Staffinity.Personal.Domain.Modules.AiIntelligence.CoreModels;

namespace Staffinity.Personal.Application.Modules.AiIntelligence.Contracts;

public sealed record AiOrchestrationQuery(
    string Question,
    AiUserRole RequestorRole,
    string? CorrelationId = null
);
