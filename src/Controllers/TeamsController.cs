using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mmmsl.Models;
using System.Linq;
using System.Threading.Tasks;

namespace mmmsl.Controllers
{
    public class TeamsController : Controller
    {
        private readonly MmmslDatabase database;

        public TeamsController(MmmslDatabase database)
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

            var teams = await database.Teams
                .Where(team => team.DivisionId == divisionId)
                .Include(team => team.Managers).ThenInclude(manager => manager.Profile)
                .Include(team => team.Roster).ThenInclude(player => player.Profile)
                .OrderBy(team => team.Name)
                .ToListAsync();

            return View(new TeamsIndexModel {
                Division = division,
                Teams = teams
            });
        }
    }
}