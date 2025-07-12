namespace Auth.Application.Services;
public interface IEncrypter
{
    string Encrypt(string text);
}
