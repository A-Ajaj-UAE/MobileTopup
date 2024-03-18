using Newtonsoft.Json;
using System.Net;

namespace MobileTopup.API.Middleware
{
    public class ExceptionHandlerMiddleware : AbstractExceptionHandlerMiddleware
    {
        public ExceptionHandlerMiddleware(RequestDelegate next) : base(next)
        {
        }

        public override (HttpStatusCode code, string message) GetResponse(Exception exception)
        {
            HttpStatusCode code;
            switch (exception)
            {
                case KeyNotFoundException
                    or FileNotFoundException:
                    code = HttpStatusCode.NotFound;
                    break;
                case UnauthorizedAccessException:
                    code = HttpStatusCode.Unauthorized;
                    break;
                case 
                       ArgumentException
                    or InvalidOperationException:
                    code = HttpStatusCode.BadRequest;
                    break;
                default:
                    code = HttpStatusCode.InternalServerError;
                    break;
            }
            SimpleResponse obj = new SimpleResponse(exception.Message, exception.InnerException?.Message);
            string objstring = JsonConvert.SerializeObject(obj);
            return (code, objstring);
        }
    }
}
