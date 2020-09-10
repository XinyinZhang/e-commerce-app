using System.Collections.Generic;

namespace API.Errors
{
    public class ApiValidationErrorResponse : ApiResponse
    {
        public ApiValidationErrorResponse() : base(400)
        {
        }
        //array of string of errors
        //since there may be more than 1 validation error
        public IEnumerable<string> Errors { get; set; }

    }
}