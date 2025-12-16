namespace Staffinity.Personal.Domain.Modules.Auth.Model;

public sealed class AuthUser
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public Guid AccessLevelId { get; set; }
    public string AccessLevelName { get; set; } = string.Empty;
}
