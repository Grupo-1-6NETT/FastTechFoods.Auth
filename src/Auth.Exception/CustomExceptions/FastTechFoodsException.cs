namespace Auth.Exception.CustomExceptions;
public class FastTechFoodsException : System.Exception
{
    public IList<string> ErrorMessages { get; set; }
    public FastTechFoodsException(IList<string> errorMessages)
    {
        ErrorMessages = errorMessages;
    }

    public FastTechFoodsException(string errorMessage)
    : this([errorMessage]) { }
}
