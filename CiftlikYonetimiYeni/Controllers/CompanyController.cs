using Microsoft.AspNetCore.Mvc;

namespace CiftlikYonetimiYeni.Controllers
{
    public class CompanyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CompanyList()
        {
            return View();
        }
    }
}
