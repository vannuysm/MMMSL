using Microsoft.AspNetCore.Mvc;

namespace mmmsl.Controllers
{
    [Route("league-information")]
    public class LeagueInformationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}