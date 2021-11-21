using Microsoft.AspNetCore.Mvc;
using PaymentIntegration.Core.Services.Abstractions;
using PaymentIntegration.Models.ViewModels;
using System;
using System.Threading.Tasks;

namespace PaymentIntegration.Controllers
{
    public class DonateController : Controller
    {
        private readonly IDonateService _donateService;

        public DonateController(IDonateService donateService)
        {
            _donateService = donateService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(DonateViewModel donate)
        {
            try
            {
                var result = await _donateService.Donate(donate);
                return Redirect(result);
            }
            catch (Exception ex)
            {
                ViewData["error"] = ex.Message;
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Donations()
        {
            var transactions = await _donateService.GetDonations();
            ViewData["transactions"] = transactions;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Verify(string reference)
        {
            try
            {
                await _donateService.VerifyDonation(reference);
                return RedirectToAction("Donations");
            }
            catch (Exception ex)
            {
                ViewData["error"] = ex.Message;
                return RedirectToAction("Index");
            }

        }
    }
}
