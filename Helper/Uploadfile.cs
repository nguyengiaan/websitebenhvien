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
using SixLabors.ImageSharp.Formats.Png;

namespace websitebenhvien.Helper
{
    public class Uploadfile
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private static readonly string[] AllowedImageExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
        private static readonly string[] AllowedVideoExtensions = { ".mp4", ".avi", ".mov", ".wmv", ".flv", ".mkv" };
        private static readonly string[] AllowedDocumentExtensions = { ".pdf", ".docx" };
        private static readonly string[] AllowedExtensions;

        static Uploadfile()
        {
            AllowedExtensions = AllowedImageExtensions
                .Concat(AllowedVideoExtensions)
                .Concat(AllowedDocumentExtensions)
                .ToArray();
        }

        public Uploadfile(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;

            // Cấu hình FFmpeg (đảm bảo FFmpeg đã được cài đặt trên hệ thống)
            GlobalFFOptions.Configure(options =>
                options.BinaryFolder = "/usr/bin"); // Thay đổi tùy theo hệ thống
        }

        public async Task<Tuple<int, string>> SaveMedia(IFormFile file,
                                                       bool compressImages = true,
                                                       bool compressVideos = true,
                                                       int imageQuality = 75,
                                                       int videoCrf = 28)
        {
            try
            {
                var ext = Path.GetExtension(file.FileName).ToLower();

                if (!AllowedExtensions.Contains(ext))
                {
                    string msg = $"Only the following formats are allowed: {string.Join(", ", AllowedExtensions)}";
                    return Tuple.Create(0, msg);
                }

                var uploadsPath = Path.Combine(_hostingEnvironment.ContentRootPath, "Uploads");
                Directory.CreateDirectory(uploadsPath); // An toàn, không ném exception nếu thư mục tồn tại

                var newFileName = $"{Guid.NewGuid():N}{ext}";
                var filePath = Path.Combine(uploadsPath, newFileName);

                // Xử lý nén ảnh
                if (compressImages && AllowedImageExtensions.Contains(ext))
                {
                    await using var outputStream = new FileStream(filePath, FileMode.Create);
                    await CompressImageAsync(file.OpenReadStream(), outputStream, imageQuality);
                }
                // Xử lý nén video
                else if (compressVideos && AllowedVideoExtensions.Contains(ext))
                {
                    await CompressVideoAsync(file, filePath, videoCrf);
                }
                // Xử lý file bình thường
                else
                {
                    await using var stream = new FileStream(filePath, FileMode.Create);
                    await file.CopyToAsync(stream);
                }

                return Tuple.Create(1, newFileName);
            }
            catch (Exception ex)
            {
                return Tuple.Create(0, $"Error: {ex.Message}");
            }
        }

        private static async Task CompressImageAsync(Stream inputStream, Stream outputStream, int quality)
        {
            using var image = await Image.LoadAsync(inputStream);

            // Tính toán kích thước mới giữ nguyên tỷ lệ
            var resizeOptions = new ResizeOptions
            {
                Mode = ResizeMode.Max,
                Size = new Size(1024) // Giới hạn chiều dài nhất là 1920px
            };

            image.Mutate(x => x.Resize(resizeOptions));

            var encoder = new JpegEncoder
            {
                Quality = quality,
  
            };

            await image.SaveAsync(outputStream, encoder);
        }

        private static async Task CompressVideoAsync(IFormFile videoFile, string outputPath, int crf)
        {
            var tempInputPath = Path.GetTempFileName();

            try
            {
                // Lưu file tạm
                await using (var tempStream = File.Create(tempInputPath))
                {
                    await videoFile.CopyToAsync(tempStream);
                }

                // Cấu hình nén video tối ưu
                await FFMpegArguments
                    .FromFileInput(tempInputPath)
                    .OutputToFile(outputPath, true, options => options
                        .WithVideoCodec(VideoCodec.LibX264)
                        .WithConstantRateFactor(crf) // CRF: 18-28 (23 là mặc định)
                
                        .WithAudioCodec(AudioCodec.Aac)
                        .WithAudioBitrate(128) // 128kbps cho âm thanh
                        .WithFastStart() // Tối ưu cho phát trực tuyến
                        .WithCustomArgument("-movflags +faststart")) // Tương thích web
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
                var filePath = Path.Combine(_hostingEnvironment.ContentRootPath, "Uploads", fileName);

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