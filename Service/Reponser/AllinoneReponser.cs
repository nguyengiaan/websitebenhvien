using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using websitebenhvien.Data;
using websitebenhvien.Models.Enitity;
using websitebenhvien.Models.EnitityVM;
using websitebenhvien.Service.Interface;

namespace websitebenhvien.Service.Reponser
{
    public class AllinoneReponser : IAllinone
    {
        private readonly MyDbcontext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public AllinoneReponser(MyDbcontext context, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _hostingEnvironment = hostingEnvironment;

        }
        public async Task<bool> AddCatogery(CategorynewsVM category)
        {
            try
            {
                if (category.Id_Categorynews != null)
                {
                    var data1 = _context.Categorynews.FindAsync(category.Id_Categorynews);
                    data1.Result.Title = category.Title;
                    data1.Result.Alias_url = category.Alias_url;
                    data1.Result.Description = category.Description;
                    await _context.SaveChangesAsync();
                    return true;
                }
                else
                {
                    var data = new Categorynews()
                    {
                        Id_Categorynews = Guid.NewGuid().ToString(),
                        Title = category.Title,
                        Alias_url = category.Alias_url,
                        Description = category.Description,
                        Createat = DateTime.Now

                    };
                    _context.Categorynews.Add(data);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<List<CategorynewsVM>> ListCatogery()
        {
            try
            {
                var data = await _context.Categorynews.Select(x => new CategorynewsVM()
                {
                    Id_Categorynews = x.Id_Categorynews,
                    Title = x.Title,
                    Alias_url = x.Alias_url,
                    Description = x.Description,
                    Createat = x.Createat
                }).OrderByDescending(x => x.Createat).ToListAsync();
                return data;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<bool> DeleteCatogery(string id)
        {
            try
            {
                var data = _context.Categorynews.FindAsync(id);
                _context.Categorynews.Remove(data.Result);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        //Thêm tin tức
        public async Task<bool> AddNews(NewsVM news)
        {
            try
            {
                if (news.Id_News != null)
                {
                    var data = _context.News.FindAsync(news.Id_News);
                    if (news.formFile != null)
                    {


                        var result = SaveMedia(news.formFile);
                        if (result.Item1 == 1)
                        {
                            news.Url = result.Item2;

                            if (data.Result.Url != null)
                            {
                                DeleteMedia(data.Result.Url);
                            }
                        }
                    }

                    data.Result.Title = news.Title;
                    data.Result.Alias_url = news.Alias_url;
                    data.Result.Description = news.Description;
                    data.Result.Id_Categorynews = news.Id_Categorynews;
                    await _context.SaveChangesAsync();
                    return true;
                }
                else
                {
                    var data = new News();
                    if (news.formFile != null)
                    {
                        var result = SaveMedia(news.formFile);
                        if (result.Item1 == 1)
                        {
                            data.Url = result.Item2;
                        }
                    }
                    data.Id_News = Guid.NewGuid().ToString();
                    data.Title = news.Title;
                    data.Alias_url = news.Alias_url;
                    data.Description = news.Description;
                    data.Id_Categorynews = news.Id_Categorynews;
                    data.Createat = DateTime.Now;

                    _context.News.Add(data);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
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
                var allowedExtensions = new string[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".mp4", ".avi", ".mov", ".wmv", ".flv", ".mkv" };
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

        public async Task<List<NewsVM>> Listnews()
        {
            try
            {
                var data = await _context.News.Select(x => new NewsVM()
                {
                    Id_News = x.Id_News,
                    Title = x.Title,
                    Description = x.Description,
                    Url = x.Url,
                    Alias_url = x.Alias_url,
                    Id_Categorynews = x.Id_Categorynews,
                    Createat = x.Createat
                }).OrderByDescending(x => x.Createat).ToListAsync();
                return data;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> DeleteNews(string id)
        {
            try
            {
                var data = await _context.News.FindAsync(id);
                _context.News.Remove(data);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<NewsVM> GetNewsById(string id)
        {
            try
            {
                var data = await _context.News.Select(x => new NewsVM()
                {
                    Id_News = x.Id_News,
                    Title = x.Title,
                    Description = x.Description,
                    Url = x.Url,
                    Alias_url = x.Alias_url,
                    Id_Categorynews = x.Id_Categorynews,
                    Createat = x.Createat
                }).FirstOrDefaultAsync(x => x.Id_News == id);
                return data;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> AddCatogeryProduct(CategoryproductVM category)
        {
            try
            {
                if (category.Id_Categoryproduct != null)
                {

                    var data = await _context.Categoryproducts.FindAsync(category.Id_Categoryproduct);
                    if (category.formFile != null)
                    {
                        var result = SaveMedia(category.formFile);
                        if (result.Item1 == 1)
                        {
                            if (data.url != null)
                            {
                                DeleteMedia(data.url);
                            }
                            data.url = result.Item2;
                           
                        }
                    }

                    data.Title = category.Title;
                    data.Alias_url = category.Alias_url;
                    data.Description = category.Description;
                    await _context.SaveChangesAsync();
                    return true;


                }
                else
                {
                    var data = new Categoryproduct()
                    {
                        Id_Categoryproduct = Guid.NewGuid().ToString(),
                        Title = category.Title,
                        Alias_url = category.Alias_url,
                        Description = category.Description,
                        Createat = DateTime.Now
                    };
                    if (category.formFile != null)
                    {
                        var result = SaveMedia(category.formFile);
                        if (result.Item1 == 1)
                        {
                            data.url = result.Item2;
                        }
                    }
                    _context.Categoryproducts.Add(data);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public Task<List<CategoryproductVM>> ListCatogeryProduct()
        {
            try
            {
                var data = _context.Categoryproducts.Select(x => new CategoryproductVM()
                {
                    Id_Categoryproduct = x.Id_Categoryproduct,
                    Title = x.Title,
                    Alias_url = x.Alias_url,
                    Description = x.Description,
                    Createat = x.Createat,
                    url = x.url
                }).OrderByDescending(x => x.Createat).ToListAsync();
                return data;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> DeleteCatogeryProduct(string id)
        {
            try
            {
                var data = await _context.Categoryproducts.FindAsync(id);
                _context.Categoryproducts.Remove(data);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;

            }
        }

        public Task<CategoryproductVM> GetCatogeryProductById(string id)
        {
            try
            {
                var data = _context.Categoryproducts.Select(x => new CategoryproductVM()
                {
                    Id_Categoryproduct = x.Id_Categoryproduct,
                    Title = x.Title,
                    Alias_url = x.Alias_url,
                    Description = x.Description,
                    Createat = x.Createat,
                    url = x.url
                }).FirstOrDefaultAsync(x => x.Id_Categoryproduct == id);
                return data;

            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
