using Microsoft.Extensions.Caching.Memory;

namespace websitebenhvien.Helper
{
    public static class CacheExtensions
    {
        public static void ClearFeedbackCache(this IMemoryCache cache)
        {
            cache.Remove("goccamnhan_feedbacks");
        }
        
        public static void ClearAllFeedbackRelatedCache(this IMemoryCache cache)
        {
            // Clear all feedback related cache keys
            var keysToRemove = new[]
            {
                "goccamnhan_feedbacks",
                "feedback_list",
                "feedback_pagination"
            };
            
            foreach (var key in keysToRemove)
            {
                cache.Remove(key);
            }
        }
    }
}