using CiftlikYonetimiYeni.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CiftlikYonetimiYeni.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly EmailService _emailService;

        public HomeController(EmailService emailService)
        {
            _emailService = emailService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> SendEmail()
        {
            await _emailService.SendEmailAsync("algoritmauzmani@gmail.com", "Test Subject", "Test Body");
            return View();
        }
    }
}
