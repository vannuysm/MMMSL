using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mmmsl.Models;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace mmmsl.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class DivisionsController : AdminController
    {
        private readonly MmmslDatabase database;

        public DivisionsController(MmmslDatabase database)
        {
            this.database = database;
        }

        public async Task<IActionResult> Index(int? page)
        {
            return PaginatedIndex(await PaginatedList<Division>.CreateAsync(database.Divisions, page ?? 1, DefaultPageSize));
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Division division)
        {
            if (!ModelState.IsValid) {
                return View(division);
            }

            division.Name = division.Name.Trim();
            division.Id = division.Name
                .Underscore()
                .Dasherize()
                .ToLowerInvariant();

            await database.Divisions.AddAsync(division);
            await database.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(string id)
        {
            var division = await database.Divisions.SingleOrDefaultAsync(d => d.Id == id);

            if (division == null) {
                return NotFound();
            }

            return View(division);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(string id)
        {
            database.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
            
            var divisionToUpdate = await database.Divisions.SingleOrDefaultAsync(d => d.Id == id);

            if (divisionToUpdate == null) {
                return NotFound();
            }
            
            var didModelUpdate = await TryUpdateModelAsync<Division>(divisionToUpdate, "",
                d => d.Id,
                d => d.Name);

            if (didModelUpdate) {
                try {
                    await database.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                catch (DbUpdateException) {
                    ModelState.AddModelError("", ErrorMessages.Database);
                }
            }

            return View(divisionToUpdate);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var divisionToDelete = await database.Divisions.SingleOrDefaultAsync(p => p.Id == id);

            if (divisionToDelete == null) {
                return RedirectToAction("Index");
            }

            try {
                database.Divisions.Remove(divisionToDelete);
                await database.SaveChangesAsync();
            }
            catch (DbUpdateException) {
                ModelState.AddModelError("", ErrorMessages.Database);
            }

            return RedirectToAction("Index");
        }

        [Route("manage/divisions/{id}/teams/json")]
        public async Task<IActionResult> GetTeams(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) {
                return BadRequest();
            }

            var teams = await database.Teams
                .Where(team => team.DivisionId == id)
                .OrderBy(team => team.Name)
                .ToListAsync();

            return Json(teams);
        }
    }
}