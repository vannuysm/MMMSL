using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mmmsl.Models;

namespace mmmsl.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class ProfilesController : AdminController
    {
        private readonly MmmslDatabase database;

        public ProfilesController(MmmslDatabase database)
        {
            this.database = database;
        }

        public async Task<IActionResult> Index(int? page)
        {
            const int pageSize = 10;

            var profiles = database.Profiles
                .OrderBy(profile => profile.FirstName)
                .ThenBy(profile => profile.LastName);

            return View(await PaginatedList<Profile>.CreateAsync(profiles, page ?? 1, pageSize));
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Profile profile)
        {
            if (!ModelState.IsValid) {
                return View(profile);
            }

            await database.Profiles.AddAsync(profile);
            await database.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var profile = await database.Profiles.SingleOrDefaultAsync(p => p.Id == id);

            if (profile == null) {
                return NotFound();
            }

            return View(profile);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int id)
        {
            database.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
            
            var profileToUpdate = await database.Profiles.SingleOrDefaultAsync(p => p.Id == id);

            if (profileToUpdate == null) {
                return NotFound();
            }
            
            var didModelUpdate = await TryUpdateModelAsync<Profile>(profileToUpdate, "",
                p => p.Id,
                p => p.FirstName,
                p => p.LastName,
                p => p.Email);

            if (didModelUpdate) {
                try {
                    await database.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                catch (DbUpdateException) {
                    ModelState.AddModelError("", ErrorMessages.Database);
                }
            }

            return View(profileToUpdate);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var profileToDelete = await database.Profiles.SingleOrDefaultAsync(p => p.Id == id);

            if (profileToDelete == null) {
                return RedirectToAction("Index");
            }

            try {
                database.Profiles.Remove(profileToDelete);
                await database.SaveChangesAsync();
            }
            catch (DbUpdateException) {
                ModelState.AddModelError("", ErrorMessages.Database);
            }

            return RedirectToAction("Index");
        }
    }
}