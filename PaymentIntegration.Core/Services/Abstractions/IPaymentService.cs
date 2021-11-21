using PaymentIntegration.Models.ViewModels;
using PayStack.Net;

namespace PaymentIntegration.Core.Services.Abstractions
{
    public interface IPaymentService
    {
        TransactionInitializeResponse InitializeTransaction(DonateViewModel donate);
        TransactionVerifyResponse VerifyTransaction(string reference);
    }
}
