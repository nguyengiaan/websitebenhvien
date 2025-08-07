using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace websitebenhvien.Helper
{
    public static class SeoHelper
    {
        /// <summary>
        /// Tạo alias URL từ tiêu đề
        /// </summary>
        /// <param name="title">Tiêu đề bài viết</param>
        /// <returns>Alias URL thân thiện với SEO</returns>
        public static string GenerateAliasUrl(string title)
        {
            if (string.IsNullOrEmpty(title))
                return string.Empty;

            // Chuyển về chữ thường
            string result = title.ToLowerInvariant();

            // Loại bỏ dấu tiếng Việt
            result = RemoveVietnameseTone(result);

            // Thay thế khoảng trắng và ký tự đặc biệt bằng dấu gạch ngang
            result = Regex.Replace(result, @"[^a-z0-9\s-]", "");
            result = Regex.Replace(result, @"\s+", "-").Trim();
            result = Regex.Replace(result, @"-+", "-");

            // Loại bỏ dấu gạch ngang ở đầu và cuối
            result = result.Trim('-');

            return result;
        }

        /// <summary>
        /// Tạo từ khóa SEO từ tiêu đề và mô tả
        /// </summary>
        /// <param name="title">Tiêu đề bài viết</param>
        /// <param name="description">Mô tả bài viết</param>
        /// <returns>Từ khóa SEO</returns>
        public static string GenerateSeoKeywords(string title, string? description = null)
        {
            if (string.IsNullOrEmpty(title))
                return string.Empty;

            List<string> keywords = new List<string>();

            // Tách từ khóa từ tiêu đề
            var titleWords = ExtractKeywords(title);
            keywords.AddRange(titleWords);

            // Tách từ khóa từ mô tả nếu có
            if (!string.IsNullOrEmpty(description))
            {
                var descWords = ExtractKeywords(description);
                keywords.AddRange(descWords.Take(5)); // Chỉ lấy 5 từ đầu từ mô tả
            }

            // Loại bỏ từ trùng lặp và từ stop words
            keywords = keywords.Distinct()
                              .Where(k => !IsStopWord(k) && k.Length > 2)
                              .Take(10) // Chỉ lấy 10 từ khóa
                              .ToList();

            return string.Join(", ", keywords);
        }

        /// <summary>
        /// Tạo Schema Markup JSON-LD cho bài viết
        /// </summary>
        /// <param name="title">Tiêu đề bài viết</param>
        /// <param name="description">Mô tả bài viết</param>
        /// <param name="url">URL bài viết</param>
        /// <param name="thumbnail">Hình ảnh thumbnail</param>
        /// <param name="publishDate">Ngày xuất bản</param>
        /// <param name="baseUrl">Base URL của website</param>
        /// <returns>Schema Markup JSON-LD</returns>
        public static string GenerateSchemaMarkup(string title, string? description, string url, string? thumbnail, DateTime publishDate, string? baseUrl = null)
        {
            // Use default base URL if not provided
            if (string.IsNullOrEmpty(baseUrl))
            {
                baseUrl = "https://your-domain.com";
            }

            // Ensure URL is absolute
            if (!url.StartsWith("http"))
            {
                url = baseUrl + (url.StartsWith("/") ? "" : "/") + url;
            }

            // Set default thumbnail if not provided
            if (string.IsNullOrEmpty(thumbnail))
            {
                thumbnail = baseUrl + "/images/sample.jpg";
            }
            else if (!thumbnail.StartsWith("http"))
            {
                thumbnail = baseUrl + (thumbnail.StartsWith("/") ? "" : "/") + thumbnail;
            }

            var schema = new
            {
                context = "https://schema.org",
                type = "Article",
                headline = title,
                description = description ?? "",
                url = url,
                image = thumbnail,
                datePublished = publishDate.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                dateModified = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                author = new
                {
                    type = "Organization",
                    name = "Bệnh Viện"
                },
                publisher = new
                {
                    type = "Organization",
                    name = "Bệnh Viện",
                    logo = new
                    {
                        type = "ImageObject",
                        url = baseUrl + "/images/logo.png"
                    }
                }
            };

            return System.Text.Json.JsonSerializer.Serialize(schema, new System.Text.Json.JsonSerializerOptions
            {
                PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase,
                WriteIndented = true
            });
        }

        /// <summary>
        /// Loại bỏ dấu tiếng Việt
        /// </summary>
        private static string RemoveVietnameseTone(string text)
        {
            string[] vietnameseChars = new string[]
            {
                "aAeEoOuUiIdDyY",
                "áàạảãâấầậẩẫăắằặẳẵ",
                "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",
                "éèẹẻẽêếềệểễ",
                "ÉÈẸẺẼÊẾỀỆỂỄ",
                "óòọỏõôốồộổỗơớờợởỡ",
                "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",
                "úùụủũưứừựửữ",
                "ÚÙỤỦŨƯỨỪỰỬỮ",
                "íìịỉĩ",
                "ÍÌỊỈĨ",
                "đ",
                "Đ",
                "ýỳỵỷỹ",
                "ÝỲỴỶỸ"
            };

            for (int i = 1; i < vietnameseChars.Length; i++)
            {
                for (int j = 0; j < vietnameseChars[i].Length; j++)
                {
                    text = text.Replace(vietnameseChars[i][j], vietnameseChars[0][i - 1]);
                }
            }

            return text;
        }

        /// <summary>
        /// Trích xuất từ khóa từ văn bản
        /// </summary>
        private static List<string> ExtractKeywords(string text)
        {
            if (string.IsNullOrEmpty(text))
                return new List<string>();

            // Loại bỏ HTML tags nếu có
            text = Regex.Replace(text, "<.*?>", " ");

            // Tách từ
            var words = Regex.Matches(text.ToLowerInvariant(), @"\b\w{3,}\b")
                            .Cast<Match>()
                            .Select(m => m.Value)
                            .ToList();

            return words;
        }

        /// <summary>
        /// Kiểm tra xem có phải stop word không
        /// </summary>
        private static bool IsStopWord(string word)
        {
            string[] stopWords = {
                "là", "của", "và", "có", "được", "một", "trong", "với", "để", "cho",
                "từ", "này", "đó", "các", "những", "như", "về", "theo", "khi", "nếu",
                "the", "a", "an", "and", "or", "but", "in", "on", "at", "to", "for",
                "of", "with", "by", "is", "are", "was", "were", "be", "been", "have",
                "has", "had", "do", "does", "did", "will", "would", "could", "should"
            };

            return stopWords.Contains(word.ToLowerInvariant());
        }
    }
}
