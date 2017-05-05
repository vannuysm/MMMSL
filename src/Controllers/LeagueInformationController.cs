using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mmmsl.Models;

namespace mmmsl.Controllers
{
    [Route("league-information")]
    public class LeagueInformationController : Controller
    {
        private readonly MmmslDatabase database;

        public LeagueInformationController(MmmslDatabase database)
        {
            this.database = database;
        }

        public async Task<IActionResult> Index()
        {
            var boardMembers = await database.BoardMembers
                .Include(boardMember => boardMember.Profile)
                .OrderBy(boardMember => boardMember.Title)
                .ToListAsync();

            var model = new LeagueInformationIndexModel {
                BoardMembers = boardMembers
            };

            return View(model);
        }
    }
}