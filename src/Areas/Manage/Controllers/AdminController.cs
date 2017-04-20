using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mmmsl.Models;

namespace mmmsl.Areas.Manage.Controllers
{
    [Authorize("CanManageLeague")]
    public class AdminController : Controller
    {
        protected const int DefaultPageSize = 20;

        protected IActionResult PaginatedIndex<T>(PaginatedList<T> list)
        {
            if (list.TotalPages > 0 && list.PageIndex > list.TotalPages) {
                return RedirectToAction("Index", new {
                    id = ControllerContext.RouteData.Values["id"],
                    page = list.TotalPages
                });
            }

            if (list.PageIndex < 1) {
                return RedirectToAction("Index", new {
                    id = ControllerContext.RouteData.Values["id"],
                    page = 1
                });
            }

            return View(list);
        }
    }
}