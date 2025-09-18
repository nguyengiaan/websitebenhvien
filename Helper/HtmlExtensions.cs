using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace websitebenhvien.Helper
{
    public static class HtmlExtensions
    {
        public static IHtmlContent ResponsiveImage(this IHtmlHelper htmlHelper, string imagePath, string altText, string cssClasses = "", object? htmlAttributes = null)
        {
            if (string.IsNullOrEmpty(imagePath))
            {
                return new HtmlString($"<img src=\"/Images/placeholder.jpg\" alt=\"{altText}\" class=\"{cssClasses}\" style=\"width: 100%; height: auto;\">");
            }

            var fileInfo = new FileInfo(imagePath);
            var nameWithoutExt = Path.GetFileNameWithoutExtension(fileInfo.Name);
            var extension = fileInfo.Extension.ToLower();

            // Build attributes string
            var attributes = "";
            if (htmlAttributes != null)
            {
                var props = htmlAttributes.GetType().GetProperties();
                attributes = string.Join(" ", props.Select(p => 
                    $"{p.Name.Replace('_', '-')}=\"{p.GetValue(htmlAttributes)}\""));
            }

            var html = $@"
<picture class=""{cssClasses}"">
    <source type=""image/webp"" 
            srcset=""/Resources/{nameWithoutExt}_320w.webp 320w,
                    /Resources/{nameWithoutExt}_640w.webp 640w,
                    /Resources/{nameWithoutExt}_768w.webp 768w,
                    /Resources/{nameWithoutExt}_1024w.webp 1024w,
                    /Resources/{nameWithoutExt}_1200w.webp 1200w""
            sizes=""(max-width: 768px) 100vw, (max-width: 1200px) 50vw, 33vw"">
    <source type=""image/jpeg""
            srcset=""/Resources/{nameWithoutExt}_320w.jpg 320w,
                    /Resources/{nameWithoutExt}_640w.jpg 640w,
                    /Resources/{nameWithoutExt}_768w.jpg 768w,
                    /Resources/{nameWithoutExt}_1024w.jpg 1024w,
                    /Resources/{nameWithoutExt}_1200w.jpg 1200w""
            sizes=""(max-width: 768px) 100vw, (max-width: 1200px) 50vw, 33vw"">
    <img src=""/Resources/{imagePath}"" 
         alt=""{altText}"" 
         loading=""lazy"" 
         style=""width: 100%; height: auto;""
         {attributes}>
</picture>";

            return new HtmlString(html);
        }

        public static IHtmlContent OptimizedImage(this IHtmlHelper htmlHelper, string imagePath, string altText, int? width = null, int? height = null, string cssClasses = "")
        {
            if (string.IsNullOrEmpty(imagePath))
            {
                return new HtmlString($"<img src=\"/Images/placeholder.jpg\" alt=\"{altText}\" class=\"{cssClasses}\">");
            }

            var queryParams = "";
            if (width.HasValue || height.HasValue)
            {
                var paramList = new List<string>();
                if (width.HasValue) paramList.Add($"width={width.Value}");
                if (height.HasValue) paramList.Add($"height={height.Value}");
                if (paramList.Any()) queryParams = "?" + string.Join("&", paramList);
            }

            var html = $@"<img src=""/Resources/{imagePath}{queryParams}"" 
                           alt=""{altText}"" 
                           class=""{cssClasses}""
                           loading=""lazy""
                           style=""width: 100%; height: auto;"">";

            return new HtmlString(html);
        }
    }
}
