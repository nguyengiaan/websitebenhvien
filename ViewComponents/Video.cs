using Microsoft.AspNetCore.Mvc;
using websitebenhvien.Service.Interface;
using Microsoft.Extensions.Caching.Memory;

namespace websitebenhvien.ViewComponents
{
    public class VideoViewComponent : ViewComponent
    {
    private readonly IRecruitment _recruitment;
    private readonly IMemoryCache _cache;

        public VideoViewComponent(IRecruitment recruitment, IMemoryCache cache)
        {
            _recruitment = recruitment;
            _cache = cache;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
          return View();
        }
    }
}