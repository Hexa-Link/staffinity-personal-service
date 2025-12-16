using Staffinity.Personal.Domain.Modules.Auth.Model;

namespace Staffinity.Personal.Domain.Modules.Auth.Ports.Out;

public interface IAuthRepository
{
    Task<(AuthUser? User, string? PasswordHash)> FindByEmailAsync(string email);
    bool VerifyPassword(string password, string passwordHash);
}
