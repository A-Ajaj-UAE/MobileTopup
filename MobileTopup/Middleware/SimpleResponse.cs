namespace MobileTopup.API.Middleware
{
    public class SimpleResponse
    {
        public string Message;
        public string Details;

        public SimpleResponse(string message, string details)
        {
            this.Message = message;
            this.Details = details;
        }
    }
}