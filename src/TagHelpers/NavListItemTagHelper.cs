using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Linq;

namespace mmmsl.TagHelpers
{
    public class NavListItemTagHelper : NavTagHelperAbstract
    {
        public NavListItemTagHelper(IHtmlGenerator generator)
            : base(generator)
        {
        }

        public async override void Process(TagHelperContext context, TagHelperOutput output)
        {
            base.Process(context, output);

            var childContent = await output.GetChildContentAsync();
            var content = childContent.GetContent();
            
            output.TagName = "li";
            var hrefAttr = output.Attributes.FirstOrDefault(a => a.Name == "href");

            if (hrefAttr != null) {
                output.Content.SetHtmlContent($@"<a class=""nav-link"" href=""{hrefAttr.Value}"">{content}</a>");
                output.Attributes.Remove(hrefAttr);
            }
            else {
                output.Content.SetHtmlContent(content);
            }

            if (ShouldBeActive()) {
                MakeActive(output);
            }
        }
    }
}