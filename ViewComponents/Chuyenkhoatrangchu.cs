using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using websitebenhvien.Service.Interface;

namespace Websitebenhvien.ViewComponents
{
   public class Chuyenkhoatrangchu : ViewComponent
{
    private readonly ISpecialty _specialty;
    private readonly IMemoryCache _cache;

    public Chuyenkhoatrangchu(ISpecialty specialty, IMemoryCache cache)
    {
        _specialty = specialty;
        _cache = cache;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        try
        {
            // Define a cache key
            var cacheKey = "SpecialtiesCache";

            // Try to get data from cache
            if (!_cache.TryGetValue(cacheKey, out var specialties))
            {
                // If not in cache, fetch from the database
                specialties = await _specialty.GetAllSpecialty();

                // Set cache options
                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15), // Cache for 30 minutes
                  // Reset expiration if accessed
                };

                // Save data in cache
                _cache.Set(cacheKey, specialties, cacheOptions);
            }

            return View(specialties);
        }
        catch (Exception ex)
        {
            // Handle the exception as needed
            return View("Error", ex.Message);
        }
    }
}
}