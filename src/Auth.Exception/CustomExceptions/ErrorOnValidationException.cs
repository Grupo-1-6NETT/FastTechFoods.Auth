namespace Auth.Exception.CustomExceptions;
public class ErrorOnValidationException : FastTechFoodsException
{
    public ErrorOnValidationException(IList<string> errorMessages) : base(errorMessages)
    {
    }
}
