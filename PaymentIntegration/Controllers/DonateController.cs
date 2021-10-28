using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PaymentIntegration.Models;
using PaymentIntegration.Repository;
using PayStack.Net;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentIntegration.Controllers
{
    public class DonateController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;
        private readonly string token;

        private PayStackApi Paystack { get; set; }
        public DonateController(IConfiguration configuration, AppDbContext context)
        {
            _configuration = configuration;
            _context = context;
            token = _configuration["Payment:PaystackSK"];
            Paystack = new PayStackApi(token);
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(DonateViewModel donate)
        {
            TransactionInitializeRequest request = new()
            {
                AmountInKobo = donate.Amount * 100,
                Email = donate.Email,
                Reference = Generate().ToString(),
                Currency = "NGN",
                CallbackUrl = "http://localhost:36222/donate/verify"
            };

            TransactionInitializeResponse response = Paystack.Transactions.Initialize(request);
            if (response.Status)
            {
                var transaction = new TransactionModel()
                {
                    Amount = donate.Amount,
                    Email = donate.Email,
                    TrxRef = request.Reference,
                    Name = donate.Name,
                };
                await _context.Transactions.AddAsync(transaction);
                await _context.SaveChangesAsync();
                return Redirect(response.Data.AuthorizationUrl);
            }
            ViewData["error"] = response.Message;
            return View();
        }

        [HttpGet]
        public IActionResult Donations()
        {
            var transactions = _context.Transactions.Where(x => x.Status == true).ToList();
            ViewData["transactions"] = transactions;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Verify(string reference)
        {
            TransactionVerifyResponse response = Paystack.Transactions.Verify(reference);
            if (response.Data.Status == "success")
            {
                var transaction = _context.Transactions.Where(x => x.TrxRef == reference).FirstOrDefault();
                if (transaction != null)
                {
                    transaction.Status = true;
                    _context.Transactions.Update(transaction);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Donations");
                }
            }
            ViewData["error"] = response.Data.GatewayResponse;
            return RedirectToAction("Index");
        }

        public static int Generate()
        {
            Random rand = new Random((int)DateTime.Now.Ticks);
            return rand.Next(100000000, 999999999);
        }
    }
}
