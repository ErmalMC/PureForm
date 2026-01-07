using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PureForm.Application.Interfaces
{
    public interface IStripeService
    {
        Task<string> CreateCheckoutSessionAsync(int userId, string priceId);
        Task<bool> HandleWebhookAsync(string json, string signature);
        Task<bool> CancelSubscriptionAsync(int userId);
    }
}
