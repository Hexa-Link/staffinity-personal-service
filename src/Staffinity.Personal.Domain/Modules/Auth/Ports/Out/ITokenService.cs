using Staffinity.Personal.Domain.Modules.Auth.Model;

namespace Staffinity.Personal.Domain.Modules.Auth.Ports.Out;

public interface ITokenService
{
    string CreateAccessToken(AuthUser user);
}
