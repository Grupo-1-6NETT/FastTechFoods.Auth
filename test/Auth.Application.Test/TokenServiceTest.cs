using Auth.Application.Services.TokenJwt;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;

namespace Auth.Application.Test;
public class TokenServiceTest
{
    [Fact]
    public void GerarToken_DeveGerarTokenValido_ComClaimsEsperados()
    {        
        var jwtSettings = Options.Create(new JwtSettings
        {
            SecretKey = "12345678901234567890123456789012", 
            Issuer = "FastTechFoods",
            Audience = "FastTechFoodsAPI",
            ExpiryInMinutes = 60
        });
        var service = new TokenService(jwtSettings);
        var userId = "cliente123";
        var role = "cliente";

        var token = service.GerarToken(userId, role);
        var handler = new JwtSecurityTokenHandler();
        var decoded = handler.ReadJwtToken(token);

        Assert.Equal(userId, decoded.Claims.First(c => c.Type == "nameid").Value);
        Assert.Equal(role, decoded.Claims.First(c => c.Type == "role").Value);
        Assert.Equal("FastTechFoods", decoded.Issuer);
        Assert.Equal("FastTechFoodsAPI", decoded.Audiences.First());
    }

}
