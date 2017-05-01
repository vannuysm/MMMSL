using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using mmmsl.Areas.Manage.Models;
using mmmsl.Models;
using System.Collections.Generic;
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
                .Include(game => game.Field).ThenInclude(field => field.Location)
                .OrderBy(game => game.DateAndTime.Date)
                .ThenBy(game => game.DateAndTime.TimeOfDay);

            return PaginatedIndex(await PaginatedList<Game>.CreateAsync(games, page ?? 1, DefaultPageSize));
        }

        public async Task<IActionResult> Create(string divisionId)
        {
            return View(await CreateEditGameModelAsync(new Game { DivisionId = divisionId }));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [StashErrorsInTempData]
        public async Task<IActionResult> Create(Game game)
        {
            var model = await CreateEditGameModelAsync(game);

            if (!ModelState.IsValid) {
                return View(model);
            }

            await database.Games.AddAsync(game);
            try {
                await database.SaveChangesAsync();
                return RedirectToAction("Index", new { id = model.Game.DivisionId });
            }
            catch (DbUpdateException ex) {
                ModelState.AddDatabaseError(ex);
            }

            return RedirectToAction("Index", new { id = model.Game.DivisionId });
        }

        public async Task<IActionResult> Edit(int id)
        {
            var game = await database.Games
                .Include(g => g.Division)
                .Include(g => g.HomeTeam)
                .Include(g => g.AwayTeam)
                .Include(g => g.Goals).ThenInclude(g => g.Player)
                .Include(g => g.Penalties).ThenInclude(g => g.Player)
                .Include(g => g.Penalties).ThenInclude(g => g.PenaltyCard)
                .SingleOrDefaultAsync(g => g.Id == id);

            if (game == null) {
                return NotFound();
            }
            
            return View(await CreateEditGameModelAsync(game));
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
            
            var didModelUpdate = await TryUpdateModelAsync(gameToUpdate, "Game",
                g => g.HomeTeamId,
                g => g.AwayTeamId,
                g => g.DateAndTime,
                g => g.Status,
                g => g.FieldId);

            if (didModelUpdate) {
                try {
                    await database.SaveChangesAsync();
                    return RedirectToAction("Index", new { id = gameToUpdate.DivisionId });
                }
                catch (DbUpdateException ex) {
                    ModelState.AddDatabaseError(ex);
                }
            }

            return View(gameToUpdate);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [StashErrorsInTempData]
        public async Task<IActionResult> Delete(int id)
        {
            var gameToDelete = await database.Games.SingleOrDefaultAsync(p => p.Id == id);
            var divisionId = gameToDelete?.DivisionId;

            if (gameToDelete == null) {
                return NotFound();
            }

            try {
                database.Games.Remove(gameToDelete);
                await database.SaveChangesAsync();
            }
            catch (DbUpdateException ex) {
                ModelState.AddDatabaseError(ex);
            }

            return RedirectToAction("Index", new { id = divisionId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [StashErrorsInTempData]
        public async Task<IActionResult> Goal(int id, EditGoalModel model)
        {
            database.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;

            var game = await database.Games
                   .Include(g => g.HomeTeam)
                   .Include(g => g.AwayTeam)
                   .Include(g => g.Goals)
                   .ThenInclude(g => g.Player)
                   .SingleOrDefaultAsync(g => g.Id == id);

            if (game == null) {
                return NotFound();
            }
            
            if (game.Goals.Any(g => g.PlayerId == model.PlayerId)) {
                var goal = game.Goals.Single(g => g.PlayerId == model.PlayerId);
                goal.Count = model.Count;
            }
            else {
                game.Goals.Add(new Goal {
                    GameId = id,
                    TeamId = model.TeamId,
                    PlayerId = model.PlayerId,
                    Count = model.Count
                });
            }
            
            try {
                await database.SaveChangesAsync();
            }

            catch (DbUpdateException ex) {
                ModelState.AddDatabaseError(ex);
            }

            var redirectUrl = Url.Action("Edit", new { id = game.Id });
            return Redirect($"{redirectUrl}#team-{model.TeamId}");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [StashErrorsInTempData]
        public async Task<IActionResult> DeleteGoal(int id)
        {
            var goalToDelete = await database.Goals.SingleOrDefaultAsync(goal => goal.Id == id);

            if (goalToDelete == null) {
                return NotFound();
            }
            
            try {
                database.Goals.Remove(goalToDelete);
                await database.SaveChangesAsync();
            }
            catch (DbUpdateException ex) {
                ModelState.AddDatabaseError(ex);
            }
            var redirectUrl = Url.Action("Edit", new { id = goalToDelete?.GameId });
            return Redirect($"{redirectUrl}#team-{goalToDelete?.TeamId}");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [StashErrorsInTempData]
        public async Task<IActionResult> Penalty(int id, EditPenaltyModel model)
        {
            database.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;

            var game = await database.Games
                   .Include(g => g.HomeTeam)
                   .Include(g => g.AwayTeam)
                   .Include(g => g.Penalties).ThenInclude(g => g.Player)
                   .Include(g => g.Penalties).ThenInclude(g => g.PenaltyCard)
                   .SingleOrDefaultAsync(g => g.Id == id);

            if (game == null) {
                return NotFound();
            }
            
            game.Penalties.Add(new Penalty {
                GameId = id,
                TeamId = model.TeamId,
                PlayerId = model.PlayerId,
                MisconductCode = model.MisconductCode
            });
            
            try {
                await database.SaveChangesAsync();
            }
            catch (DbUpdateException ex) {
                ModelState.AddDatabaseError(ex);
            }

            var redirectUrl = Url.Action("Edit", new { id = game.Id });
            return Redirect($"{redirectUrl}#team-{model.TeamId}");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [StashErrorsInTempData]
        public async Task<IActionResult> DeletePenalty(int id)
        {
            var penaltyToDelete = await database.Penalties.SingleOrDefaultAsync(penalty => penalty.Id == id);

            if (penaltyToDelete == null) {
                return NotFound();
            }
            
            try {
                database.Penalties.Remove(penaltyToDelete);
                await database.SaveChangesAsync();
            }
            catch (DbUpdateException ex) {
                ModelState.AddDatabaseError(ex);
            }

            var redirectUrl = Url.Action("Edit", new { id = penaltyToDelete?.GameId });
            return Redirect($"{redirectUrl}#team-{penaltyToDelete?.TeamId}");
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
                Divisions = divisions,
                Fields = await GetFieldSelectList()
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

            if (game.Id > 0) {
                model.HomeTeamPlayers = await GetPlayerSelectList(game.HomeTeamId);
                model.AwayTeamPlayers = await GetPlayerSelectList(game.AwayTeamId);
                model.PenaltyCards = await GetPenaltyCardSelectList();
            }

            return model;
        }

        private async Task<List<SelectListItem>> GetFieldSelectList()
        {
            var fields = await database.Fields
                .Include(field => field.Location)
                .ToListAsync();

            return fields
                .OrderBy(field => field.Description)
                .Select(field => new SelectListItem {
                    Value = field.Id.ToString(),
                    Text = field.Description
                }).ToList();
        }

        private async Task<List<SelectListItem>> GetPlayerSelectList(int teamId)
        {
            var team = await database.Teams
                    .Include(t => t.Roster)
                    .ThenInclude(t => t.Profile)
                    .SingleAsync(t => t.Id == teamId);

            return team.Roster.Select(roster => new SelectListItem {
                Value = roster.Profile.Id.ToString(),
                Text = roster.Profile.FullName
            }).ToList();
        }

        private async Task<List<SelectListItem>> GetPenaltyCardSelectList()
        {
            return await database.PenaltyDefinitions
                .Select(pd => new SelectListItem {
                    Value = pd.MisconductCode,
                    Text = pd.Title
                }).ToListAsync();
        }
    }
}
 