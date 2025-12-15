using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Staffinity.Personal.Domain.Modules.AiIntelligence.CoreModels;
using Staffinity.Personal.Domain.Modules.AiIntelligence.Exceptions;

namespace Staffinity.Personal.Application.Modules.AiIntelligence.Services
{
    internal static class AllowedIntentsPolicy
    {
        private static readonly HashSet<AiIntent> HrAllowedIntents = new()
        {
            AiIntent.HrKpiSummary,
            AiIntent.EmployeeHeadcountSnapshot,
            AiIntent.VacationRequestsOverview,
            AiIntent.TurnoverRiskSignals,
            AiIntent.WorkforceAnomalies,
            AiIntent.VacationPolicyCompliance,
            AiIntent.ActionRecommendations,
        };

        public static void EnsureRoleAllowed(AiUserRole role)
        {
            if (role == AiUserRole.Employee)
                throw new ForbiddenAiDataAccessException(
                    "Role 'Employee' is not allowed to execute AI queries."
                );
        }

        public static void EnsureIntentAllowed(AiUserRole role, AiIntent intent)
        {
            EnsureRoleAllowed(role);

            // If an invalid intent (cast/bug) arrives, we block it
            if (!Enum.IsDefined(typeof(AiIntent), intent))
                throw new UnauthorizedAiIntentException(role, intent);

            // HR and Admin allowed (Admin = HR + future)
            if (role is AiUserRole.Hr or AiUserRole.Admin)
            {
                if (!HrAllowedIntents.Contains(intent))
                    throw new UnauthorizedAiIntentException(role, intent);

                return;
            }

            // Ultra-safe fallback
            throw new ForbiddenAiDataAccessException(
                $"Role '{role}' is not allowed to execute AI queries."
            );
        }
    }
}
