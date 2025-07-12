using Auth.Exception.CustomExceptions;
using Auth.Exception.ErrorMessages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Auth.API.Filters;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        var problemDetails = new ProblemDetails();

        switch (context.Exception)
        {
            case ErrorOnValidationException ex:
                problemDetails.Status = StatusCodes.Status400BadRequest;
                problemDetails.Title = "Erro de validação";
                problemDetails.Extensions["errors"] = ex.ErrorMessages;
                break;

            case NotFoundException ex:
                problemDetails.Status = StatusCodes.Status404NotFound;
                problemDetails.Title = "Recurso não encontrado";
                problemDetails.Detail = ResourceErrorMessages.NOT_FOUND;
                break;

            case UnauthorizedAccessException:
                problemDetails.Status = StatusCodes.Status401Unauthorized;
                problemDetails.Title = "Não autorizado";
                break;

            default:
                problemDetails.Status = StatusCodes.Status500InternalServerError;
                problemDetails.Title = "Erro interno";
                problemDetails.Detail = ResourceErrorMessages.UNKNOWN_ERROR;
                break;
        }

        context.Result = new ObjectResult(problemDetails)
        {
            StatusCode = problemDetails.Status
        };

        context.ExceptionHandled = true;
    }
}