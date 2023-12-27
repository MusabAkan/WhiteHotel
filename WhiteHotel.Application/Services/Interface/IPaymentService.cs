using Stripe.Checkout;
using WhiteHotel.Domain.Entities;

namespace WhiteHotel.Application.Services.Interface
{
    public interface IPaymentService
    {
        SessionCreateOptions CreateStripeSessionOptions(Booking booking, Villa villa, string domain);
        Session CreateStripeSession(SessionCreateOptions options);

    }
}
