using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mmmsl.Models;
using System.Threading.Tasks;

namespace mmmsl.Controllers
{
    public class StandingsController : Controller
    {
        private readonly MmmslDatabase database;

        public StandingsController(MmmslDatabase database)
        {
            this.database = database;
        }

        public async Task<IActionResult> Index(string divisionId)
        {
            var division = await database.Divisions
                .SingleOrDefaultAsync(d => d.Id == divisionId);

            if (division == null) {
                return NotFound();
            }

            return View(new StandingsIndexModel {
                Division = division
            });
        }
    }
}