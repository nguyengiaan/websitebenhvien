using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using websitebenhvien.Models.Enitity;
using websitebenhvien.Service.Interface;

namespace WebsiteBenhVien.ViewComponents
{
    public class GetslideViewComponent : ViewComponent
    {
        private readonly IPageMain _page;
        private readonly IMemoryCache _cache;

        public GetslideViewComponent(IPageMain page, IMemoryCache cache)
        {
            _page = page;
            _cache= cache;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
             try
            {
            if (!_cache.TryGetValue("slides", out List<Slidepage> slides))
            {
                slides = await _page.GetSlide();
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(30));
                _cache.Set("slides", slides, cacheEntryOptions);
            }
            return View(slides);
            }
            catch (Exception ex)
            {
              return View("Error", ex.Message);
            }
        }
    }
}