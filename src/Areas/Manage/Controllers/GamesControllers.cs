using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mmmsl.Models;
using System.Linq;
using System.Threading.Tasks;
using mmmsl.Areas.Manage.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

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

        public async Task<IActionResult> Create(string divisionId)
        {
            var model = await CreateEditGameModelAsync(new Game {
                DivisionId = divisionId
            });

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Game game)
        {
            var model = await CreateEditGameModelAsync(game);

            if (!ModelState.IsValid) {
                return View(model);
            }

            await database.Games.AddAsync(game);
            await database.SaveChangesAsync();

            return RedirectToAction("Index", new { id = model.Game.DivisionId });
        }

        public async Task<IActionResult> Edit(int id)
        {
            var game = await database.Games
                .Include(g => g.Division)
                .Include(g => g.HomeTeam)
                .Include(g => g.AwayTeam)
                .SingleOrDefaultAsync(g => g.Id == id);

            if (game == null) {
                return NotFound();
            }

            var model = await CreateEditGameModelAsync(game);
            return View(model);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int id)
        {
            database.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
            
            var gameToUpdate = await database.Games
                .Include(g => g.Division)
                .SingleOrDefaultAsync(p => p.Id == id);

            if (gameToUpdate == null) {
                return NotFound();
            }
            
            var didModelUpdate = await TryUpdateModelAsync<Game>(gameToUpdate, "Game",
                g => g.Id,
                g => g.DivisionId,
                g => g.HomeTeamId,
                g => g.AwayTeamId,
                g => g.DateAndTime);

            if (didModelUpdate) {
                try {
                    await database.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                catch (DbUpdateException) {
                    ModelState.AddModelError("", ErrorMessages.Database);
                }
            }

            return View(gameToUpdate);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var gameToDelete = await database.Games.SingleOrDefaultAsync(p => p.Id == id);
            var divisionId = gameToDelete?.DivisionId;

            if (gameToDelete == null) {
                return RedirectToAction("Index");
            }

            try {
                database.Games.Remove(gameToDelete);
                await database.SaveChangesAsync();
            }
            catch (DbUpdateException) {
                ModelState.AddModelError("", ErrorMessages.Database);
            }

            return RedirectToAction("Index", new { id = divisionId });
        }

        private async Task<EditGameModel> CreateEditGameModelAsync(Game game = null)
        {
            var divisions = await database.Divisions
                .OrderBy(division => division.Name)
                .Select(division => new SelectListItem {
                    Value = division.Id,
                    Text = division.Name
                })
                .ToListAsync();

            if (game == null || string.IsNullOrEmpty(game.DivisionId)) {
                game = new Game {
                    DivisionId = (await database.Divisions.FirstAsync())?.Id
                };
            }
            
            var model = new EditGameModel {
                Game = game,
                Divisions = divisions
            };

            if (!string.IsNullOrWhiteSpace(model.Game.DivisionId)) {
                model.Teams = await database.Teams
                    .Where(team => team.DivisionId == model.Game.DivisionId)
                    .OrderBy(team => team.Name)
                    .Select(team => new SelectListItem {
                        Value = team.Id.ToString(),
                        Text = team.Name
                    })
                    .ToListAsync();
            }

            return model;
        }
    }
}