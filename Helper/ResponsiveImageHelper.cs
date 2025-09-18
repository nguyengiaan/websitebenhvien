using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace websitebenhvien.Helper
{
    public class ResponsiveImageHelper
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<ResponsiveImageHelper> _logger;

        public ResponsiveImageHelper(IWebHostEnvironment environment, ILogger<ResponsiveImageHelper> logger)
        {
            _environment = environment;
            _logger = logger;
        }

        public string GenerateResponsiveImages(string originalPath, int[]? sizes = null)
        {
            sizes ??= new[] { 320, 640, 768, 1024, 1200, 1920 };

            try
            {
                var uploadsPath = Path.Combine(_environment.ContentRootPath, "Uploads");
                var fullPath = Path.Combine(uploadsPath, originalPath);
                
                if (!File.Exists(fullPath))
                {
                    _logger.LogWarning($"Image file not found: {fullPath}");
                    return originalPath;
                }

                var fileInfo = new FileInfo(fullPath);
                var nameWithoutExt = Path.GetFileNameWithoutExtension(fileInfo.Name);
                var extension = fileInfo.Extension.ToLower();

                using var originalImage = Image.FromFile(fullPath);
                var originalWidth = originalImage.Width;
                var originalHeight = originalImage.Height;

                // Generate resized versions
                foreach (var size in sizes.Where(s => s < originalWidth))
                {
                    var aspectRatio = (double)originalHeight / originalWidth;
                    var newHeight = (int)(size * aspectRatio);
                    
                    // JPEG version
                    var jpegFileName = $"{nameWithoutExt}_{size}w.jpg";
                    var jpegPath = Path.Combine(uploadsPath, jpegFileName);
                    
                    if (!File.Exists(jpegPath))
                    {
                        using var resizedImage = ResizeImage(originalImage, size, newHeight);
                        var encoder = GetEncoder(ImageFormat.Jpeg);
                        if (encoder != null)
                        {
                            resizedImage.Save(jpegPath, encoder, GetEncoderParameters(85L));
                        }
                    }
                }

                return originalPath;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error generating responsive images for: {originalPath}");
                return originalPath;
            }
        }

        private static Bitmap ResizeImage(Image image, int width, int height)
        {
            var resized = new Bitmap(width, height);
            using var graphics = Graphics.FromImage(resized);
            
            graphics.CompositingMode = CompositingMode.SourceCopy;
            graphics.CompositingQuality = CompositingQuality.HighQuality;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            
            graphics.DrawImage(image, 0, 0, width, height);
            return resized;
        }

        private static ImageCodecInfo? GetEncoder(ImageFormat format)
        {
            return ImageCodecInfo.GetImageDecoders()
                .FirstOrDefault(codec => codec.FormatID == format.Guid);
        }

        private static EncoderParameters GetEncoderParameters(long quality)
        {
            var encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, quality);
            return encoderParams;
        }

        public string GeneratePictureElement(string imagePath, string altText, string cssClasses = "", int[]? sizes = null)
        {
            if (string.IsNullOrEmpty(imagePath))
                return $"<img src=\"/Images/placeholder.jpg\" alt=\"{altText}\" class=\"{cssClasses}\">";

            sizes ??= new[] { 320, 640, 768, 1024, 1200, 1920 };
            
            var fileInfo = new FileInfo(imagePath);
            var nameWithoutExt = Path.GetFileNameWithoutExtension(fileInfo.Name);

            var pictureHtml = $"<picture class=\"{cssClasses}\">";

            // JPEG sources with different sizes
            pictureHtml += "<source type=\"image/jpeg\" srcset=\"";
            var jpegSrcset = sizes.Select(size => $"/Resources/{nameWithoutExt}_{size}w.jpg {size}w");
            pictureHtml += string.Join(", ", jpegSrcset);
            pictureHtml += "\" sizes=\"(max-width: 768px) 100vw, (max-width: 1200px) 50vw, 33vw\">";

            // Fallback img
            pictureHtml += $"<img src=\"/Resources/{imagePath}\" alt=\"{altText}\" loading=\"lazy\" style=\"width: 100%; height: auto;\">";
            pictureHtml += "</picture>";

            return pictureHtml;
        }
    }
}
