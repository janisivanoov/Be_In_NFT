namespace TwitterSystem.Web.Infrastructure.Helpers
{
    using System.Web.Mvc;

    public static class Bootstrap
    {
        public static MvcHtmlString BootstrapSubmitButton(this HtmlHelper helper, string value, object htmlAttributes = null)
        {
            var submitButton = new TagBuilder("button");
            submitButton.AddCssClass("btn btn-primary");
            submitButton.Attributes.Add("type", "submit");
            submitButton.InnerHtml = value;
            submitButton.ApplyAttributes(htmlAttributes);

            return new MvcHtmlString(submitButton.ToString());
        }

        private static void ApplyAttributes(this TagBuilder tagBuilder, object htmlAttributes)
        {
            if (htmlAttributes != null)
            {
                var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
                tagBuilder.MergeAttributes(attributes);
            }
        }
    }
}
