using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using websitebenhvien.Data;
using websitebenhvien.Models.EnitityVM;

namespace websitebenhvien.Controllers
{
    public class ActivityController : Controller
    {
        private readonly MyDbcontext _context;

        public ActivityController(MyDbcontext context)
        {
            _context = context;
        }

        // GET: /Activity/{categoryAlias}
        [Route("hoat-dong/{categoryAlias}")]
        public async Task<IActionResult> Category(string categoryAlias, int page = 1, int pageSize = 12)
        {
            // Tìm danh mục theo alias
            var category = await _context.Activitycategories
                .FirstOrDefaultAsync(c => c.link_alias == categoryAlias);

            if (category == null)
            {
                return NotFound("Không tìm thấy danh mục hoạt động này.");
            }

            // Lấy tất cả danh mục để hiển thị sidebar
            var allCategories = await _context.Activitycategories
                .Select(c => new ActivitycategoryVM
                {
                    Id_activitycategory = c.Id_activitycategory,
                    Title = c.Title,
                    Description = c.Description,
                    link_alias = c.link_alias
                })
                .ToListAsync();

            ViewBag.AllCategories = allCategories;

            // Lấy danh sách bài viết theo danh mục với phân trang
            var query = _context.Postactivity
                .Where(p => p.Id_Categoryactivity == category.Id_activitycategory && p.Status == true)
                .OrderByDescending(p => p.Createat);

            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var posts = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new PostactivityVM
                {
                    Id_Postactivity = p.Id_Postactivity,
                    Title = p.Title,
                    Description = p.Description,
                    Alias_url = p.Alias_url,
                    Keyword = p.Keyword,
                    Descriptionshort = p.Descriptionshort,
                    Id_Categoryactivity = p.Id_Categoryactivity,
                    Status = p.Status,
                    SchemaMakup = p.SchemaMakup,
                    Thumbnail = p.Thumbnail,
                    Createat = p.Createat,
                    CategoryName = category.Title
                })
                .ToListAsync();

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
            var post = await _context.Postactivity
                .Include(p => p.Activitycategory)
                .FirstOrDefaultAsync(p => p.Alias_url.Contains(alias) && p.Status == true);

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

            return View(viewModel);
        }

        // API: Lấy bài viết liên quan
        [HttpGet]
        [Route("api/activity/related/{postId}")]
        public async Task<IActionResult> GetRelatedPosts(int postId, int limit = 5)
        {
            var currentPost = await _context.Postactivity
                .Include(p => p.Activitycategory)
                .FirstOrDefaultAsync(p => p.Id_Postactivity == postId);

            if (currentPost == null)
            {
                return NotFound();
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
                            p.Keyword.Split().Any(k => currentPost.Keyword.Contains(k.Trim()))))
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
            var query = _context.Postactivity
                .Include(p => p.Activitycategory)
                .Where(p => p.Status == true);

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(p => p.Title.Contains(search) || 
                                       (p.Descriptionshort != null && p.Descriptionshort.Contains(search)));
            }

            query = query.OrderByDescending(p => p.Createat);

            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var posts = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new PostactivityVM
                {
                    Id_Postactivity = p.Id_Postactivity,
                    Title = p.Title,
                    Description = p.Description,
                    Alias_url = p.Alias_url,
                    Keyword = p.Keyword,
                    Descriptionshort = p.Descriptionshort,
                    Id_Categoryactivity = p.Id_Categoryactivity,
                    Status = p.Status,
                    SchemaMakup = p.SchemaMakup,
                    Thumbnail = p.Thumbnail,
                    Createat = p.Createat,
                    CategoryName = p.Activitycategory != null ? p.Activitycategory.Title : ""
                })
                .ToListAsync();

            // Lấy danh sách categories để hiển thị menu
            var categories = await _context.Activitycategories
                .Select(c => new ActivitycategoryVM
                {
                    Id_activitycategory = c.Id_activitycategory,
                    Title = c.Title,
                    Description = c.Description,
                    link_alias = c.link_alias
                })
                .ToListAsync();

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
    }
}
