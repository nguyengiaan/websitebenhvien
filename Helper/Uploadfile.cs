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

        }
        public Tuple<int, string> SaveMedia(IFormFile imageFile)
        {
            try
            {
                var contentPath = this._hostingEnvironment.ContentRootPath;
                // path = "c://projects/productminiapi/uploads" ,not exactly something like that
                var path = Path.Combine(contentPath, "Uploads");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                // Check the allowed extenstions
                var ext = Path.GetExtension(imageFile.FileName);
                var allowedExtensions = new string[]
                {
                ".jpg", ".jpeg", ".png", ".gif", ".bmp",
                 ".mp4", ".avi", ".mov", ".wmv", ".flv", ".mkv",
             ".pdf", ".docx"
                };

                if (!allowedExtensions.Contains(ext))
                {
                    string msg = string.Format("Only {0} extensions are allowed", string.Join(",", allowedExtensions));
                    return new Tuple<int, string>(0, msg);
                }
                string uniqueString = Guid.NewGuid().ToString();
                var fileName = Path.GetFileName(imageFile.FileName);
                // we are trying to create a unique filename here
                var newFileName = Guid.NewGuid().ToString().Substring(0, 4) + "__" + fileName;
                var fileWithPath = Path.Combine(path, newFileName);
                var stream = new FileStream(fileWithPath, FileMode.Create);
                imageFile.CopyTo(stream);
                stream.Close();
                return new Tuple<int, string>(1, newFileName);
            }
            catch (Exception ex)
            {
                return new Tuple<int, string>(0, "Error has occured");
            }
        }
        public bool DeleteMedia(string fileName)
        {
            try
            {
                var contentPath = this._hostingEnvironment.ContentRootPath;
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
}
