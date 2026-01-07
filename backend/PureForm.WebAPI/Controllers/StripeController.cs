using Microsoft.AspNetCore.Mvc;
using PureForm.Application.Interfaces;
using Stripe.Forwarding;

namespace PureForm.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StripeController : ControllerBase
    {
        private readonly IStripeService _stripeService;

        public StripeController(IStripeService stripeService)
        {
            _stripeService = stripeService;
        }

        [HttpPost("create-checkout-session")]
        public async Task<ActionResult> CreateCheckoutSession([FromBody] CreateCheckoutRequest request)
        {
            try
            {
                var url = await _stripeService.CreateCheckoutSessionAsync(request.UserId, request.PriceId);
                return Ok(new { url });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> HandleWebhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var signature = Request.Headers["Stripe-Signature"].ToString();

            var success = await _stripeService.HandleWebhookAsync(json, signature);
            return success ? Ok() : BadRequest();
        }

        [HttpPost("cancel-subscription/{userId}")]
        public async Task<IActionResult> CancelSubscription(int userId)
        {
            var success = await _stripeService.CancelSubscriptionAsync(userId);
            if (!success) return NotFound("No active subscription found");
            return Ok(new { message = "Subscription canceled successfully" });
        }
    }

    public record CreateCheckoutRequest(int UserId, string PriceId);
}
