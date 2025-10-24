using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using websitebenhvien.Models.EnitityVM;
using websitebenhvien.Service.Interface;

namespace websitebenhvien.Controllers
{
    public class PriceController : Controller
    {
        private readonly IPricelist _pricelist;
        private readonly IMemoryCache _cache;

        public PriceController(IPricelist pricelist, IMemoryCache cache)
        {
            _pricelist= pricelist;
            _cache = cache;

        }
        [Route("/bang-gia-dich-vu")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var cacheKey = "pricelist_cache";
                if (!_cache.TryGetValue(cacheKey, out List<PricelistVM> pricelistData))
                {
                    pricelistData = await _pricelist.ListPricelistVM();

                    var cacheOptions = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
                        Priority = CacheItemPriority.Normal
                    };
                    _cache.Set(cacheKey, pricelistData, cacheOptions);
                }
                return View(pricelistData);


            }
            catch (Exception ex)
            {
                return View("Error", ex.Message);

            }
        }
    }
}
