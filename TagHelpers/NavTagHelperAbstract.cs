using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;
using System.Linq;

namespace mmmsl.TagHelpers
{
    public enum ActiveIndicator
    {
        Controller,
        Action,
        RouteValues
    }

    public abstract class NavTagHelperAbstract : AnchorTagHelper
    {
        public NavTagHelperAbstract(IHtmlGenerator generator)
            : base(generator)
        {
        }
        
        public ActiveIndicator ActiveIndicator { get; set; } = ActiveIndicator.Controller;

        protected bool ShouldBeActive()
        {
            string currentController = ViewContext.RouteData.Values["Controller"].ToString();
            string currentAction = ViewContext.RouteData.Values["Action"].ToString();

            if (!string.IsNullOrWhiteSpace(Controller) && Controller.ToLower() != currentController.ToLower()) {
                return false;
            }

            if (ActiveIndicator == ActiveIndicator.Controller) {
                return true;
            }

            if (!string.IsNullOrWhiteSpace(Action) && Action.ToLower() != currentAction.ToLower()) {
                return false;
            }

            if (ActiveIndicator == ActiveIndicator.Action) {
                return true;
            }

            foreach (KeyValuePair<string, string> routeValue in RouteValues) {
                if (!ViewContext.RouteData.Values.ContainsKey(routeValue.Key) || ViewContext.RouteData.Values[routeValue.Key].ToString() != routeValue.Value) {
                    return false;
                }
            }

            return true;
        }

        protected void MakeActive(TagHelperOutput output)
        {
            var classAttr = output.Attributes.FirstOrDefault(a => a.Name == "class");
            if (classAttr == null) {
                classAttr = new TagHelperAttribute("class", "active");
                output.Attributes.Add(classAttr);
            }
            else if (classAttr.Value == null || classAttr.Value.ToString().IndexOf("active") < 0) {
                output.Attributes.SetAttribute("class", classAttr.Value == null
                    ? "active"
                    : classAttr.Value.ToString() + " active");
            }
        }
    }
}