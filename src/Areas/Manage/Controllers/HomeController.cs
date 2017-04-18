using Microsoft.AspNetCore.Mvc;

namespace mmmsl.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class HomeController : AdminController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}