using Microsoft.AspNetCore.Mvc;

namespace SIMS_Project.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
