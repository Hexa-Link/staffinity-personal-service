using Staffinity.Personal.Application.Modules.Auth.Dtos;
using Staffinity.Personal.Domain.Modules.Auth.Ports.Out;

namespace Staffinity.Personal.Application.Modules.Auth.UseCases;

public sealed class LoginUseCase : ILoginUseCase
{
    private readonly IAuthRepository _authRepository;
    private readonly ITokenService _tokenService;

    public LoginUseCase(IAuthRepository authRepository, ITokenService tokenService)
    {
        _authRepository = authRepository ?? throw new ArgumentNullException(nameof(authRepository));
        _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
    }

    public async Task<LoginResponse> ExecuteAsync(LoginRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (string.IsNullOrWhiteSpace(request.Email))
        {
            throw new ArgumentException("Email is required.", nameof(request.Email));
        }

        if (string.IsNullOrWhiteSpace(request.Password))
        {
            throw new ArgumentException("Password is required.", nameof(request.Password));
        }

        var (user, passwordHash) = await _authRepository.FindByEmailAsync(request.Email);

        if (user is null || string.IsNullOrWhiteSpace(passwordHash) ||
            !_authRepository.VerifyPassword(request.Password, passwordHash))
        {
            throw new UnauthorizedAccessException("Invalid email or password.");
        }

        var token = _tokenService.CreateAccessToken(user);
        return new LoginResponse(token);
    }
}
