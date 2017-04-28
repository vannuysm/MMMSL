using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using mmmsl.Areas.Manage.Models;
using mmmsl.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

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
                .Include(team => team.Managers).ThenInclude(manager => manager.Profile)
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
        public async Task<IActionResult> Create(EditTeamModel model)
        {
            if (!ModelState.IsValid) {
                return View(await CreateEditTeamModelAsync(model.Team));
            }
            
            await database.Teams.AddAsync(model.Team);
            await database.SaveChangesAsync();

            return RedirectToAction("Index", new { id = model.Team.DivisionId });
        }

        public async Task<IActionResult> Edit(int id)
        {
            var team = await database.Teams
                .Include(t => t.Division)
                .Include(t => t.Managers).ThenInclude(teamManager => teamManager.Profile)
                .Include(t => t.Roster).ThenInclude(player => player.Profile)
                .SingleOrDefaultAsync(t => t.Id == id);

            if (team == null) {
                return NotFound();
            }

            var model = await CreateEditTeamModelAsync(team);
            return View(model);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int id, EditTeamPostModel model)
        {
            database.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
            
            var teamToUpdate = await database.Teams
                .Include(t => t.Division)
                .Include(t => t.Managers).ThenInclude(teamManager => teamManager.Profile)
                .Include(t => t.Roster).ThenInclude(player => player.Profile)
                .SingleOrDefaultAsync(p => p.Id == id);

            if (teamToUpdate == null) {
                return NotFound();
            }
            
            var didModelUpdate = await TryUpdateModelAsync(teamToUpdate, "Team", t => t.Name);

            if (didModelUpdate) {
                try {
                    await database.SaveChangesAsync();
                    return RedirectToAction("Index", new { id = teamToUpdate.DivisionId });
                }
                catch (DbUpdateException) {
                    ModelState.AddModelError("", ErrorMessages.Database);
                }
            }

            var failedModel = await CreateEditTeamModelAsync(teamToUpdate);
            return View(failedModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Manager(int id, int managerId)
        {
            database.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;

            var team = await database.Teams
                    .Include(t => t.Managers)
                    .SingleOrDefaultAsync(g => g.Id == id);

            if (team == null) {
                return NotFound();
            }

            if (team.Managers.Any(manager => manager.ProfileId == managerId)) {
                return RedirectToAction("Edit", new { id = id });
            }

            team.Managers.Add(new TeamManager {
                TeamId = id,
                ProfileId = managerId
            });

            try {
                await database.SaveChangesAsync();
            }
            catch (DbUpdateException) {
                ModelState.AddModelError("", ErrorMessages.Database);
            }

            return RedirectToAction("Edit", new { id = id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteManager(int id, int managerId)
        {
            var managerToDelete = await database.TeamManagers.SingleOrDefaultAsync(manager => manager.TeamId == id && manager.ProfileId == managerId);

            if (managerToDelete == null) {
                return RedirectToAction("Edit", new { id = id });
            }

            try {
                database.TeamManagers.Remove(managerToDelete);
                await database.SaveChangesAsync();
            }
            catch (DbUpdateException) {
                ModelState.AddModelError("", ErrorMessages.Database);
            }

            return RedirectToAction("Edit", new { id = id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Roster(int id, int playerId)
        {
            database.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;

            var team = await database.Teams
                    .Include(t => t.Roster)
                    .SingleOrDefaultAsync(g => g.Id == id);

            if (team == null) {
                return NotFound();
            }

            if (team.Roster.Any(manager => manager.ProfileId == playerId)) {
                return RedirectToAction("Edit", new { id = id });
            }

            team.Roster.Add(new RosterPlayer {
                TeamId = id,
                ProfileId = playerId
            });

            try {
                await database.SaveChangesAsync();
            }
            catch (DbUpdateException) {
                ModelState.AddModelError("", ErrorMessages.Database);
            }

            return RedirectToAction("Edit", new { id = id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePlayer(int id, int playerId)
        {
            var playerToDelete = await database.RosterPlayers.SingleOrDefaultAsync(player => player.TeamId == id && player.ProfileId == playerId);

            if (playerToDelete == null) {
                return RedirectToAction("Edit", new { id = id });
            }

            try {
                database.RosterPlayers.Remove(playerToDelete);
                await database.SaveChangesAsync();
            }
            catch (DbUpdateException) {
                ModelState.AddModelError("", ErrorMessages.Database);
            }

            return RedirectToAction("Edit", new { id = id });
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