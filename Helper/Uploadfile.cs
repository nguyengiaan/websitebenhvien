﻿using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using FFMpegCore;
using FFMpegCore.Enums;
using System.Linq;
using System.Collections.Generic;

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

        public Uploadfile(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            _uploadsPath = Path.Combine(_hostingEnvironment.ContentRootPath, "Uploads");

            // Đảm bảo thư mục tồn tại
            Directory.CreateDirectory(_uploadsPath);

            // Cấu hình FFmpeg
            GlobalFFOptions.Configure(options =>
                options.BinaryFolder = "/usr/bin" // Thay đổi tùy hệ thống
            );
        }

        public async Task<(int Status, string Message)> SaveMedia(
            IFormFile file,
            bool compressImages = true,
            bool compressVideos = true,
            int imageQuality = 75,
            int videoCrf = 28)
        {
            try
            {
                var ext = Path.GetExtension(file.FileName);
                if (!AllowedExtensions.Contains(ext))
                {
                    return (0, $"Only the following formats are allowed: {string.Join(", ", AllowedExtensions)}");
                }

                var newFileName = $"{Guid.NewGuid():N}{ext}";
                var filePath = Path.Combine(_uploadsPath, newFileName);

                if (AllowedImageExtensions.Contains(ext) && compressImages)
                {
                    await using var output = new FileStream(filePath, FileMode.Create);
                    await CompressImageAsync(file.OpenReadStream(), output, imageQuality);
                }
                else if (AllowedVideoExtensions.Contains(ext) && compressVideos)
                {
                    await CompressVideoAsync(file, filePath, videoCrf);
                }
                else
                {
                    await using var output = new FileStream(filePath, FileMode.Create);
                    await file.CopyToAsync(output);
                }

                return (1, newFileName);
            }
            catch (Exception ex)
            {
                return (0, $"Error: {ex.Message}");
            }
        }

        private static async Task CompressImageAsync(Stream inputStream, Stream outputStream, int quality)
        {
            using var image = await Image.LoadAsync(inputStream);

            image.Mutate(x => x.Resize(new ResizeOptions
            {
                Mode = ResizeMode.Max,
                Size = new Size(1024) // Chiều dài tối đa
            }));

            await image.SaveAsync(outputStream, new JpegEncoder
            {
                Quality = quality
            });
        }

        private static async Task CompressVideoAsync(IFormFile videoFile, string outputPath, int crf)
        {
            var tempInputPath = Path.GetTempFileName();

            try
            {
                await using (var tempStream = File.Create(tempInputPath))
                {
                    await videoFile.CopyToAsync(tempStream);
                }

                await FFMpegArguments
                    .FromFileInput(tempInputPath)
                    .OutputToFile(outputPath, true, options => options
                        .WithVideoCodec(VideoCodec.LibX264)
                        .WithConstantRateFactor(crf)
                        .WithAudioCodec(AudioCodec.Aac)
                        .WithAudioBitrate(128)
                        .WithFastStart()
                        .WithCustomArgument("-movflags +faststart"))
                    .ProcessAsynchronously();
            }
            finally
            {
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
                var filePath = Path.Combine(_uploadsPath, fileName);
                if (!File.Exists(filePath)) return false;

                File.Delete(filePath);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
