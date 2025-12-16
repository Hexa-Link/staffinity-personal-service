namespace Staffinity.Personal.Application.Modules.Auth.Dtos;

public sealed class LoginResponse
{
    public string AccessToken { get; set; }

    public LoginResponse(string accessToken)
    {
        AccessToken = accessToken;
    }
}
