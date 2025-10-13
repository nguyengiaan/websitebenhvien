using Microsoft.AspNetCore.Mvc;
using websitebenhvien.Data;
using websitebenhvien.Models.EnitityVM;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace websitebenhvien.ViewComponents
{
    public class DanhmuchoatdongViewComponent : ViewComponent
    {
        private readonly MyDbcontext _context;
        private readonly IMemoryCache _cache;

        public DanhmuchoatdongViewComponent(MyDbcontext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var cacheKey = "activity_categories";
            
            // Kiểm tra cache trước
            if (!_cache.TryGetValue(cacheKey, out List<CategoryActivityVM> categories))
            {
                try
                {
                    // Lấy danh mục hoạt động từ database - sử dụng Activitycategories
                    categories = await _context.Activitycategories
                        .OrderBy(c => c.Id_activitycategory)
                        .Select(c => new CategoryActivityVM
                        {
                         
                            Title = c.Title ?? "",
                            Link_alias = c.link_alias ?? "",
                           
                        })
                        .ToListAsync();

                    // Cache kết quả trong 30 phút
                    var cacheOptions = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30),
                        Priority = CacheItemPriority.Normal
                    };
                    _cache.Set(cacheKey, categories, cacheOptions);
                }
                catch (Exception)
                {
                    // Nếu có lỗi, trả về danh sách rỗng
                    categories = new List<CategoryActivityVM>();
                }
            }

            return View(categories);
        }
    }
}