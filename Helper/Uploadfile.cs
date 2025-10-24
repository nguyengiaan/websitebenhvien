﻿using System; // Thêm
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png; 
using SixLabors.ImageSharp.Formats.Gif; 
using SixLabors.ImageSharp.Processing;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics; 

namespace websitebenhvien.Helper
{
    public class Uploadfile
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        // Dùng HashSet để tìm kiếm nhanh hơn (O(1))
        private static readonly HashSet<string> AllowedImageExtensions = new(StringComparer.OrdinalIgnoreCase)
        { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };

        private static readonly HashSet<string> AllowedVideoExtensions = new(StringComparer.OrdinalIgnoreCase)
        { ".mp4", ".avi", ".mov", ".wmv", ".flv", ".mkv" };

        private static readonly HashSet<string> AllowedDocumentExtensions = new(StringComparer.OrdinalIgnoreCase)
        { ".pdf", ".docx" };

        private static readonly HashSet<string> AllowedExtensions = new(
            AllowedImageExtensions.Concat(AllowedVideoExtensions).Concat(AllowedDocumentExtensions),
            StringComparer.OrdinalIgnoreCase
        );

        private readonly string _uploadsPath;
        
        // Ngưỡng 2MB (tính bằng bytes)
        private const long MaxImageSizeBeforeCompression = 2 * 1024 * 1024; // 2MB

        public Uploadfile(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            // Đảm bảo lưu file vào wwwroot/Uploads (public folder)
            _uploadsPath = Path.Combine(_hostingEnvironment.WebRootPath, "Uploads");

            // Đảm bảo thư mục tồn tại
            Directory.CreateDirectory(_uploadsPath);
        }
        

        public async Task<(int Status, string Message)> SaveMedia(
            IFormFile file,
            bool compressImages = true,
            bool compressVideos = true,
            int imageQuality = 80, 
            int videoCrf = 28)
        {
            try
            {
                var ext = Path.GetExtension(file.FileName);
                if (string.IsNullOrEmpty(ext) || !AllowedExtensions.Contains(ext))
                {
                    return (0, $"Only the following formats are allowed: {string.Join(", ", AllowedExtensions)}");
                }

                var newFileName = $"{Guid.NewGuid():N}{ext}";
                var filePath = Path.Combine(_uploadsPath, newFileName);

                if (AllowedImageExtensions.Contains(ext) && compressImages)
                {
                    // *** LOGIC MỚI: Chỉ nén nếu file LỚN HƠN 2MB ***
                    if (file.Length > MaxImageSizeBeforeCompression)
                    {
                        // File lớn, tiến hành nén
                        await using var output = new FileStream(filePath, FileMode.Create);
                        await CompressImageAsync(file.OpenReadStream(), output, imageQuality, ext);
                    }
                    else
                    {
                        // File nhỏ (<= 2MB), chỉ copy, không nén
                        await using var output = new FileStream(filePath, FileMode.Create);
                        await file.CopyToAsync(output);
                    }
                }
                else if (AllowedVideoExtensions.Contains(ext) && compressVideos)
                {
                    // (Bạn có thể áp dụng logic tương tự cho video nếu muốn)
                    await CompressVideoAsync(file, filePath, videoCrf);
                }
                else
                {
                    // Lưu trực tiếp (cho .pdf, .docx hoặc khi không nén)
                    await using var output = new FileStream(filePath, FileMode.Create);
                    await file.CopyToAsync(output);
                }

                return (1, newFileName);
            }
            catch (Exception ex)
            {
                // Ghi log chi tiết hơn
                // Log.Error(ex, "Error saving media file.");
                return (0, $"Error: {ex.Message}");
            }
        }

        private static async Task CompressImageAsync(Stream inputStream, Stream outputStream, int quality, string originalExtension)
        {
            using var image = await Image.LoadAsync(inputStream);

            // 1. Resize
            image.Mutate(x => x.Resize(new ResizeOptions
            {
                Mode = ResizeMode.Max,
                Size = new Size(1024) // Chiều lớn nhất 1024px
            }));

            // 2. Xóa metadata
            image.Metadata.ExifProfile = null;

            // 3. Lưu theo định dạng gốc
            if (originalExtension.Equals(".png", StringComparison.OrdinalIgnoreCase))
            {
                await image.SaveAsPngAsync(outputStream, new PngEncoder
                {
                    CompressionLevel = PngCompressionLevel.BestCompression
                });
            }
            else if (originalExtension.Equals(".gif", StringComparison.OrdinalIgnoreCase))
            {
                await image.SaveAsGifAsync(outputStream, new GifEncoder());
            }
            else
            {
                // Mặc định là JPEG
                await image.SaveAsJpegAsync(outputStream, new JpegEncoder
                {
                    Quality = quality,
             
                });
            }
        }

        private static async Task CompressVideoAsync(IFormFile videoFile, string outputPath, int crf)
        {
            // CẢNH BÁO: Yêu cầu cài đặt FFmpeg trên máy chủ và cấu hình PATH

            // 1. Lưu file tạm
            var tempFileName = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}{Path.GetExtension(videoFile.FileName)}");
            await using (var tempStream = new FileStream(tempFileName, FileMode.Create))
            {
                await videoFile.CopyToAsync(tempStream);
            }

            try
            {
                // 2. Chạy FFmpeg
                var arguments = $"-i \"{tempFileName}\" -vcodec libx264 -crf {crf} \"{outputPath}\"";
                var processInfo = new ProcessStartInfo
                {
                    FileName = "ffmpeg", 
                    Arguments = arguments,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                };

                using var process = new Process { StartInfo = processInfo };
                process.Start();
                string error = await process.StandardError.ReadToEndAsync();
                await process.WaitForExitAsync();

                if (process.ExitCode != 0)
                {
                    // Ném lỗi nếu FFmpeg thất bại
                    throw new Exception($"FFmpeg failed with exit code {process.ExitCode}: {error}");
                }
            }
            finally
            {
                // 3. Luôn xóa file tạm dù thành công hay thất bại
                if (File.Exists(tempFileName))
                {
                    File.Delete(tempFileName);
                }
            }
           
            if (!File.Exists(outputPath))
            {
                throw new Exception("Video compression failed. Output file not created.");
            }
        }

        public bool DeleteMedia(string fileName)
        {
            try
            {
                // Đảm bảo tên file không chứa các ký tự điều hướng (path traversal)
                if (string.IsNullOrWhiteSpace(fileName) || fileName.Contains(".."))
                {
                    return false;
                }

                var filePath = Path.Combine(_uploadsPath, fileName);
                
                if (!File.Exists(filePath)) return false;

                File.Delete(filePath);
                return true;
            }
            catch (Exception)
            {
                // Nên log lỗi
                return false;
            }
        }
    }
}