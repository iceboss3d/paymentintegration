using PaymentIntegration.Core.Services.Abstractions;
using PaymentIntegration.Data.Repositories.Abstractions;
using PaymentIntegration.Models.Domain;
using PaymentIntegration.Models.ViewModels;
using PaymentIntegration.Utilities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PaymentIntegration.Core.Services.Implementations
{
    public class DonateService : IDonateService
    {
        private readonly IPaymentService _paymentService;
        private readonly ITransactionRepository _transactionRepository;

        public DonateService(IPaymentService paymentService, ITransactionRepository transactionRepository)
        {
            _paymentService = paymentService;
            _transactionRepository = transactionRepository;
        }
        public async Task<string> Donate(DonateViewModel donate)
        {
            var transactionResponse = _paymentService.InitializeTransaction(donate);
            if (transactionResponse.Status)
            {
                var transaction = new Transaction()
                {
                    Amount = donate.Amount,
                    Email = donate.Email,
                    TrxRef = transactionResponse.Data.Reference,
                    Name = donate.Name,
                };

                await _transactionRepository.SaveTransactionAsync(transaction);
                return transactionResponse.Data.AuthorizationUrl;
            }
            throw new Exception(transactionResponse.Message);
        }

        public async Task<ICollection<Transaction>> GetDonations()
        {
            return await _transactionRepository.GetAllTransactionsAsync();
        }

        public async Task VerifyDonation(string reference)
        {
            var result = _paymentService.VerifyTransaction(reference);
            if (result.Data.Status == PaymentStatus.Success)
            {
                var transaction = await _transactionRepository.GetByReferenceAsync(reference);
                if (transaction != null)
                {
                    transaction.Status = true;
                    await _transactionRepository.UpdateTransaction(transaction);
                }
            }
            else
            {
                throw new Exception(result.Data.GatewayResponse);
            }
        }
    }
}
