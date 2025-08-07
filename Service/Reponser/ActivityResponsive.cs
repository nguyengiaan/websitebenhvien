using Microsoft.EntityFrameworkCore;
using AutoMapper;
using websitebenhvien.Data;
using websitebenhvien.Models.Enitity;
using websitebenhvien.Models.EnitityVM;
using websitebenhvien.Service.Interface;

namespace websitebenhvien.Service.Reponser
{
    public class ActivityResponsive : IActivity
    {
        private readonly MyDbcontext _context;
        private readonly IMapper _mapper;

        public ActivityResponsive(MyDbcontext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PagedActivityCategoryResult> GetPagedActivityCategoriesAsync(ActivityCategorySearchVM searchModel)
        {
            try
            {
                var query = _context.Activitycategories.AsQueryable();

                // Áp dụng tìm kiếm
                if (!string.IsNullOrWhiteSpace(searchModel.SearchTerm))
                {
                    var searchTerm = searchModel.SearchTerm.Trim().ToLower();
                    query = query.Where(x => x.Title.ToLower().Contains(searchTerm) ||
                                            (x.Description != null && x.Description.ToLower().Contains(searchTerm)));
                }

                // Đếm tổng số bản ghi trước khi phân trang
                var totalItems = await query.CountAsync();

                // Áp dụng sắp xếp
                query = ApplySorting(query, searchModel.SortBy, searchModel.SortDirection);

                // Áp dụng phân trang
                var items = await query
                    .Skip((searchModel.Page - 1) * searchModel.PageSize)
                    .Take(searchModel.PageSize)
                    .ToListAsync();

                var mappedItems = _mapper.Map<List<ActivitycategoryVM>>(items);

                return new PagedActivityCategoryResult
                {
                    Items = mappedItems,
                    TotalItems = totalItems,
                    Page = searchModel.Page,
                    PageSize = searchModel.PageSize,
                    SearchTerm = searchModel.SearchTerm,
                    SortBy = searchModel.SortBy,
                    SortDirection = searchModel.SortDirection
                };
            }
            catch (Exception)
            {
                return new PagedActivityCategoryResult();
            }
        }

        public async Task<ActivitycategoryVM?> GetActivityCategoryByIdAsync(int id)
        {
            try
            {
                var entity = await _context.Activitycategories
                    .FirstOrDefaultAsync(x => x.Id_activitycategory == id);
                
                return entity != null ? _mapper.Map<ActivitycategoryVM>(entity) : null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> AddActivityCategoryAsync(ActivitycategoryVM model)
        {
            try
            {
                var entity = _mapper.Map<Activitycategory>(model);
                
                _context.Activitycategories.Add(entity);
                var result = await _context.SaveChangesAsync();
                
                return result > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateActivityCategoryAsync(ActivitycategoryVM model)
        {
            try
            {
                var entity = await _context.Activitycategories
                    .FirstOrDefaultAsync(x => x.Id_activitycategory == model.Id_activitycategory);
                
                if (entity == null) return false;

                entity.Title = model.Title;
                entity.Description = model.Description;

                _context.Activitycategories.Update(entity);
                var result = await _context.SaveChangesAsync();
                
                return result > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteActivityCategoryAsync(int id)
        {
            try
            {
                var entity = await _context.Activitycategories
                    .FirstOrDefaultAsync(x => x.Id_activitycategory == id);
                
                if (entity == null) return false;

                _context.Activitycategories.Remove(entity);
                var result = await _context.SaveChangesAsync();
                
                return result > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> ActivityCategoryExistsAsync(int id)
        {
            try
            {
                return await _context.Activitycategories
                    .AnyAsync(x => x.Id_activitycategory == id);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> IsTitleExistsAsync(string title, int? excludeId = null)
        {
            try
            {
                var query = _context.Activitycategories
                    .Where(x => x.Title.ToLower() == title.ToLower());

                if (excludeId.HasValue)
                {
                    query = query.Where(x => x.Id_activitycategory != excludeId.Value);
                }

                return await query.AnyAsync();
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<List<ActivitycategoryVM>> GetAllActivityCategoriesAsync()
        {
            try
            {
                var entities = await _context.Activitycategories
                    .OrderBy(x => x.Title)
                    .ToListAsync();
                
                return _mapper.Map<List<ActivitycategoryVM>>(entities);
            }
            catch (Exception)
            {
                return new List<ActivitycategoryVM>();
            }
        }

        private static IQueryable<Activitycategory> ApplySorting(IQueryable<Activitycategory> query, string sortBy, string sortDirection)
        {
            return sortBy.ToLower() switch
            {
                "title" => sortDirection.ToLower() == "desc" 
                    ? query.OrderByDescending(x => x.Title)
                    : query.OrderBy(x => x.Title),
                "description" => sortDirection.ToLower() == "desc"
                    ? query.OrderByDescending(x => x.Description)
                    : query.OrderBy(x => x.Description),
                "id" => sortDirection.ToLower() == "desc"
                    ? query.OrderByDescending(x => x.Id_activitycategory)
                    : query.OrderBy(x => x.Id_activitycategory),
                _ => query.OrderBy(x => x.Title)
            };
        }
    }
}
