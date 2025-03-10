using System.Net;

namespace Solarnelle.Domain.Exceptions
{
    public class CustomHttpException : Exception
    {
        public HttpStatusCode? StatusCode { get; set; }

        public CustomHttpException(string message, HttpStatusCode? statusCode = default) : base(message)
        {
            StatusCode = statusCode ?? HttpStatusCode.InternalServerError;
        }

        public CustomHttpException(string message, Exception innerException, HttpStatusCode? statusCode = default) : base(message, innerException)
        {
            StatusCode = statusCode ?? HttpStatusCode.InternalServerError;
        }
    }
}
