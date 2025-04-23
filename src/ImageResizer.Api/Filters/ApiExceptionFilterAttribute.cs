using System.Net;
using ImageResizer.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ImageResizer.Api.Filters
{
    internal sealed class ApiExceptionFilterAttribute(ILogger<ApiExceptionFilterAttribute> logger) : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            logger.LogError($"An unexpected error has occurred. {context.Exception.Message}");

            if (context.Exception is CustomHttpException customHttpException)
            {
                context.HttpContext.Response.StatusCode = (int)(customHttpException.StatusCode ?? HttpStatusCode.InternalServerError);

                var errorObject = new
                {
                    customHttpException.Message
                };

                context.Result = new ObjectResult(errorObject);
            }
            else
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var errorObject = new
                {
                    Message = "An unexpected error occurred. Please try again later."
                };

                context.Result = new ObjectResult(errorObject);
            }

            base.OnException(context);
        }
    }
}
