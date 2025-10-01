using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using websitebenhvien.Service.Interface;

namespace websitebenhvien.Controllers
{
    public class DoctorController : Controller
    {
        private readonly ISpecialty _specialty;
        private readonly IMemoryCache _cache;
        private const string DOCTOR_SPECIALTY_CACHE_KEY = "DoctorSpecialty";

        public DoctorController(ISpecialty specialty, IMemoryCache cache)
        {
            _specialty = specialty;
            _cache = cache;
        }
        
        [HttpGet("/bac-si-chuyen-khoa")]
        public async Task<IActionResult> Doctor()
        {
            try
            {
                // Kiểm tra cache trước
                if (!_cache.TryGetValue(DOCTOR_SPECIALTY_CACHE_KEY, out var cachedDoctorSpecialty))
                {
                    // Nếu không có trong cache, lấy từ database
                    cachedDoctorSpecialty = await _specialty.GetDoctorSpecialty();
                    
                    // Lưu vào cache với thời gian hết hạn 5 phút
                    var cacheOptions = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
                        SlidingExpiration = TimeSpan.FromMinutes(2), // Reset cache nếu được truy cập trong 2 phút
                        Priority = CacheItemPriority.Normal
                    };
                    
                    _cache.Set(DOCTOR_SPECIALTY_CACHE_KEY, cachedDoctorSpecialty, cacheOptions);
                }
                
                return View(cachedDoctorSpecialty);
            }
            catch (Exception ex)
            {
                // Log the exception (ex) here as needed
                return StatusCode(500, "Đã có lỗi xảy ra khi xử lý yêu cầu của bạn.");
            }
        }
    }
}
