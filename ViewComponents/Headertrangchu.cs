using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using websitebenhvien.Service.Interface;
using websitebenhvien.Models.EnitityVM;

namespace websitebenhvien.Views.Shared.Components
{
    public class Headertrangchu : ViewComponent
    {
        private readonly IPageMain _pagemain;
        private readonly IMemoryCache _memoryCache;
        private const string HeaderCacheKey = "HeaderCacheKey";

        public Headertrangchu(IPageMain pageMain, IMemoryCache memoryCache)
        {
            _pagemain = pageMain;
            _memoryCache = memoryCache;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            Header_trangchu header;
            try
            {
                if (!_memoryCache.TryGetValue(HeaderCacheKey, out header))
                {
                    var headerData = await _pagemain.GetTitleheader();
                    var menuData = await _pagemain.ListMenu();

                    if (headerData == null || menuData == null)
                    {
                        return View(new Header_trangchu());
                    }
                    header = new Header_trangchu
                    {
                        headers = headerData,
                        menus = menuData
                    };

                  var cacheEntryOptions = new MemoryCacheEntryOptions()
    .SetAbsoluteExpiration(TimeSpan.FromMinutes(15));


                    _memoryCache.Set(HeaderCacheKey, header, cacheEntryOptions);
                }

                return View(header);
            }
            catch (Exception)
            {
                return View( new Header_trangchu());
            }
        }
    }
}
