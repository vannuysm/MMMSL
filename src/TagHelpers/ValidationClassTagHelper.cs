using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace mmmsl.TagHelpers
{
    [HtmlTargetElement("*", Attributes = ValidationForAttributeName)]
    public class ValidationClassTagHelper : TagHelper
    {
        private const string ValidationForAttributeName = "bs-validation-for";
        private const string ValidationErrorClassName = "bs-validationerror-class";

        [HtmlAttributeName(ValidationForAttributeName)]
        public ModelExpression For { get; set; }

        [HtmlAttributeName(ValidationErrorClassName)]
        public string ErrorClass { get; set; } = "has-danger";

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            ViewContext.ViewData.ModelState.TryGetValue(For.Name, out var entry);
            if (entry == null || !entry.Errors.Any()) {
                return;
            }

            var classAttr = output.Attributes.FirstOrDefault(a => a.Name == "class");
            if (classAttr == null) {
                classAttr = new TagHelperAttribute("class", ErrorClass);
                output.Attributes.Add(classAttr);
            }
            else if (classAttr.Value == null || classAttr.Value.ToString().IndexOf(ErrorClass) < 0) {
                var classValue = classAttr.Value == null
                    ? ErrorClass
                    : $"{classAttr.Value.ToString()} {ErrorClass}";

                output.Attributes.SetAttribute("class", classValue);
            }
        }
    }
}