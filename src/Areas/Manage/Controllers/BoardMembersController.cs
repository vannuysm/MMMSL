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
    public class BoardMembersController : AdminController
    {
        private readonly MmmslDatabase database;

        public BoardMembersController(MmmslDatabase database)
        {
            this.database = database;
        }

        public async Task<IActionResult> Index(int? page)
        {
            var boardMembers = database.BoardMembers.Include(boardMember => boardMember.Profile);
            return PaginatedIndex(await PaginatedList<BoardMember>.CreateAsync(boardMembers, page ?? 1, DefaultPageSize));
        }

        public async Task<IActionResult> Create()
        {
            var model = await CreateEditBoardMemberModelAsync(new BoardMember());
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [StashErrorsInTempData]
        public async Task<IActionResult> Create(EditBoardMemberModel model)
        {
            if (!ModelState.IsValid) {
                return View(await CreateEditBoardMemberModelAsync(model.BoardMember));
            }

            model.BoardMember.Title = model.BoardMember.Title.Trim();
            await database.BoardMembers.AddAsync(model.BoardMember);

            try {
                await database.SaveChangesAsync();
            }
            catch (DbUpdateException ex) {
                ModelState.AddDatabaseError(ex);
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var boardMember = await database.BoardMembers.SingleOrDefaultAsync(bm => bm.Id == id);

            if (boardMember == null) {
                return NotFound();
            }

            var model = await CreateEditBoardMemberModelAsync(boardMember);
            return View(model);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        [StashErrorsInTempData]
        public async Task<IActionResult> EditPost(int id)
        {
            database.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
            
            var boardMemberToUpdate = await database.BoardMembers.SingleOrDefaultAsync(bm => bm.Id == id);

            if (boardMemberToUpdate == null) {
                return NotFound();
            }
            
            var didModelUpdate = await TryUpdateModelAsync(boardMemberToUpdate, "BoardMember",
                bm => bm.Title,
                bm => bm.ProfileId,
                bm => bm.Email);

            if (didModelUpdate) {
                try {
                    await database.SaveChangesAsync();

                    return RedirectToAction("Index");
                }
                catch (DbUpdateException ex) {
                    ModelState.AddDatabaseError(ex);
                }
            }

            var failedModel = await CreateEditBoardMemberModelAsync(boardMemberToUpdate);
            return View(failedModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [StashErrorsInTempData]
        public async Task<IActionResult> Delete(int id)
        {
            var boardMemberToDelete = await database.BoardMembers.SingleOrDefaultAsync(bm => bm.Id == id);

            if (boardMemberToDelete == null) {
                return NotFound();
            }

            try {
                database.BoardMembers.Remove(boardMemberToDelete);
                await database.SaveChangesAsync();
            }
            catch (DbUpdateException ex) {
                ModelState.AddDatabaseError(ex);
            }

            return RedirectToAction("Index");
        }

        private async Task<EditBoardMemberModel> CreateEditBoardMemberModelAsync(BoardMember boardMember = null)
        {
            var profiles = await database.Profiles
                .OrderBy(profile => profile.FirstName)
                .ThenBy(profile => profile.LastName)
                .Select(profile => new SelectListItem {
                    Value = profile.Id.ToString(),
                    Text = profile.FullName
                })
                .ToListAsync();

            return new EditBoardMemberModel {
                BoardMember = boardMember ?? new BoardMember(),
                Profiles = profiles
            };
        }
    }
}