using Microsoft.Extensions.Configuration;
using PaymentIntegration.Core.Services.Abstractions;
using PaymentIntegration.Models.ViewModels;
using PaymentIntegration.Utilities;
using PayStack.Net;

namespace PaymentIntegration.Core.Services.Implementations
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly string token;

        private PayStackApi Paystack { get; set; }
        public PaymentService(IConfiguration configuration)
        {
            _configuration = configuration;
            token = _configuration["Payment:PaystackSK"];
            Paystack = new PayStackApi(token);
        }
        public TransactionInitializeResponse InitializeTransaction(DonateViewModel donate)
        {
            TransactionInitializeRequest request = new()
            {
                AmountInKobo = donate.Amount * 100,
                Email = donate.Email,
                Reference = RefGen.Generate().ToString(),
                Currency = "NGN",
                CallbackUrl = "http://localhost:36222/donate/verify"
            };

            TransactionInitializeResponse response = Paystack.Transactions.Initialize(request);
            return response;
        }

        public TransactionVerifyResponse VerifyTransaction(string reference)
        {
            return Paystack.Transactions.Verify(reference);
        }
    }
}
