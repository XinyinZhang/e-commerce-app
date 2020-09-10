using API.Errors;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    
    public class BuggyController : BaseApiController
    {
        private readonly StoreContext _context;
        public BuggyController(StoreContext context)
        {
            _context = context;
        }

        [HttpGet("notfound")]
        public ActionResult GetNotFoundRequest()
        {
            var thing = _context.Products.Find(42);
            if(thing == null){
                return NotFound(new ApiResponse(404));
            }
            return Ok();
        }

        [HttpGet("servererror")]
        public ActionResult GetServerError()
        {
           //no referrence error: trying to execute a method on something that is none
           var thing = _context.Products.Find(42);
           var thingToReturn = thing.ToString();
           return Ok(); //return an empty Status200 OK response
        }

        [HttpGet("badrequest")]
        
        public ActionResult GetBadRequest()
        {

            return BadRequest(new ApiResponse(400));
            //return BadRequest();
        }
        [HttpGet("badrequest/{id}")]
        public ActionResult GetBadRequest(int id)
        {
            //generate the validation error by passing in a string
            //instead of an int id
            return Ok();
        }

    }
}