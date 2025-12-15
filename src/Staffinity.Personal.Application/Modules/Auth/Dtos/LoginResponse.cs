namespace Staffinity.Personal.Application.Modules.Auth.Dtos;

public class LoginResponse
{
    public string AccessToken { get; set; } = string.Empty;

    public LoginResponse(string accessToken)
    {
        AccessToken = accessToken;
    }
}
