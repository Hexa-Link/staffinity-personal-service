namespace Staffinity.Personal.Domain.Modules.AiIntelligence.Exceptions
{
    public class ForbiddenAiDataAccessException : Exception
    {
        public ForbiddenAiDataAccessException(string reason)
            : base($"AI data access is forbidden. Reason: {reason}") { }
    }
}
