using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Solarnelle.Domain.Exceptions;

namespace ImageResizer.Api.Filters
{
    internal sealed class ApiExceptionFilterAttribute(ILogger<ApiExceptionFilterAttribute> logger) : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            logger.LogError($"An unexpected error has occurred: {context.Exception.Message}");

            if (context.Exception is CustomHttpException customHttpException)
            {
                context.HttpContext.Response.StatusCode = (int)customHttpException.StatusCode!.Value;

                var errorObject = new
                {
                    customHttpException.Message
                };

                context.Result = new ObjectResult(errorObject);
            }
            else if (context.Exception is DatabaseException databaseException)
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var errorObject = new
                {
                    databaseException.Message
                };

                context.Result = new ObjectResult(errorObject);
            }

            base.OnException(context);
        }
    }
}
