using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Globalization;
using Microsoft.AspNetCore.Hosting;

namespace Czytnik.Helpers
{
    [HtmlTargetElement("profile")]
    public class ProfileHelper : TagHelper
    {
        public string? Path { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            string imgSrc = (Path == null || Path == "") ? "~/assets/img/defaultProfilePicture.png" : $"~/uploads/{Path}";
            output.TagName = "img";
            output.Attributes.SetAttribute("src", imgSrc);
        }
    }
}
