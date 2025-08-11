using AutoMapper;
using Microsoft.EntityFrameworkCore;
using websitebenhvien.Data;
using websitebenhvien.Helper;
using websitebenhvien.Models.Enitity;
using websitebenhvien.Models.EnitityVM;
using websitebenhvien.Service.Interface;

namespace websitebenhvien.Service.Reponser
{
    public class ActivitypostReponsive : IActivitypost
    {
        private readonly MyDbcontext _context;
        private readonly IMapper _mapper;
        private readonly Uploadfile _uploadfile;

        public ActivitypostReponsive(MyDbcontext context, IMapper mapper, Uploadfile uploadfile)
        {
            _context = context;
            _mapper = mapper;
            _uploadfile = uploadfile;
        }

        public async Task<PagedPostactivityResult> GetPagedPostactivitiesAsync(PostactivitySearchVM searchModel)
        {
            var query = _context.Postactivity
                .Include(p => p.Activitycategory)
                .AsQueryable();

            // Tìm kiếm theo từ khóa
            if (!string.IsNullOrWhiteSpace(searchModel.SearchTerm))
            {
                query = query.Where(p => p.Title.Contains(searchModel.SearchTerm) || 
                                       (p.Descriptionshort != null && p.Descriptionshort.Contains(searchModel.SearchTerm)));
            }

            // Lọc theo danh mục
            if (searchModel.CategoryId.HasValue && searchModel.CategoryId > 0)
            {
                query = query.Where(p => p.Id_Categoryactivity == searchModel.CategoryId);
            }

            // Lọc theo trạng thái
            if (searchModel.Status.HasValue)
            {
                query = query.Where(p => p.Status == searchModel.Status);
            }

            // Đếm tổng số bản ghi
            var totalCount = await query.CountAsync();

            // Sắp xếp theo ngày tạo mới nhất
            query = query.OrderByDescending(p => p.Createat);

            // Phân trang
            var items = await query
                .Skip((searchModel.Page - 1) * searchModel.PageSize)
                .Take(searchModel.PageSize)
                .ToListAsync();

            var postactivities = _mapper.Map<List<PostactivityVM>>(items);

            return new PagedPostactivityResult
            {
                Items = postactivities,
                TotalItems = totalCount,
                Page = searchModel.Page,
                PageSize = searchModel.PageSize
            };
        }

        public async Task<PostactivityVM?> GetPostactivityByIdAsync(int id)
        {
            var postactivity = await _context.Postactivity
                .Include(p => p.Activitycategory)
                .FirstOrDefaultAsync(p => p.Id_Postactivity == id);

            return postactivity != null ? _mapper.Map<PostactivityVM>(postactivity) : null;
        }

        public async Task<bool> AddPostactivityAsync(PostactivityVM model)
        {
            try
            {
                var postactivity = _mapper.Map<Postactivity>(model);
                
                // Tự động tạo Alias URL
                if (string.IsNullOrEmpty(postactivity.Alias_url))
                {
                    postactivity.Alias_url = SeoHelper.GenerateAliasUrl(postactivity.Title);
                }
                
                // Tự động tạo từ khóa SEO
                if (string.IsNullOrEmpty(postactivity.Keyword))
                {
                    postactivity.Keyword = SeoHelper.GenerateSeoKeywords(postactivity.Title, postactivity.Descriptionshort);
                }
                
                // Upload thumbnail nếu có
                if (model.ThumbnailFile != null)
                {
                    var uploadResult = await _uploadfile.SaveMedia(model.ThumbnailFile, true, false, 75);
                    if (uploadResult.Item1 == 1)
                    {
                        postactivity.Thumbnail = uploadResult.Item2;
                    }
                }
                
                // Tự động tạo URL nếu trống
                if (string.IsNullOrEmpty(postactivity.Url))
                {
                    postactivity.Url = $"/hoat-dong/{postactivity.Alias_url}";
                }
                
                // Tự động tạo Schema Markup
                postactivity.SchemaMakup = SeoHelper.GenerateSchemaMarkup(
                    postactivity.Title,
                    postactivity.Descriptionshort,
                    postactivity.Url,
                    postactivity.Thumbnail,
                    postactivity.Createat
                );
                
                postactivity.Createat = DateTime.Now;
                
                _context.Postactivity.Add(postactivity);
                return await _context.SaveChangesAsync() > 0;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdatePostactivityAsync(PostactivityVM model)
        {
            try
            {
                var existingPostactivity = await _context.Postactivity
                    .FirstOrDefaultAsync(p => p.Id_Postactivity == model.Id_Postactivity);

                if (existingPostactivity == null)
                    return false;

                // Lưu thumbnail cũ để xóa nếu cần
                string? oldThumbnail = existingPostactivity.Thumbnail;
                bool shouldDeleteOldThumbnail = false;

                // Map các thuộc tính từ model sang entity (trừ thumbnail)
                var originalThumbnail = existingPostactivity.Thumbnail;
                _mapper.Map(model, existingPostactivity);
                
                // Xử lý thumbnail
                if (model.ThumbnailFile != null && model.ThumbnailFile.Length > 0)
                {
                    // Upload thumbnail mới
                    var uploadResult = await _uploadfile.SaveMedia(model.ThumbnailFile, true, false, 75);
                    if (uploadResult.Item1 == 1)
                    {
                        existingPostactivity.Thumbnail = uploadResult.Item2;
                        shouldDeleteOldThumbnail = true; // Đánh dấu để xóa file cũ
                    }
                    else
                    {
                        // Nếu upload thất bại, giữ lại thumbnail cũ
                        existingPostactivity.Thumbnail = originalThumbnail;
                    }
                }
                else if (string.IsNullOrEmpty(model.Thumbnail))
                {
                    // Nếu model.Thumbnail rỗng và không có file mới, người dùng muốn xóa thumbnail
                    existingPostactivity.Thumbnail = null;
                    shouldDeleteOldThumbnail = true;
                }
                else
                {
                    // Giữ lại thumbnail hiện tại nếu không có thay đổi
                    existingPostactivity.Thumbnail = originalThumbnail;
                }
                
                // Tự động cập nhật Alias URL nếu tiêu đề thay đổi
                if (string.IsNullOrEmpty(existingPostactivity.Alias_url))
                {
                    existingPostactivity.Alias_url = SeoHelper.GenerateAliasUrl(existingPostactivity.Title);
                }
                
                // Tự động cập nhật từ khóa SEO
                if (string.IsNullOrEmpty(existingPostactivity.Keyword))
                {
                    existingPostactivity.Keyword = SeoHelper.GenerateSeoKeywords(existingPostactivity.Title, existingPostactivity.Descriptionshort);
                }
                
                // Tự động cập nhật URL nếu trống
                if (string.IsNullOrEmpty(existingPostactivity.Url))
                {
                    existingPostactivity.Url = $"/hoat-dong/{existingPostactivity.Alias_url}";
                }
                
                // Tự động cập nhật Schema Markup
                existingPostactivity.SchemaMakup = SeoHelper.GenerateSchemaMarkup(
                    existingPostactivity.Title,
                    existingPostactivity.Descriptionshort,
                    existingPostactivity.Url,
                    existingPostactivity.Thumbnail,
                    existingPostactivity.Createat
                );
                
                // Lưu thay đổi vào database
                var result = await _context.SaveChangesAsync() > 0;
                
                // Xóa file cũ sau khi lưu thành công
                if (result && shouldDeleteOldThumbnail && !string.IsNullOrEmpty(oldThumbnail))
                {
                    try
                    {
                        _uploadfile.DeleteMedia(oldThumbnail);
                    }
                    catch
                    {
                        // Log error nhưng không làm thất bại toàn bộ operation
                        // Có thể implement logging ở đây
                    }
                }
                
                return result;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeletePostactivityAsync(int id)
        {
            try
            {
                var postactivity = await _context.Postactivity
                    .FirstOrDefaultAsync(p => p.Id_Postactivity == id);
            

                if (postactivity == null)
                    return false;

                // Xóa thumbnail nếu có
                if (!string.IsNullOrEmpty(postactivity.Thumbnail))
                {
                    _uploadfile.DeleteMedia(postactivity.Thumbnail);
                }
                

                _context.Postactivity.Remove(postactivity);
                return await _context.SaveChangesAsync() > 0;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> PostactivityExistsAsync(int id)
        {
            return await _context.Postactivity.AnyAsync(p => p.Id_Postactivity == id);
        }

        public async Task<bool> IsTitleExistsAsync(string title, int? excludeId = null)
        {
            var query = _context.Postactivity.Where(p => p.Title == title);
            
            if (excludeId.HasValue)
                query = query.Where(p => p.Id_Postactivity != excludeId.Value);

            return await query.AnyAsync();
        }

        public async Task<bool> IsUrlExistsAsync(string url, int? excludeId = null)
        {
            var query = _context.Postactivity.Where(p => p.Url == url);
            
            if (excludeId.HasValue)
                query = query.Where(p => p.Id_Postactivity != excludeId.Value);

            return await query.AnyAsync();
        }

        public async Task<List<PostactivityVM>> GetAllPostactivitiesAsync()
        {
            var postactivities = await _context.Postactivity
                .Include(p => p.Activitycategory)
                .OrderByDescending(p => p.Createat)
                .ToListAsync();

            return _mapper.Map<List<PostactivityVM>>(postactivities);
        }
    }
}
