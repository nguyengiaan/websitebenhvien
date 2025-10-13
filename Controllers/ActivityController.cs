using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using websitebenhvien.Data;
using websitebenhvien.Models.EnitityVM;

namespace websitebenhvien.Controllers
{
    public class ActivityController : Controller
    {
        private readonly MyDbcontext _context;
        private readonly IMemoryCache _cache;

        public ActivityController(MyDbcontext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        // GET: /Activity/{categoryAlias}
        [Route("hoat-dong/{categoryAlias}")]
        public async Task<IActionResult> Category(string categoryAlias, int page = 1, int pageSize = 12)
        {
            // Cache key cho danh mục
            var categoryCacheKey = $"activity_category_{categoryAlias}";
            var allCategoriesCacheKey = "all_activity_categories";
             categoryAlias = "/hoat-dong/" + categoryAlias;
            // Debug: Log thông tin đầu vào
            if (string.IsNullOrEmpty(categoryAlias))
            {
                return BadRequest("Category alias không được để trống.");
            }

            // Tìm danh mục theo alias với cache
            var category = await _cache.GetOrCreateAsync(categoryCacheKey, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
                entry.Priority = CacheItemPriority.High;
                
                return await _context.Activitycategories
                    .FirstOrDefaultAsync(c => !string.IsNullOrEmpty(c.link_alias) && 
                                            c.link_alias.ToLower() == categoryAlias.ToLower());
            });

            if (category == null)
            {
                // Debug: Kiểm tra tất cả danh mục có trong DB
                var allAliases = await _context.Activitycategories
                    .Where(c => !string.IsNullOrEmpty(c.link_alias))
                    .Select(c => c.link_alias)
                    .ToListAsync();
                
                ViewBag.DebugInfo = new
                {
                    RequestedAlias = categoryAlias,
                    AvailableAliases = allAliases,
                    Message = $"Không tìm thấy danh mục với alias: {categoryAlias}"
                };
                
                return NotFound("Không tìm thấy danh mục hoạt động này.");
            }

            // Lấy tất cả danh mục để hiển thị sidebar với cache
            var allCategories = await _cache.GetOrCreateAsync(allCategoriesCacheKey, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
                entry.Priority = CacheItemPriority.High;
                
                return await _context.Activitycategories
                    .Select(c => new ActivitycategoryVM
                    {
                        Id_activitycategory = c.Id_activitycategory,
                        Title = c.Title,
                        Description = c.Description,
                        link_alias = c.link_alias
                    })
                    .ToListAsync();
            });

            ViewBag.AllCategories = allCategories;

            // Cache key cho bài viết theo danh mục và trang
            var postsCacheKey = $"activity_posts_{category.Id_activitycategory}_{page}_{pageSize}";
            var totalItemsCacheKey = $"activity_total_{category.Id_activitycategory}";

            // Cache tổng số bài viết
            var totalItems = await _cache.GetOrCreateAsync(totalItemsCacheKey, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
                entry.Priority = CacheItemPriority.Normal;
                
                return await _context.Postactivity
                    .Where(p => p.Id_Categoryactivity == category.Id_activitycategory && p.Status == true)
                    .CountAsync();
            });

            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            // Cache danh sách bài viết với phân trang
            var posts = await _cache.GetOrCreateAsync(postsCacheKey, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
                entry.Priority = CacheItemPriority.Normal;
                
                return await _context.Postactivity
                    .Where(p => p.Id_Categoryactivity == category.Id_activitycategory && p.Status == true)
                    .OrderByDescending(p => p.Createat)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(p => new PostactivityVM
                    {
                        Id_Postactivity = p.Id_Postactivity,
                        Title = p.Title,
                        Alias_url = p.Alias_url,
              
                        Status = p.Status,
                        Thumbnail = p.Thumbnail,
                        Createat = p.Createat,
                        CategoryName = category.Title
                    })
                    .ToListAsync();
            });

            var viewModel = new ActivityCategoryPageVM
            {
                Category = new ActivitycategoryVM
                {
                    Id_activitycategory = category.Id_activitycategory,
                    Title = category.Title,
                    Description = category.Description,
                    link_alias = category.link_alias
                },
                Posts = posts,
                CurrentPage = page,
                TotalPages = totalPages,
                TotalItems = totalItems,
                PageSize = pageSize,
                HasPreviousPage = page > 1,
                HasNextPage = page < totalPages
            };

            return View(viewModel);
        }

        // GET: /Activity/Detail/{alias}
        [Route("hoat-dong/chi-tiet/{alias}")]
        public async Task<IActionResult> Detail(string alias)
        {
            // Cache key cho bài viết chi tiết
            var postCacheKey = $"activity_detail_{alias}";
            
            var post = await _cache.GetOrCreateAsync(postCacheKey, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
                entry.Priority = CacheItemPriority.High;
                
                return await _context.Postactivity
                    .Include(p => p.Activitycategory)
                    .FirstOrDefaultAsync(p => p.Alias_url.Contains(alias) && p.Status == true);
            });

            if (post == null)
            {
                return NotFound("Không tìm thấy bài viết này.");
            }

            var viewModel = new PostactivityVM
            {
                Id_Postactivity = post.Id_Postactivity,
                Title = post.Title,
                Description = post.Description,
                Alias_url = post.Alias_url,
                Keyword = post.Keyword,
                Descriptionshort = post.Descriptionshort,
                Id_Categoryactivity = post.Id_Categoryactivity,
                Status = post.Status,
                SchemaMakup = post.SchemaMakup,
                Thumbnail = post.Thumbnail,
                Createat = post.Createat,
                CategoryName = post.Activitycategory?.Title
            };

            // Pass category ID and post ID to view for related posts
            ViewBag.CategoryId = post.Id_Categoryactivity;
            ViewBag.PostId = post.Id_Postactivity;
            ViewBag.CategoryAlias = post.Activitycategory?.link_alias;

            return View(viewModel);
        }

        // API: Lấy bài viết liên quan
        [HttpGet]
        [Route("api/activity/related/{postId}")]
        public async Task<IActionResult> GetRelatedPosts(int postId, int limit = 5)
        {
            // Cache key cho bài viết liên quan
            var relatedPostsCacheKey = $"activity_related_{postId}_{limit}";
            
            var result = await _cache.GetOrCreateAsync(relatedPostsCacheKey, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
                entry.Priority = CacheItemPriority.Normal;
                
                var currentPost = await _context.Postactivity
                    .Include(p => p.Activitycategory)
                    .FirstOrDefaultAsync(p => p.Id_Postactivity == postId);

                if (currentPost == null)
                {
                    return null;
                }

                // Tìm bài viết liên quan theo:
                // 1. Cùng danh mục
                // 2. Có từ khóa tương tự
                // 3. Loại bỏ bài viết hiện tại
                var relatedPosts = await _context.Postactivity
                    .Include(p => p.Activitycategory)
                    .Where(p => p.Status == true && p.Id_Postactivity != postId)
                    .Where(p => p.Id_Categoryactivity == currentPost.Id_Categoryactivity ||
                               (currentPost.Keyword != null && p.Keyword != null && 
                                p.Keyword.Split(Array.Empty<char>()).Any(k => currentPost.Keyword.Contains(k.Trim()))))
                    .OrderByDescending(p => p.Createat)
                    .Take(limit)
                    .Select(p => new
                    {
                        id = p.Id_Postactivity,
                        title = p.Title,
                        alias = p.Alias_url,
                        thumbnail = p.Thumbnail,
                        createAt = p.Createat.ToString("dd/MM/yyyy"),
                        categoryName = p.Activitycategory != null ? p.Activitycategory.Title : "",
                        shortDescription = p.Descriptionshort
                    })
                    .ToListAsync();

                return relatedPosts;
            });

            if (result == null)
            {
                return NotFound();
            }

            var relatedPosts = result;

            // Nếu không đủ bài viết cùng danh mục, lấy thêm bài viết mới nhất
            if (relatedPosts.Count < limit)
            {
                var additionalPosts = await _context.Postactivity
                    .Include(p => p.Activitycategory)
                    .Where(p => p.Status == true && p.Id_Postactivity != postId &&
                               !relatedPosts.Select(rp => rp.id).Contains(p.Id_Postactivity))
                    .OrderByDescending(p => p.Createat)
                    .Take(limit - relatedPosts.Count)
                    .Select(p => new
                    {
                        id = p.Id_Postactivity,
                        title = p.Title,
                        alias = p.Alias_url,
                        thumbnail = p.Thumbnail,
                        createAt = p.Createat.ToString("dd/MM/yyyy"),
                        categoryName = p.Activitycategory != null ? p.Activitycategory.Title : "",
                        shortDescription = p.Descriptionshort
                    })
                    .ToListAsync();

                relatedPosts.AddRange(additionalPosts);
            }

            return Json(relatedPosts);
        }

        // GET: /Activity (Trang tổng hợp tất cả hoạt động)
        [Route("hoat-dong")]
        public async Task<IActionResult> Index(int page = 1, int pageSize = 12, string? search = null)
        {
            // Cache key khác nhau cho search và không search
            var cacheKeySuffix = string.IsNullOrEmpty(search) ? "all" : $"search_{search.GetHashCode()}";
            var postsCacheKey = $"activity_index_posts_{cacheKeySuffix}_{page}_{pageSize}";
            var totalItemsCacheKey = $"activity_index_total_{cacheKeySuffix}";
            var categoriesCacheKey = "activity_index_categories";

            // Cache tổng số items
            var totalItems = await _cache.GetOrCreateAsync(totalItemsCacheKey, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
                entry.Priority = CacheItemPriority.Normal;
                
                var query = _context.Postactivity.Where(p => p.Status == true);
                
                if (!string.IsNullOrEmpty(search))
                {
                    query = query.Where(p => p.Title.Contains(search) || 
                                           (p.Descriptionshort != null && p.Descriptionshort.Contains(search)));
                }
                
                return await query.CountAsync();
            });

            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            // Cache danh sách posts
            var posts = await _cache.GetOrCreateAsync(postsCacheKey, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
                entry.Priority = CacheItemPriority.Normal;
                
                var query = _context.Postactivity
                    .Include(p => p.Activitycategory)
                    .Where(p => p.Status == true);

                if (!string.IsNullOrEmpty(search))
                {
                    query = query.Where(p => p.Title.Contains(search) || 
                                           (p.Descriptionshort != null && p.Descriptionshort.Contains(search)));
                }

                return await query
                    .OrderByDescending(p => p.Createat)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(p => new PostactivityVM
                    {
                        Id_Postactivity = p.Id_Postactivity,
                        Title = p.Title,
                        Alias_url = p.Alias_url,
                        Id_Categoryactivity = p.Id_Categoryactivity,
                        Status = p.Status,
                        Thumbnail = p.Thumbnail,
                        Createat = p.Createat,
                        CategoryName = p.Activitycategory != null ? p.Activitycategory.Title : ""
                    })
                    .ToListAsync();
            });

            // Cache danh sách categories
            var categories = await _cache.GetOrCreateAsync(categoriesCacheKey, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);
                entry.Priority = CacheItemPriority.High;
                
                return await _context.Activitycategories
                    .Select(c => new ActivitycategoryVM
                    {
                        Id_activitycategory = c.Id_activitycategory,
                        Title = c.Title,
                        Description = c.Description,
                        link_alias = c.link_alias
                    })
                    .ToListAsync();
            });

            var viewModel = new ActivityIndexPageVM
            {
                Posts = posts,
                Categories = categories,
                CurrentPage = page,
                TotalPages = totalPages,
                TotalItems = totalItems,
                PageSize = pageSize,
                HasPreviousPage = page > 1,
                HasNextPage = page < totalPages,
                SearchTerm = search
            };

            return View(viewModel);
        }

        // API: Get related posts by category
        [HttpGet]
        [Route("api/activity/related/{categoryId}")]
        public async Task<IActionResult> GetRelatedPosts(int categoryId, int currentPostId = 0, int take = 5)
        {
            // Cache key cho bài viết liên quan theo danh mục
            var relatedCategoryCacheKey = $"activity_related_category_{categoryId}_{currentPostId}_{take}";
            
            var relatedPosts = await _cache.GetOrCreateAsync(relatedCategoryCacheKey, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15);
                entry.Priority = CacheItemPriority.Normal;
                
                return await _context.Postactivity
                    .Include(p => p.Activitycategory)
                    .Where(p => p.Id_Categoryactivity == categoryId && 
                               p.Status == true && 
                               p.Id_Postactivity != currentPostId)
                    .OrderByDescending(p => p.Createat)
                    .Take(take)
                    .Select(p => new
                    {
                        id = p.Id_Postactivity,
                        title = p.Title,
                        thumbnail = p.Thumbnail,
                        alias = p.Alias_url,
                        createAt = p.Createat.ToString("dd/MM/yyyy"),
                        categoryName = p.Activitycategory.Title
                    })
                    .ToListAsync();
            });

            return Json(relatedPosts);
        }

        // API: Clear cache (for admin use)
        [HttpPost]
        [Route("api/activity/clear-cache")]
        public IActionResult ClearCache(string? type = null)
        {
            try
            {
                switch (type?.ToLower())
                {
                    case "categories":
                        InvalidateActivityCache();
                        break;
                    case "posts":
                        InvalidateAllPostsCache();
                        break;
                    case "all":
                    default:
                        InvalidateActivityCache();
                        InvalidateAllPostsCache();
                        break;
                }

                return Json(new { status = true, message = "Cache đã được xóa thành công" });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = $"Lỗi khi xóa cache: {ex.Message}" });
            }
        }

        // Cache helper methods
        private void InvalidateActivityCache()
        {
            // Xóa cache các danh mục
            var categoryCacheKeys = new[]
            {
                "all_activity_categories",
                "activity_index_categories"
            };

            foreach (var key in categoryCacheKeys)
            {
                _cache.Remove(key);
            }
        }

        private void InvalidateAllPostsCache()
        {
            // Trong thực tế, bạn nên implement cache key management system
            // Ở đây chỉ là ví dụ cơ bản - xóa một số cache keys phổ biến

            // Note: MemoryCache không có built-in pattern matching
            // Trong production, nên sử dụng Redis với pattern matching
            // hoặc maintain danh sách cache keys

            var commonCacheKeys = new[]
            {
                "activity_index_posts_all_1_12",
                "activity_index_posts_all_2_12", 
                "activity_index_posts_all_3_12",
                "activity_index_total_all"
            };

            foreach (var key in commonCacheKeys)
            {
                _cache.Remove(key);
            }
        }

        private void InvalidatePostCache(int categoryId, string? alias = null)
        {
            // Xóa cache cho danh mục cụ thể và bài viết
            var keysToRemove = new List<string>
            {
                $"activity_category_{alias}",
                $"activity_total_{categoryId}",
                $"activity_detail_{alias}"
            };

            // Xóa cache posts của danh mục (các trang khác nhau)
            for (int page = 1; page <= 10; page++) // Giả sử tối đa 10 trang
            {
                keysToRemove.Add($"activity_posts_{categoryId}_{page}_12");
            }

            foreach (var key in keysToRemove)
            {
                _cache.Remove(key);
            }
        }

        // API: Debug - Lấy tất cả categories
        [HttpGet]
        [Route("api/activity/debug/categories")]
        public async Task<IActionResult> DebugCategories()
        {
            try
            {
                var categories = await _context.Activitycategories
                    .Select(c => new
                    {
                        id = c.Id_activitycategory,
                        title = c.Title,
                        alias = c.link_alias,
                        description = c.Description
                    })
                    .ToListAsync();

                return Json(new
                {
                    status = true,
                    count = categories.Count,
                    data = categories
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    status = false,
                    message = ex.Message
                });
            }
        }

        // API: Tạo dữ liệu test categories
        [HttpPost]
        [Route("api/activity/debug/create-test-data")]
        public async Task<IActionResult> CreateTestData()
        {
            try
            {
                // Kiểm tra xem đã có dữ liệu chưa
                var existingCount = await _context.Activitycategories.CountAsync();
                if (existingCount > 0)
                {
                    return Json(new
                    {
                        status = false,
                        message = $"Đã có {existingCount} danh mục. Không tạo dữ liệu test."
                    });
                }

                // Tạo danh mục test
                var testCategories = new[]
                {
                    new websitebenhvien.Models.Enitity.Activitycategory
                    {
                        Id_activitycategory = 1,
                        Title = "Hoạt động cộng đồng",
                        Description = "Các hoạt động phục vụ cộng đồng",
                        link_alias = "hoat-dong-cong-dong"
                    },
                    new websitebenhvien.Models.Enitity.Activitycategory
                    {
                        Id_activitycategory = 2,
                        Title = "Sự kiện y tế",
                        Description = "Các sự kiện liên quan đến y tế",
                        link_alias = "su-kien-y-te"
                    },
                    new websitebenhvien.Models.Enitity.Activitycategory
                    {
                        Id_activitycategory = 3,
                        Title = "Chương trình khám bệnh",
                        Description = "Các chương trình khám bệnh miễn phí",
                        link_alias = "chuong-trinh-kham-benh"
                    },
                    new websitebenhvien.Models.Enitity.Activitycategory
                    {
                        Id_activitycategory = 4,
                        Title = "Hoạt động đào tạo",
                        Description = "Các hoạt động đào tạo nhân viên y tế",
                        link_alias = "hoat-dong-dao-tao"
                    }
                };

                _context.Activitycategories.AddRange(testCategories);
                await _context.SaveChangesAsync();

                // Clear cache
                InvalidateActivityCache();

                return Json(new
                {
                    status = true,
                    message = $"Đã tạo {testCategories.Length} danh mục test thành công",
                    data = testCategories.Select(c => new { c.Title, c.link_alias })
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    status = false,
                    message = $"Lỗi khi tạo dữ liệu test: {ex.Message}"
                });
            }
        }
    }
}
