using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace Auth.Application.Services.Cryptography;
internal class Encrypter : IEncrypter
{
    private readonly string _secretKey;

    public Encrypter(IConfiguration config)
    {
        _secretKey = config["Settings:Secret"]
            ?? throw new ArgumentNullException("Chave secreta não configurada");
    }

    public string Encrypt(string text)
    {
        var newText = $"{text}{_secretKey}";
        var bytes = Encoding.UTF8.GetBytes(newText);
        var hashKey = SHA512.HashData(bytes);

        return BytesToString(hashKey);
    }

    private static string BytesToString(byte[] bytes)
    {
        var sb = new StringBuilder();
        foreach (byte b in bytes)
        {
            sb.Append(b.ToString("x2"));
        }

        return sb.ToString();
    }
}