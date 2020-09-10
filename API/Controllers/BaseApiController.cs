using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    //purpose: acts as a base controller, that save us typing out
    //the same thing on every single controller we create
    
    [ApiController] //this attribute can be applied to a controller class
                    //to enable some API-specific behaviours
    [Route("api/[controller]")] //specify URL pattern for a controller
    public class BaseApiController : ControllerBase
    { 
        
    }
}