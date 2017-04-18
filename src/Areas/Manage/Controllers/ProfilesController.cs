using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mmmsl.Areas.Manage.Models;
using mmmsl.Models;
using System.Linq;
using System.Threading.Tasks;

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
            var profiles = database.Profiles
                .OrderBy(profile => profile.FirstName)
                .ThenBy(profile => profile.LastName);

            return PaginatedIndex(await PaginatedList<Profile>.CreateAsync(profiles, page ?? 1, DefaultPageSize));
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
            var profile = await database.Profiles
                .Include(p => p.Roles)
                .SingleOrDefaultAsync(p => p.Id == id);

            if (profile == null) {
                return NotFound();
            }

            return View(new EditProfileModel {
                Profile = profile,
                MakeAdministrator = profile.HasRole(AppRoles.Administrator)
            });
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int id, EditProfileModel model)
        {
            database.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
            
            var profileToUpdate = await database.Profiles
                .Include(p => p.Roles)
                .SingleOrDefaultAsync(p => p.Id == id);

            if (profileToUpdate == null) {
                return NotFound();
            }
            
            var didModelUpdate = await TryUpdateModelAsync(profileToUpdate, "Profile",
                p => p.Id,
                p => p.FirstName,
                p => p.LastName,
                p => p.Email);

            var userIsAdministrator = profileToUpdate.HasRole(AppRoles.Administrator);

            if (userIsAdministrator && !model.MakeAdministrator) {
                profileToUpdate.Roles.Remove(profileToUpdate.GetRole(AppRoles.Administrator));
                didModelUpdate = true;
            }

            if (!userIsAdministrator && model.MakeAdministrator) {
                profileToUpdate.Roles.Add(new Role {
                    Name = AppRoles.Administrator,
                    ProfileId = id
                });
                didModelUpdate = true;
            }

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