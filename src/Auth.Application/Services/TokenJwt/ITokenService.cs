namespace Auth.Application.Services.TokenJwt;
public interface ITokenService
{
    string GerarToken(string userId, string role);
}
