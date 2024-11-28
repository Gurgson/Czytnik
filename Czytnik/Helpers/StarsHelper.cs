using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Globalization;

namespace Czytnik.Helpers
{
    [HtmlTargetElement("stars")]
    public class StarsHelper : TagHelper
    {
        public decimal? Rating { get; set; }
        public string ParentClass { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            decimal ratingString = (Rating == null) ? 0 : (decimal)Rating;
            var className = $"stars-rating {ParentClass}";
            output.TagName = "div";
            output.Attributes.SetAttribute("style", $"--rating: {ratingString.ToString(CultureInfo.CreateSpecificCulture("en-GB"))}");
            output.Attributes.Add("class", className);
        }
    }
}
