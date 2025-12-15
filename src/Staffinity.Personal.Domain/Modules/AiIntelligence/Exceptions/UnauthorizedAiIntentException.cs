using Staffinity.Personal.Domain.Modules.AiIntelligence.CoreModels;

namespace Staffinity.Personal.Domain.Modules.AiIntelligence.Exceptions
{
    public sealed class UnauthorizedAiIntentException : Exception
    {
        public AiUserRole Role { get; }
        public AiIntent Intent { get; }

        public UnauthorizedAiIntentException(AiUserRole role, AiIntent intent)
            : base($"AI intent '{intent}' is not allowed for role '{role}'.")
        {
            Role = role;
            Intent = intent;
        }
    }
}
