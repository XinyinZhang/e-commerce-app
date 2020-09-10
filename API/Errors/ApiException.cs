namespace API.Errors
{
    public class ApiException : ApiResponse
    {
        /* extend the ApiResponse to include the stack trace
        (about where the exception was generated from)*/
        // we can then create some middlewares to handle exceptions and use this
        //class any event that we get an exception
        public ApiException(int statusCode, 
        string message = null, string details = null) : base(statusCode, message)
        {
            Details = details;
        }

        //stack trace field
        public string Details { get; set; }
    }
}