using Microsoft.AspNetCore.Mvc;

namespace mmmsl.ViewComponents
{
    public class ManagementNavigationViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}