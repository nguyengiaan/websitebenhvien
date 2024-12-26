using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using websitebenhvien.Data;
using websitebenhvien.Helper;
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
        private readonly Uploadfile _uploadfile;

        public AllinoneReponser(Uploadfile uploadfile,MyDbcontext context, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _hostingEnvironment = hostingEnvironment;
            _uploadfile= uploadfile;

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


                        var result = _uploadfile.SaveMedia(news.formFile);
                        if (result.Item1 == 1)
                        {
                            

                            if (data.Result.Url != null)
                            {
                                _uploadfile.DeleteMedia(data.Result.Url);
                            }
                            data.Result.Url = result.Item2;
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
                        var result = _uploadfile.SaveMedia(news.formFile);
                        if (result.Item1 == 1)
                        {
                            data.Url = result.Item2;
                        }
                        
                    }
                   
                    data.Title = news.Title;
                    data.Alias_url = news.Alias_url;
                    data.Description = news.Description;
                    data.Id_Categorynews = news.Id_Categorynews;
                    data.Createat = DateTime.Now;
                    data.Status = true;

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
     
        public async Task<List<NewsVM>> Listnews()
        {
            try
            {
                // Sử dụng AsNoTracking() để không track entities và tăng hiệu suất
                var data = await _context.News
                    .AsNoTracking()
                    .Select(x => new NewsVM
                    {
                        Id_News = x.Id_News,
                        Title = x.Title,
                        Url = x.Url,
                        Alias_url = x.Alias_url,
                        Id_Categorynews = x.Id_Categorynews,
                        Createat = x.Createat,
                        Status = x.Status
                    })
                    .OrderByDescending(x => x.Createat)
                    .ToListAsync();

                return data;
            }
            catch (Exception)
            {
                return null;
            }
        }

        
        public async Task<bool> DeleteNews(int id)
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

         public async Task<NewsVM> GetNewsById(int id)
     {
    try
    {
        // Truy vấn dữ liệu với AsNoTracking để cải thiện hiệu năng
        var data = await _context.News
            .AsNoTracking()
            .Where(x => x.Id_News == id)
            .Select(x => new NewsVM
            {
                Id_News = x.Id_News,
                Title = x.Title,
                Description = x.Description,
                Url = x.Url,
                Alias_url = x.Alias_url,
                Id_Categorynews = x.Id_Categorynews,
                Createat = x.Createat
            })
            .FirstOrDefaultAsync();

        return data;
    }
    catch (Exception ex)
    {
        // Log lỗi (có thể sử dụng thư viện logging như Serilog hoặc NLog)
        Console.WriteLine($"Error in GetNewsById: {ex.Message}");
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
                        var result = _uploadfile.SaveMedia(category.formFile);
                        if (result.Item1 == 1)
                        {
                            if (data.url != null)
                            {
                                _uploadfile.DeleteMedia(data.url);
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
                        var result = _uploadfile.SaveMedia(category.formFile);
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

        public async Task<bool> AddProduct(ProductVM product)
        {
            try
            {
                 if(product.Id_product != null)
                {
                    var data =await _context.Products.FindAsync(product.Id_product);
                    if (product.ImageThumnail != null)
                    {
                        var result = _uploadfile.SaveMedia(product.ImageThumnail);
                        if (result.Item1 == 1)
                        {
                            if (data.url != null)
                            {
                                _uploadfile.DeleteMedia(data.url);
                            }
                            data.url = result.Item2;
                        }
                    }
                    if (product.Images != null)
                    {
                        // Xóa ảnh cũ
                        var existingImages = await _context.Proimages
                            .Where(x => x.Id_product == product.Id_product)
                            .ToListAsync();
                            
                        foreach (var img in existingImages)
                        {
                            _uploadfile.DeleteMedia(img.url);
                        }
                        _context.Proimages.RemoveRange(existingImages);

                        // Thêm ảnh mới
                        var newImages = product.Images
                            .Select(img => _uploadfile.SaveMedia(img))
                            .Where(result => result.Item1 == 1)
                            .Select(result => new Proimages
                            {
                                Id_proimages = Guid.NewGuid().ToString(),
                                Id_product = product.Id_product,
                                url = result.Item2
                            });

                        await _context.Proimages.AddRangeAsync(newImages);
                        await _context.SaveChangesAsync();
                    }

                    data.Product_Id = product.Product_Id;
                    data.Title = product.Title;
                    data.Alias_url = product.Alias_url;
                    data.Description = product.Description;
                    data.Id_Categoryproduct = product.Id_Categoryproduct;
                    await _context.SaveChangesAsync();
                    return true;

                }

                else
                {
                    var data = new Product()
                    {
                        Id_product = Guid.NewGuid().ToString(),
                        Product_Id = product.Product_Id,
                        Title = product.Title,
                        Alias_url = product.Alias_url,
                        Description = product.Description,
                        Createat = DateTime.Now,
                        Id_Categoryproduct = product.Id_Categoryproduct
                    };
                    if (product.ImageThumnail != null)
                    {
                        var result = _uploadfile.SaveMedia(product.ImageThumnail);
                        if (result.Item1 == 1)
                        {
                            data.url = result.Item2;
                        }
                    }
                    if (product.Images != null)
                    {
                        foreach (var item in product.Images)
                        {
                            var result = _uploadfile.SaveMedia(item);
                            if (result.Item1 == 1)
                            {
                                var data1 = new Proimages()
                                {
                                    Id_proimages = Guid.NewGuid().ToString(),
                                    Id_product = data.Id_product,
                                    url = result.Item2
                                };
                                _context.Proimages.Add(data1);
                            }
                        }
                    }
                    _context.Products.Add(data);
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

        public async Task<List<ProductVM>> ListProduct()
        {
            try
            {
                var data = await _context.Products
                    .Select(p => new ProductVM
                    {
                        Id_product = p.Id_product,
                        Title = p.Title,
                        Description = p.Description,
                        Alias_url = p.Alias_url,
                        Id_Categoryproduct = p.Id_Categoryproduct,
                        Createat = p.Createat,
                        url = p.url,
                        ImagesDelete = p.Proimages.Select(x => x.url).ToList(),
                        status = p.status,
                        Product_Id = p.Product_Id,
                    })
                    .OrderByDescending(x => x.Createat)
                    .ToListAsync();
              return data;
            }catch(Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> DeleteProduct(string id)
        {
            try
            {
                var data = await _context.Products.FindAsync(id);
                _context.Products.Remove(data);
                await _context.SaveChangesAsync();
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<ProductVM> GetProductById(string id)
        {
          try
          {
                   var data = await _context.Products
                    .Select(p => new ProductVM
                    {
                        Id_product = p.Id_product,
                        Title = p.Title,
                        Description = p.Description,
                        Alias_url = p.Alias_url,
                        Id_Categoryproduct = p.Id_Categoryproduct,
                        Createat = p.Createat,
                        url = p.url,
                        ImagesDelete = p.Proimages.Select(x => x.url).ToList(),
                        status = p.status,
                        Product_Id = p.Product_Id
                    })
                    .OrderByDescending(x => x.Createat)
                    .FirstOrDefaultAsync(x => x.Id_product == id);
              return data;

          }
          catch(Exception ex)
          {
            return null;
          }
        }

        public async Task<bool> UpdatestatusPro(string id)
        {
            try
            {
                var data = await _context.Products.FindAsync(id);
                data.status = !data.status;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<CountdashboardVM> Countdashboard()
        {
            try
            {
                var data = new CountdashboardVM();
                data.CountUser = await _context.Users.CountAsync() ;
                data.CountProduct = await _context.Products.CountAsync() ;
                data.CountCategoryProduct = await _context.Categoryproducts.CountAsync()  ;
                data.CountNews = await _context.News.CountAsync()  ;
                data.CountCategoryNews = await _context.Categorynews.CountAsync()  ;
                return data;
            }
            catch(Exception ex)
            {
                return null;
            }
        }
        // update status news
        public async Task<bool> UpdateStatusNews(int id)
        {
         try
    {   
        // Thực hiện cập nhật trực tiếp và kiểm tra số bản ghi bị ảnh hưởng
        int affectedRows = await _context.News
            .Where(x => x.Id_News == id)
            .ExecuteUpdateAsync(s => s
                .SetProperty(b => b.Status, b => !b.Status));

                // Nếu không có bản ghi nào bị cập nhật, trả về false
                return affectedRows > 0;
            }
            catch (Exception)
    {
                // Log lỗi nếu cần
                return false;
            }
        }


        public async Task<List<NewsVM>> ListService()
        {
            try
            {
                var data = await _context.News
                    .Join(_context.Categorynews,
                        n => n.Id_Categorynews,
                        c => c.Id_Categorynews,
                        (n, c) => new { n, c })
                    .Where(x => x.c.Title == "Dịch vụ")
                    .Select(x => new NewsVM
                    {
                        Id_News = x.n.Id_News,
                        Title = x.n.Title,
                        Url = x.n.Url,
                        Alias_url = x.n.Alias_url,
                        Createat = x.n.Createat,
                        Status = x.n.Status,
                    })
                    .ToListAsync();
                return data;
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public async Task<List<NewsVM>> ListNews()
        {
            try
            {
                var data = await _context.News
                    .Join(_context.Categorynews,
                        n => n.Id_Categorynews,
                        c => c.Id_Categorynews,
                        (n, c) => new { n, c })
                    .Where(x => x.c.Title == "Tin tức và sự kiện")
                    .Select(x => new NewsVM
                    {
                        Id_News = x.n.Id_News,
                        Title = x.n.Title,
                        Url = x.n.Url,
                        Alias_url = x.n.Alias_url,
                        Createat = x.n.Createat,
                        Status = x.n.Status,
                    })
                    .ToListAsync();
                return data;
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public async Task<List<LogocustomerVM>> ListShareCustomer()
        {
            try
            {   
                var data = await _context.Logocustomer.Select(x => new LogocustomerVM()
                {
                    CustomerName = x.Logo,
                }).ToListAsync();
                return data;

            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public async Task<(List<NewsVM> ds, int totalpages)> ListCategorypost(int page, int pagesize,string Catogery)
        {
            try
            {
                var totalItems = await _context.News
     .Join(_context.Categorynews,
           n => n.Id_Categorynews,
           c => c.Id_Categorynews,
           (n, c) => new { n, c })
     .Where(x => x.c.Alias_url == Catogery) // Sửa tên biến "Catogery" thành "Category"
     .CountAsync();

                var totalPages = (int)Math.Ceiling(totalItems / (double)pagesize);

                var data = await _context.News
                    .Join(_context.Categorynews,
                          n => n.Id_Categorynews,
                          c => c.Id_Categorynews,
                          (n, c) => new { n, c })
                    .Where(x => x.c.Alias_url == Catogery)
                    .OrderByDescending(x => x.n.Createat)
                    .Skip((page - 1) * pagesize)
                    .Take(pagesize)
                    .Select(x => new NewsVM
                    {
                        Id_News = x.n.Id_News,
                        Title = x.n.Title,
                        Url = x.n.Url,
                        Alias_url = x.n.Alias_url,
                        Createat = x.n.Createat,
                        Status = x.n.Status,
                    })
                    .ToListAsync();

                return (data, totalPages);

            }
            catch (Exception ex)
            {
                return (null,0);
            }
        }
        // danh mục catogery post

    }
}
