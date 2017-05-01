using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace mmmsl.Areas.Manage
{
    public class StashErrorsInTempDataAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);

            var controller = context.Controller as Controller;

            if (controller == null) {
                return;
            }

            var errors = context.ModelState
                .Where(ms => ms.Value.Errors.Any())
                .SelectMany(ms => ms.Value.Errors.Select(error => error.ErrorMessage));

            controller.TempData["Errors"] = JsonConvert.SerializeObject(errors);
            controller.TempData["Action"] = controller.ControllerContext?.ActionDescriptor?.ActionName;
        }
    }
}
