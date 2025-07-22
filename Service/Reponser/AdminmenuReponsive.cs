using AutoMapper;
using Microsoft.EntityFrameworkCore;
using websitebenhvien.Data;
using websitebenhvien.Helper;
using websitebenhvien.Models.Enitity;
using websitebenhvien.Models.EnitityVM;
using websitebenhvien.Service.Interface;

namespace websitebenhvien.Service.Reponser
{
    public class AdminmenuReponsive : IAdminmenu
    {
        private readonly MyDbcontext _context;
        private readonly Uploadfile _uploadfile;
        private readonly IMapper _mapper;

        public AdminmenuReponsive(MyDbcontext context, Uploadfile uploadfile, IMapper mapper)
        {
            _context = context;
            _uploadfile = uploadfile;
            _mapper = mapper;
        }

        public async Task<Boolean> Create(MenuAdminVM menuAdminVM)
        {
            try
            {
                if (menuAdminVM.formFile != null)
                {
                    var file = await _uploadfile.SaveMedia(menuAdminVM.formFile);
                    if (file.Item1 == 0)
                    {
                        return false; // Lỗi khi lưu file
                    }
                    menuAdminVM.Icon = file.Item2; // Lưu đường dẫn file vào Image
                }
                var menu = _mapper.Map<MenuAdminVM, MenuAdmin>(menuAdminVM);
                await _context.MenuAdmins.AddAsync(menu);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<PaginatedMenuAdminVM> Danhsachmenu(int page, int pagesize, string? searchTerm = null)
        {
            try
            {
                var query = _context.MenuAdmins.AsQueryable();

                // Tìm kiếm theo tên menu hoặc URL
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    query = query.Where(x => x.Title.Contains(searchTerm) || (x.Url != null && x.Url.Contains(searchTerm)));
                }

                var totalCount = await query.CountAsync();
                var totalPages = (int)Math.Ceiling((double)totalCount / pagesize);

                var items = await query
                    .OrderBy(x => x.id)
                    .Skip((page - 1) * pagesize)
                    .Take(pagesize)
                    .Select(x => new MenuAdminVM
                    {
                        id = x.id,
                        Title = x.Title,
                        Icon = x.Icon,
                        Url = x.Url
                    })
                    .ToListAsync();

                return new PaginatedMenuAdminVM
                {
                    Items = items,
                    CurrentPage = page,
                    TotalPages = totalPages,
                    PageSize = pagesize,
                    TotalCount = totalCount,
                    SearchTerm = searchTerm
                };
            }
            catch (Exception)
            {
                return new PaginatedMenuAdminVM();
            }
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                var menu = await _context.MenuAdmins.FindAsync(id);
                if (menu == null)
                {
                    return false;
                }

                // Xóa file icon nếu có
                if (!string.IsNullOrEmpty(menu.Icon))
                {
                    var oldIconPath = _uploadfile.DeleteMedia(menu.Icon);
                 
                }

                _context.MenuAdmins.Remove(menu);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<MenuAdminVM?> MenuId(int id)
        {
            try
            {
                var menu = await _context.MenuAdmins.FindAsync(id);
                if (menu == null)
                {
                    return null;
                }

                return _mapper.Map<MenuAdmin, MenuAdminVM>(menu);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> Update(MenuAdminVM menuAdminVM)
        {
            try
            {
                var existingMenu = await _context.MenuAdmins.FindAsync(menuAdminVM.id);
                if (existingMenu == null)
                {
                    return false;
                }

                // Xử lý upload file mới nếu có
                if (menuAdminVM.formFile != null)
                {
                    // Xóa file cũ nếu có
                    if (!string.IsNullOrEmpty(existingMenu.Icon))
                    {
                        var oldIconPath = _uploadfile.DeleteMedia(existingMenu.Icon);
                       
                    }

                    // Lưu file mới
                    var file = await _uploadfile.SaveMedia(menuAdminVM.formFile);
                    if (file.Item1 == 0)
                    {
                        return false; // Lỗi khi lưu file
                    }
                    menuAdminVM.Icon = file.Item2;
                }
                else
                {
                    // Giữ nguyên icon cũ nếu không upload file mới
                    menuAdminVM.Icon = existingMenu.Icon;
                }

                // Cập nhật thông tin
                existingMenu.Title = menuAdminVM.Title;
                existingMenu.Url = menuAdminVM.Url;
                existingMenu.Icon = menuAdminVM.Icon;

                _context.MenuAdmins.Update(existingMenu);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
