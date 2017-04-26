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
                return View(model);
            }

            UpdateTeamManagers(model.Managers, model.Team);

            await database.Teams.AddAsync(model.Team);
            await database.SaveChangesAsync();

            return RedirectToAction("Index", new { id = model.Team.DivisionId });
        }

        public async Task<IActionResult> Edit(int id)
        {
            var team = await database.Teams
                .Include(t => t.Division)
                .Include(t => t.Managers).ThenInclude(teamManager => teamManager.Profile)
                .SingleOrDefaultAsync(t => t.Id == id);

            if (team == null) {
                return NotFound();
            }

            var model = await CreateEditTeamModelAsync(team);
            return View(model);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int id, List<int> managers)
        {
            database.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
            
            var teamToUpdate = await database.Teams
                .Include(t => t.Division)
                .Include(t => t.Managers)
                .SingleOrDefaultAsync(p => p.Id == id);

            if (teamToUpdate == null) {
                return NotFound();
            }
            
            var didModelUpdate = await TryUpdateModelAsync(teamToUpdate, "Team", t => t.Name);

            if (didModelUpdate) {
                UpdateTeamManagers(managers, teamToUpdate);

                try {
                    await database.SaveChangesAsync();
                    return RedirectToAction("Index", new { id = teamToUpdate.DivisionId });
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

        private void UpdateTeamManagers(List<int> managers, Team team)
        {
            if (managers == null) {
                team.Managers = new List<TeamManager>();
                return;
            }

            var selectedManagers = new HashSet<int>(managers);
            var currentManagers = new HashSet<int>(team.Managers.Select(teamManager => teamManager.ProfileId));

            if (selectedManagers.SetEquals(currentManagers)) {
                return;
            }

            team.Managers.RemoveAll(teamManager => {
                return currentManagers.Contains(teamManager.ProfileId) && !selectedManagers.Contains(teamManager.ProfileId);
            });

            var managersToAdd = managers
                .Where(manager => !currentManagers.Contains(manager))
                .Select(manager => new TeamManager {
                    ProfileId = manager,
                    TeamId = team.Id
                });

            if (managersToAdd.Any()) {
                team.Managers.AddRange(managersToAdd);
            }
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