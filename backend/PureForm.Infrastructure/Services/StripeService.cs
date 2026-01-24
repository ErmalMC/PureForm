using Microsoft.Extensions.Configuration;
using PureForm.Application.Interfaces;
using PureForm.Domain.Entities;
using PureForm.Infrastructure.Repositories;
using Stripe;
using Stripe.Checkout;

namespace PureForm.Infrastructure.Services
{
    public class StripeService : IStripeService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<StripeSubscription> _subscriptionRepository;
        private readonly string _webhookSecret;

        public StripeService(
            IRepository<User> userRepository,
            IRepository<StripeSubscription> subscriptionRepository,
            IConfiguration configuration)
        {
            _userRepository = userRepository;
            _subscriptionRepository = subscriptionRepository;
            _webhookSecret = configuration["Stripe:WebhookSecret"] ?? "";
            StripeConfiguration.ApiKey = configuration["Stripe:SecretKey"];
        }

        public async Task<string> CreateCheckoutSessionAsync(int userId, string priceId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) throw new Exception("User not found");

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
            {
                new()
                {
                    Price = priceId,
                    Quantity = 1
                }
            },
                Mode = "subscription",
                SuccessUrl = "http://localhost:5173/success?session_id={CHECKOUT_SESSION_ID}",
                CancelUrl = "http://localhost:5173/cancel",
                ClientReferenceId = userId.ToString(),
                CustomerEmail = user.Email
            };

            var service = new SessionService();
            var session = await service.CreateAsync(options);
            return session.Url;
        }

        public async Task<bool> HandleWebhookAsync(string json, string signature)
        {
            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json, signature, _webhookSecret);

                if (stripeEvent.Type == "checkout.session.completed")
                {
                    var session = stripeEvent.Data.Object as Session;
                    if (session == null) return false;

                    var userId = int.Parse(session.ClientReferenceId);
                    var user = await _userRepository.GetByIdAsync(userId);
                    if (user == null) return false;

                    user.IsPremium = true;
                    await _userRepository.UpdateAsync(user);

                    var subscription = new StripeSubscription
                    {
                        UserId = userId,
                        StripeCustomerId = session.CustomerId,
                        StripeSubscriptionId = session.SubscriptionId,
                        Status = "active",
                        CurrentPeriodStart = DateTime.UtcNow,
                        CurrentPeriodEnd = DateTime.UtcNow.AddMonths(1),
                        CreatedAt = DateTime.UtcNow
                    };

                    await _subscriptionRepository.AddAsync(subscription);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> CancelSubscriptionAsync(int userId)
        {
            var subscriptions = await _subscriptionRepository.FindAsync(s => s.UserId == userId && s.Status == "active");
            var subscription = subscriptions.FirstOrDefault();
            if (subscription == null) return false;

            var service = new SubscriptionService();
            await service.CancelAsync(subscription.StripeSubscriptionId);

            subscription.Status = "canceled";
            subscription.UpdatedAt = DateTime.UtcNow;
            await _subscriptionRepository.UpdateAsync(subscription);

            var user = await _userRepository.GetByIdAsync(userId);
            if (user != null)
            {
                user.IsPremium = false;
                await _userRepository.UpdateAsync(user);
            }

            return true;
        }
    }
}
