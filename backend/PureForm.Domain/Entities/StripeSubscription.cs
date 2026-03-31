using PureForm.Domain.Common;
using PureForm.Domain.Enums;

namespace PureForm.Domain.Entities;

public class StripeSubscription : BaseEntity
{
    public int UserId { get; set; }
    public string StripeCustomerId { get; set; } = string.Empty;
    public string StripeSubscriptionId { get; set; } = string.Empty;
    public SubscriptionStatus Status { get; set; }
    public DateTime CurrentPeriodStart { get; set; }
    public DateTime CurrentPeriodEnd { get; set; }

    public User User { get; set; } = null!;
}
