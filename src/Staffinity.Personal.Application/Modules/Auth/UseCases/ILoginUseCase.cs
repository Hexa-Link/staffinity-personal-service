using Staffinity.Personal.Application.Modules.Auth.Dtos;

namespace Staffinity.Personal.Application.Modules.Auth.UseCases;

public interface ILoginUseCase
{
    Task<LoginResponse> ExecuteAsync(LoginRequest request);
}
