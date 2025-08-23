using Microsoft.EntityFrameworkCore;
using AutoMapper;
using websitebenhvien.Data;
using websitebenhvien.Models.Enitity;
using websitebenhvien.Models.EnitityVM;
using websitebenhvien.Service.Interface;
using websitebenhvien.Helper;

namespace websitebenhvien.Service.Reponser
{
    public class TitlemenuReponser : ITitlemenu
    {
        private readonly MyDbcontext _context;
        private readonly IMapper _mapper;
        private readonly Uploadfile _uploadfile;

        public TitlemenuReponser(MyDbcontext context, IMapper mapper, Uploadfile uploadfile)
        {
            _context = context;
            _mapper = mapper;
            _uploadfile = uploadfile;
        }

        public async Task<PagedTitlemenuResult> GetPagedTitlemenusAsync(TitlemenuSearchVM searchModel)
        {
            try
            {
                var query = _context.Titlemenus.AsQueryable();

                // Áp dụng tìm kiếm
                if (!string.IsNullOrWhiteSpace(searchModel.SearchTerm))
                {
                    var searchTerm = searchModel.SearchTerm.Trim().ToLower();
                    query = query.Where(x => x.Name.ToLower().Contains(searchTerm));
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

                var mappedItems = _mapper.Map<List<TitlemenuVM>>(items);

                return new PagedTitlemenuResult
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
            catch
            {
                // Log exception here
                throw;
            }
        }

        private IQueryable<Titlemenu> ApplySorting(IQueryable<Titlemenu> query, string sortBy, string sortDirection)
        {
            return sortBy.ToLower() switch
            {
                "name" => sortDirection.ToLower() == "desc" 
                    ? query.OrderByDescending(x => x.Name)
                    : query.OrderBy(x => x.Name),
                "id_titlemenu" => sortDirection.ToLower() == "desc"
                    ? query.OrderByDescending(x => x.Id_titlemenu)
                    : query.OrderBy(x => x.Id_titlemenu),
                _ => query.OrderBy(x => x.Name)
            };
        }

        public async Task<TitlemenuVM?> GetTitlemenuByIdAsync(int id)
        {
            try
            {
                var entity = await _context.Titlemenus.FirstOrDefaultAsync(x => x.Id_titlemenu == id);
                return entity != null ? _mapper.Map<TitlemenuVM>(entity) : null;
            }
            catch
            {
                // Log exception here
                throw;
            }
        }

        public async Task<bool> AddTitlemenuAsync(TitlemenuVM model)
        {
            try
            {
                var entity = _mapper.Map<Titlemenu>(model);
                
                // Xử lý upload file nếu có
                if (model.formFile != null && model.formFile.Length > 0)
                {
                    var uploadResult = await _uploadfile.SaveMedia(model.formFile);
                    if (uploadResult.Status == 1)
                    {
                        entity.Url_icon = uploadResult.Message;
                    }
                }

                _context.Titlemenus.Add(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                // Log exception here
                return false;
            }
        }

        public async Task<bool> UpdateTitlemenuAsync(TitlemenuVM model)
        {
            try
            {
                var entity = await _context.Titlemenus.FirstOrDefaultAsync(x => x.Id_titlemenu == model.Id_titlemenu);
                if (entity == null) return false;

                // Cập nhật thông tin
                entity.Name = model.Name;
                
                // Xử lý upload file mới nếu có
                if (model.formFile != null && model.formFile.Length > 0)
                {
                    // Xóa file cũ nếu có
                    if (!string.IsNullOrEmpty(entity.Url_icon))
                    {
                        _uploadfile.DeleteMedia(entity.Url_icon);
                    }
                    
                    var uploadResult = await _uploadfile.SaveMedia(model.formFile);
                    if (uploadResult.Status == 1)
                    {
                        entity.Url_icon = uploadResult.Message;
                    }
                }

                _context.Titlemenus.Update(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                // Log exception here
                return false;
            }
        }

        public async Task<bool> DeleteTitlemenuAsync(int id)
        {
            try
            {
                var entity = await _context.Titlemenus.FirstOrDefaultAsync(x => x.Id_titlemenu == id);
                if (entity == null) return false;

                // Xóa file nếu có
                if (!string.IsNullOrEmpty(entity.Url_icon))
                {
                    _uploadfile.DeleteMedia(entity.Url_icon);
                }

                _context.Titlemenus.Remove(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                // Log exception here
                return false;
            }
        }

        public async Task<bool> TitlemenuExistsAsync(int id)
        {
            try
            {
                return await _context.Titlemenus.AnyAsync(x => x.Id_titlemenu == id);
            }
            catch
            {
                // Log exception here
                return false;
            }
        }

        public async Task<bool> IsNameExistsAsync(string name, int? excludeId = null)
        {
            try
            {
                var query = _context.Titlemenus.Where(x => x.Name.ToLower() == name.ToLower());
                
                if (excludeId.HasValue)
                {
                    query = query.Where(x => x.Id_titlemenu != excludeId.Value);
                }

                return await query.AnyAsync();
            }
            catch
            {
                // Log exception here
                return false;
            }
        }

        public async Task<List<TitlemenuVM>> GetAllTitlemenusAsync()
        {
            try
            {
                var entities = await _context.Titlemenus
                    .OrderBy(x => x.Name)
                    .ToListAsync();

                return _mapper.Map<List<TitlemenuVM>>(entities);
            }
            catch
            {
                // Log exception here
                return new List<TitlemenuVM>();
            }
        }
    }
}
