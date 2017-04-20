using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using mmmsl.Areas.Manage.Models;
using mmmsl.Models;
using System.Linq;
using System.Threading.Tasks;

namespace mmmsl.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class TeamsController : AdminController
    {
        private readonly MmmslDatabase database;

        public TeamsController(MmmslDatabase database)
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

            var teams = database.Teams
                .Where(team => team.DivisionId == id)
                .Include(team => team.Manager)
                .OrderBy(team => team.Name);

            return PaginatedIndex(await PaginatedList<Team>.CreateAsync(teams, page ?? 1, DefaultPageSize));
        }

        public async Task<IActionResult> Create(string divisionId)
        {
            var model = await CreateEditTeamModelAsync(new Team {
                DivisionId = divisionId
            });

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Team team)
        {
            var model = await CreateEditTeamModelAsync(team);

            if (!ModelState.IsValid) {
                return View(model);
            }

            await database.Teams.AddAsync(team);
            await database.SaveChangesAsync();

            return RedirectToAction("Index", new { id = model.Team.DivisionId });
        }

        public async Task<IActionResult> Edit(int id)
        {
            var team = await database.Teams
                .Include(t => t.Division)
                .SingleOrDefaultAsync(t => t.Id == id);

            if (team == null) {
                return NotFound();
            }

            var model = await CreateEditTeamModelAsync(team);
            return View(model);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int id)
        {
            database.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
            
            var teamToUpdate = await database.Teams
                .Include(t => t.Division)
                .SingleOrDefaultAsync(p => p.Id == id);

            if (teamToUpdate == null) {
                return NotFound();
            }
            
            var didModelUpdate = await TryUpdateModelAsync<Team>(teamToUpdate, "Team",
                t => t.Id,
                t => t.Name,
                t => t.ManagerId);

            if (didModelUpdate) {
                try {
                    await database.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                catch (DbUpdateException) {
                    ModelState.AddModelError("", ErrorMessages.Database);
                }
            }

            return View(teamToUpdate);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var teamToDelete = await database.Teams.SingleOrDefaultAsync(p => p.Id == id);
            var divisionId = teamToDelete?.DivisionId;

            if (teamToDelete == null) {
                return RedirectToAction("Index");
            }

            try {
                database.Teams.Remove(teamToDelete);
                await database.SaveChangesAsync();
            }
            catch (DbUpdateException) {
                ModelState.AddModelError("", ErrorMessages.Database);
            }

            return RedirectToAction("Index", new { id = divisionId });
        }

        [Route("manage/teams/{id}/players/json")]
        public async Task<IActionResult> GetPlayers(int id)
        {
            if (id == 0) {
                return BadRequest();
            }

            var team = await database.Teams
                .Include(t => t.Roster)
                .ThenInclude(roster => roster.Profile)
                .SingleOrDefaultAsync(t => t.Id == id);
            
            return Json(team.Roster.Select(roster => roster.Profile));
        }

        private async Task<EditTeamModel> CreateEditTeamModelAsync(Team team = null)
        {
            var divisions = await database.Divisions
                .OrderBy(division => division.Name)
                .Select(division => new SelectListItem {
                    Value = division.Id,
                    Text = division.Name
                })
                .ToListAsync();
            
            var profiles = await database.Profiles
                .OrderBy(profile => profile.FirstName)
                .ThenBy(profile => profile.LastName)
                .Select(profile => new SelectListItem {
                    Value = profile.Id.ToString(),
                    Text = profile.FullName
                })
                .ToListAsync();

            return new EditTeamModel {
                Team = team ?? new Team(),
                Divisions = divisions,
                Profiles = profiles
            };
        }
    }
}