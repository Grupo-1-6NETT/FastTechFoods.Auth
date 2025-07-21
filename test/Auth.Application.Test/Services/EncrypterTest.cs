using Auth.Application.Services.Cryptography;
using Microsoft.Extensions.Configuration;

namespace Auth.Application.Test.Services;
public class EncrypterTest
{
    private readonly IEncrypter _encrypter;

    public EncrypterTest()
    {
        var config = new ConfigurationBuilder()
             .AddInMemoryCollection(new Dictionary<string, string?>
             {
                { "Settings:Secret", "da450cda202043fcb740346f45ace357" }
             })
             .Build();

        _encrypter = new Encrypter(config);
    }

    [Fact]
    public void Encrypt_DadoMesmoInput_DeveRetornarMesmoHash()
    {
        var input = "senha123";

        var hash1 = _encrypter.Encrypt(input);
        var hash2 = _encrypter.Encrypt(input);

        Assert.Equal(hash1, hash2);
    }

    [Fact]
    public void Encrypt_DadoDiferentesInputs_DeveRetornarHashsDiferentes()
    {
        var input1 = "senha123";
        var input2 = "senha456";

        var hash1 = _encrypter.Encrypt(input1);
        var hash2 = _encrypter.Encrypt(input2);

        Assert.NotEqual(hash1, hash2);
    }
}
