using API.Errors;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("errors/{code}")]
    //we don't want to have an HTTP explicit method here, we want this controller
    //to be able to handle any type of Http method
    
    //tell swagger we don't want this controller to be added as an endpoint that
    //our client would consume --> ignore this controller
    [ApiExplorerSettings(IgnoreApi = true)]
    // ErrorController: 
    // A controller that we redirect the request(for a notfound page) to so that it can generate a consistent API response
    // why not put in BuggyController: cuz we need to overwrite [Route("api/[controller]")]  -->  [Route("errors/{code}")]
    
    /* Idea: A request(send to an endpoint that does not exist) come into API server  --> we redirect it to ErrorController 
    --> ErrorController generates a consistent API response */
    // How to redirect: a middleware in startup class
    public class ErrorController : BaseApiController
    {
        public IActionResult Error(int code){
            return new ObjectResult(new ApiResponse(code));
        }
    }
}