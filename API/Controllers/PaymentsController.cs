using System.IO;
using System.Threading.Tasks;
using API.Errors;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Stripe;
using Order = Core.Entities.OrderAggregate.Order;

namespace API.Controllers
{
    public class PaymentsController : BaseApiController
    {
        private readonly IPaymentService _paymentService;
        // we will get this string from Stripe to tell us that
        // we can trust what it's sending
        // anybody can use our endpoint, we only want to trust things that
        // have this secret(we will get this web hook secrets when we configure
        // our endpoint inside stripe)
        private const string WhSecret = "whsec_SRAFvzqjnE4fwmFOr5CbnmiaffBks58C";
        private readonly ILogger<IPaymentService> _logger;

        public PaymentsController(IPaymentService paymentService, 
        ILogger<IPaymentService> logger, 
        IConfiguration config)
        {
            _logger = logger;
            _paymentService = paymentService;
            // _whSecret = config.GetSection("StripeSettings:WhSecret").Value;
        }
        
        [Authorize]
        [HttpPost("{basketId}")]
        public async Task<ActionResult<CustomerBasket>> CreateOrUpdatePaymentIntent(string basketId)
        {
            var basket = await _paymentService.CreateOrUpdatePaymentIntent(basketId);

            if (basket == null) {
                return BadRequest(new ApiResponse(400, "Problem with your basket"));
            }
        
            return basket;
        }
        // read data comming from stripe to this particular endpoint
         [HttpPost("webhook")]
        public async Task<ActionResult> StripeWebhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            // confirm with whSecret that this data is from stripe and we can trust it
            var stripeEvent = EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"], WhSecret);

            PaymentIntent intent;
            Order order;

            switch (stripeEvent.Type)
            {
                case "payment_intent.succeeded":
                    intent = (PaymentIntent)stripeEvent.Data.Object;
                    _logger.LogInformation("Payment Succeeded: ", intent.Id);
                    order  = await _paymentService.UpdateOrderPaymentSucceeded(intent.Id);
                    _logger.LogInformation("Order updated to payment received: ", order.Id);
                    break;
                case "payment_intent.payment_failed":
                    intent = (PaymentIntent)stripeEvent.Data.Object;
                    _logger.LogInformation("Payment Failed: ", intent.Id);
                    order = await _paymentService.UpdateOrderPaymentFailed(intent.Id);
                    _logger.LogInformation("Payment Failed: ", order.Id);
                    break;
            }

            return new EmptyResult();
        }

    }
}