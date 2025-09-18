using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace websitebenhvien.Helper
{
    public static class OptimizedImageHelper
    {
        public static IHtmlContent ResponsiveImage(this IHtmlHelper htmlHelper, string imagePath, string altText, string cssClass = "", bool lazy = true, int quality = 85)
        {
            if (string.IsNullOrEmpty(imagePath))
                return new HtmlString("");

            var imageExtension = Path.GetExtension(imagePath).ToLower();
            var imageNameWithoutExt = Path.GetFileNameWithoutExtension(imagePath);
            var imageDirPath = Path.GetDirectoryName(imagePath);

            // Generate responsive image sizes
            var sizes = new[] { 320, 480, 768, 1024, 1200 };
            var srcset = new List<string>();
            var webpSrcset = new List<string>();

            foreach (var size in sizes)
            {
                var resizedPath = $"{imageDirPath}/{imageNameWithoutExt}_{size}w{imageExtension}";
                var webpPath = $"{imageDirPath}/{imageNameWithoutExt}_{size}w.webp";
                
                srcset.Add($"{resizedPath} {size}w");
                webpSrcset.Add($"{webpPath} {size}w");
            }

            var pictureBuilder = new TagBuilder("picture");
            
            // WebP source
            var webpSource = new TagBuilder("source");
            webpSource.Attributes["type"] = "image/webp";
            webpSource.Attributes["srcset"] = string.Join(", ", webpSrcset);
            webpSource.Attributes["sizes"] = "(max-width: 320px) 320px, (max-width: 480px) 480px, (max-width: 768px) 768px, (max-width: 1024px) 1024px, 1200px";

            // Fallback source
            var fallbackSource = new TagBuilder("source");
            fallbackSource.Attributes["srcset"] = string.Join(", ", srcset);
            fallbackSource.Attributes["sizes"] = "(max-width: 320px) 320px, (max-width: 480px) 480px, (max-width: 768px) 768px, (max-width: 1024px) 1024px, 1200px";

            // IMG tag
            var img = new TagBuilder("img");
            img.Attributes["src"] = imagePath; // Fallback
            img.Attributes["alt"] = altText;
            img.Attributes["decoding"] = "async";
            
            if (lazy)
            {
                img.Attributes["loading"] = "lazy";
            }
            
            if (!string.IsNullOrEmpty(cssClass))
            {
                img.Attributes["class"] = cssClass;
            }

            // Build the picture element  
            var html = $"<picture>{webpSource}{fallbackSource}{img}</picture>";
            
            return new HtmlString(html);
        }

        public static IHtmlContent OptimizedImage(this IHtmlHelper htmlHelper, string imagePath, string altText, int width = 0, int height = 0, string cssClass = "", bool lazy = true)
        {
            if (string.IsNullOrEmpty(imagePath))
                return new HtmlString("");

            var img = new TagBuilder("img");
            
            // Use WebP if available, fallback to original
            var webpPath = Path.ChangeExtension(imagePath, ".webp");
            
            img.Attributes["src"] = imagePath;
            img.Attributes["alt"] = altText;
            img.Attributes["decoding"] = "async";
            
            if (width > 0)
                img.Attributes["width"] = width.ToString();
                
            if (height > 0)
                img.Attributes["height"] = height.ToString();
            
            if (lazy)
                img.Attributes["loading"] = "lazy";
                
            if (!string.IsNullOrEmpty(cssClass))
                img.Attributes["class"] = cssClass;

            return new HtmlString(img.ToString());
        }

        public static IHtmlContent CriticalImage(this IHtmlHelper htmlHelper, string imagePath, string altText, int width = 0, int height = 0, string cssClass = "")
        {
            // For above-the-fold images - no lazy loading
            return OptimizedImage(htmlHelper, imagePath, altText, width, height, cssClass, false);
        }
    }
}
