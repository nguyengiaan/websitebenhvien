using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using websitebenhvien.Models.EnitityVM;
using websitebenhvien.Service.Interface;

namespace websitebenhvien.Controllers
{
    public class PartialViewController : Controller
    {
        private readonly IPageMain _pagemain;
        private readonly IMemoryCache _memoryCache;
        private const string HeaderCacheKey = "HeaderCacheKey";

        public PartialViewController(IPageMain pageMain, IMemoryCache memoryCache)
        {
            _pagemain = pageMain;
            _memoryCache = memoryCache;
        }
      
    }
}
