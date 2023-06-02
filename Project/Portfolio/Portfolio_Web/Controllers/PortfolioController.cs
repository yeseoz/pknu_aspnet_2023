using Microsoft.AspNetCore.Mvc;

namespace Portfolio_Web.Controllers
{
    public class PortfolioController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
