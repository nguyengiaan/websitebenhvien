using System.IO;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace websitebenhvien.Service.Utils
{
    public static class ImageCompressor
    {
        public static async Task<byte[]> CompressImageAsync(Stream inputStream, int quality = 75, int maxWidth = 1024, int maxHeight = 1024)
        {
            using var image = await Image.LoadAsync(inputStream);

            // Resize nếu kích thước lớn hơn maxWidth hoặc maxHeight
            image.Mutate(x => x.Resize(new ResizeOptions
            {
                Mode = ResizeMode.Max,
                Size = new Size(maxWidth, maxHeight)
            }));

            // Nén ảnh với chất lượng được chỉ định
            var encoder = new JpegEncoder
            {
                Quality = quality
            };

            using var outputStream = new MemoryStream();
            await image.SaveAsync(outputStream, encoder);
            return outputStream.ToArray();
        }
    }
}