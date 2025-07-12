namespace Auth.Application.Services.Cryptography;
public interface IEncrypter
{
    string Encrypt(string text);
}
