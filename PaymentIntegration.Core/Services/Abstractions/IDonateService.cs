using PaymentIntegration.Models.Domain;
using PaymentIntegration.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PaymentIntegration.Core.Services.Abstractions
{
    public interface IDonateService
    {
        Task<string> Donate(DonateViewModel donate);
        Task<ICollection<Transaction>> GetDonations();
        Task VerifyDonation(string reference);
    }
}
