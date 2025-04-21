using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using FFMpegCore; // Thêm namespace này
using FFMpegCore.Enums;
using FFMpegCore.Pipes;

namespace websitebenhvien.Helper
{
    public class Uploadfile
    {
        private readonly IHttpContextAccessor _httpContextAccessor; 
        private readonly IWebHostEnvironment _hostingEnvironment;

        public Uploadfile(IHttpContextAccessor httpContextAccessor, IWebHostEnvironment hostingEnvironment)
        {
            _httpContextAccessor = httpContextAccessor;
            _hostingEnvironment = hostingEnvironment;
            
            // Cấu hình đường dẫn đến FFmpeg nếu cần
            GlobalFFOptions.Configure(options => options.BinaryFolder = "path/to/ffmpeg");
        }

        public async Task<Tuple<int, string>> SaveMedia(IFormFile file, 
                                                           bool compressImages = true,
                                                           bool compressVideos = true)
        {
            try
            {
                var contentPath = _hostingEnvironment.ContentRootPath;
                var path = Path.Combine(contentPath, "Uploads");
                
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                var ext = Path.GetExtension(file.FileName).ToLower();
                var allowedExtensions = new string[]
                {
                    ".jpg", ".jpeg", ".png", ".gif", ".bmp",
                    ".mp4", ".avi", ".mov", ".wmv", ".flv", ".mkv",
                    ".pdf", ".docx"
                };

                if (!allowedExtensions.Contains(ext))
                {
                    string msg = $"Chỉ chấp nhận các định dạng: {string.Join(", ", allowedExtensions)}";
                    return new Tuple<int, string>(0, msg);
                }

                string uniqueString = Guid.NewGuid().ToString();
                var fileName = Path.GetFileName(file.FileName);
                var newFileName = $"{Guid.NewGuid():N}_{fileName}";
                var fileWithPath = Path.Combine(path, newFileName);

                // Xử lý nén ảnh
                var imageExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
                if (compressImages && imageExtensions.Contains(ext))
                {
                    using var compressedStream = new MemoryStream(await ImageCompressor.CompressImageAsync(file.OpenReadStream()));
                    using var fileStream = new FileStream(fileWithPath, FileMode.Create);
                    await compressedStream.CopyToAsync(fileStream);
                }
                // Xử lý nén video
                else if (compressVideos && IsVideoExtension(ext))
                {
                    await CompressVideoAsync(file, fileWithPath);
                }
                else
                {
                    using var stream = new FileStream(fileWithPath, FileMode.Create);
                    await file.CopyToAsync(stream);
                }

                return new Tuple<int, string>(1, newFileName);
            }
            catch (Exception ex)
            {
                return new Tuple<int, string>(0, $"Lỗi: {ex.Message}");
            }
        }

        private bool IsVideoExtension(string extension)
        {
            var videoExtensions = new[] { ".mp4", ".avi", ".mov", ".wmv", ".flv", ".mkv" };
            return videoExtensions.Contains(extension.ToLower());
        }

        private async Task CompressVideoAsync(IFormFile videoFile, string outputPath)
        {
            // Tạo file tạm để FFmpeg xử lý
            var tempInputPath = Path.GetTempFileName();
            try
            {
                // Lưu file tạm
                using (var stream = new FileStream(tempInputPath, FileMode.Create))
                {
                    await videoFile.CopyToAsync(stream);
                }

                // Cấu hình nén video
                await FFMpegArguments
                    .FromFileInput(tempInputPath)
                    .OutputToFile(outputPath, true, options => options
                        .WithVideoCodec(VideoCodec.LibX264)
                        .WithConstantRateFactor(28) // CRF: 18-28 (thấp hơn = chất lượng cao hơn)
                        .WithAudioCodec(AudioCodec.Aac)
                        .WithFastStart())
                    .ProcessAsynchronously();
            }
            finally
            {
                // Xóa file tạm
                if (File.Exists(tempInputPath))
                {
                    File.Delete(tempInputPath);
                }
            }
        }

        public bool DeleteMedia(string fileName)
        {
            try
            {
                var contentPath = _hostingEnvironment.ContentRootPath;
                var path = Path.Combine(contentPath, "Uploads");
                var filePath = Path.Combine(path, fileName);

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }

    public static class ImageCompressor
    {
        public static async Task<byte[]> CompressImageAsync(Stream inputStream, 
                                                          int quality = 75, 
                                                          int maxWidth = 1024, 
                                                          int maxHeight = 1024)
        {
            using var image = await Image.LoadAsync(inputStream);

            image.Mutate(x => x.Resize(new ResizeOptions
            {
                Mode = ResizeMode.Max,
                Size = new Size(maxWidth, maxHeight)
            }));

            var encoder = new JpegEncoder { Quality = quality };

            using var outputStream = new MemoryStream();
            await image.SaveAsync(outputStream, encoder);
            return outputStream.ToArray();
        }
    }
}