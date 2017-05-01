using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System;

namespace mmmsl.Areas.Manage.ViewComponents
{
    public class AlertsViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            if (!TempData.ContainsKey("Errors")) {
                return View();
            }

            var errors = JsonConvert.DeserializeObject<List<string>>(TempData["Errors"].ToString());

            if (errors.Any()) {
                return View("AlertErrors", errors);
            }

            var action = TempData["Action"].ToString();
            var operationText = action.IndexOf("delete", StringComparison.OrdinalIgnoreCase) >= 0 ? "deleted from" : "saved to";

            return View("AlertSuccess", operationText);
        }
    }
}
