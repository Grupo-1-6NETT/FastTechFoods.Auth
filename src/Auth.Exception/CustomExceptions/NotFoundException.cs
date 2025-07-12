using Auth.Exception.ErrorMessages;

namespace Auth.Exception.CustomExceptions;
public class NotFoundException : FastTechFoodsException
{
    public NotFoundException() : base(ResourceErrorMessages.NOT_FOUND)
    {
    }
}
