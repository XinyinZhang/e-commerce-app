using System;

namespace API.Errors
{
    // consistent error response from API
    public class ApiResponse
    {
        public ApiResponse(int statusCode, string message = null)                                                             
       {
            StatusCode = statusCode;
            //use the default message given or, if the
            //the default message is none, we'll create our own default messages
            //??: if message is none, then execute what's in the right
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
        }

    

        public int StatusCode{ get; set; }
        public string Message { get; set; }

        //create a default message based on the statusCode
        //each type of error should have at least a message
        private string GetDefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "A bad request, you have made",
                401 => "Authorized, you are not",
                404 => "Resource found, it was not",
                500 => "Errors are the path to the dark side. Errors lead to anger. Anger leads to hate. Hate leads to career change",
                _ => null
            };
        }
        
    }
}