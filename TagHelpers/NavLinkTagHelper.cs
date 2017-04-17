using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace mmmsl.TagHelpers
{
    public class NavLinkTagHelper : NavTagHelperAbstract
    {
        public NavLinkTagHelper(IHtmlGenerator generator)
            : base(generator)
        {
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            base.Process(context, output);

            output.TagName = "a";

            if (ShouldBeActive()) {
                MakeActive(output);
            }
        }
    }
}