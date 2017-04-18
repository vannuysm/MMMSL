using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mmmsl.Models;
using System.Linq;
using System.Threading.Tasks;

namespace mmmsl.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class GamesController : AdminController
    {
        private readonly MmmslDatabase database;

        public GamesController(MmmslDatabase database)
        {
            this.database = database;
        }

        public async Task<IActionResult> Index(string id, int? page)
        {
            if (string.IsNullOrWhiteSpace(id)) {
                return RedirectToAction("Index", new {
                    id = (await database.Divisions.FirstAsync())?.Id
                });
            }

            var games = database.Games
                .Where(game => game.DivisionId == id)
                .Include(game => game.HomeTeam)
                .Include(game => game.AwayTeam)
                .Include(game => game.Goals)
                .OrderBy(game => game.DateAndTime);

            return PaginatedIndex(await PaginatedList<Game>.CreateAsync(games, page ?? 1, DefaultPageSize));
        }
    }
}