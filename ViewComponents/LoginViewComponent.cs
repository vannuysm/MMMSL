using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace mmmsl.ViewComponents
{
    public class LoginViewComponent : ViewComponent
    {
        private readonly IOptions<OpenIdConnectOptions> options;

        public LoginViewComponent(IOptions<OpenIdConnectOptions> options)
        {
            this.options = options;
        }

        public IViewComponentResult Invoke()
        {
            var lockContext = HttpContext.GenerateLockContext(options.Value);
            return View(lockContext);
        }
    }
}