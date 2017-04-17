using System.Threading.Tasks;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mmmsl.Models;

namespace mmmsl.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class DivisionsController : Controller
    {
        private readonly MmmslDatabase database;

        public DivisionsController(MmmslDatabase database)
        {
            this.database = database;
        }

        public async Task<IActionResult> Index(int? page)
        {
            const int pageSize = 10;
            return View(await PaginatedList<Division>.CreateAsync(database.Divisions, page ?? 1, pageSize));
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
    }
}