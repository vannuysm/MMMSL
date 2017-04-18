using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace mmmsl.Areas.Manage.Controllers
{
    [Authorize("CanManageLeague")]
    public class AdminController : Controller
    {
    }
}