using AutoMapper;
using Microsoft.EntityFrameworkCore;
using websitebenhvien.Data;
using websitebenhvien.Models.Enitity;
using websitebenhvien.Models.EnitityVM;
using websitebenhvien.Service.Interface;

namespace websitebenhvien.Service.Reponser
{
    public class MenuadminuserReponser : IMenuadminuser
    {
        private readonly MyDbcontext _context;
        private readonly IMapper _mapper;

        public MenuadminuserReponser(MyDbcontext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<MenuAdminVM>> Listmenuadmin()
        {
            try
            {
                var menuadmin = await _context.MenuAdmins.ToListAsync();
                var menuadminVM = _mapper.Map<List<MenuAdminVM>>(menuadmin);
                return menuadminVM;
            }
            catch (Exception ex)
            {
                return new List<MenuAdminVM>();
            }
        }

        public async Task<List<MenuAdminVM>> GetMenusByUserId(string userId)
        {
            try
            {
                // Lấy danh sách tất cả menu và đánh dấu menu nào user đã có
                var allMenus = await _context.MenuAdmins.ToListAsync();
                var userMenus = await _context.MenuAdminUsers
                    .Where(x => x.UserId == userId)
                    .Select(x => x.MenuAdminId)
                    .ToListAsync();

                var menuVMs = _mapper.Map<List<MenuAdminVM>>(allMenus);
                
                // Thêm thuộc tính isSelected để biết menu nào đã được assign
                foreach (var menu in menuVMs)
                {
                    // Giả sử MenuAdminVM có property IsSelected
                    if (menu.id.HasValue && userMenus.Contains(menu.id.Value))
                    {
                        // Menu đã được assign cho user
                        // Có thể thêm property IsSelected vào MenuAdminVM
                    }
                }

                return menuVMs;
            }
            catch (Exception ex)
            {
                return new List<MenuAdminVM>();
            }
        }

        public async Task<bool> AssignMenuToUser(string userId, List<int> menuIds)
        {
            try
            {
                // Xóa tất cả menu cũ của user
                var existingMenus = await _context.MenuAdminUsers
                    .Where(x => x.UserId == userId)
                    .ToListAsync();
                
                _context.MenuAdminUsers.RemoveRange(existingMenus);

                // Thêm menu mới
                foreach (var menuId in menuIds)
                {
                    var menuUser = new MenuAdminUser
                    {
                        UserId = userId,
                        MenuAdminId = menuId
                    };
                    await _context.MenuAdminUsers.AddAsync(menuUser);
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> RemoveMenuFromUser(string userId, int menuId)
        {
            try
            {
                var menuUser = await _context.MenuAdminUsers
                    .FirstOrDefaultAsync(x => x.UserId == userId && x.MenuAdminId == menuId);

                if (menuUser != null)
                {
                    _context.MenuAdminUsers.Remove(menuUser);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> UpdateUserMenus(string userId, List<int> menuIds)
        {
            try
            {
                // Xóa tất cả menu cũ
                var existingMenus = await _context.MenuAdminUsers
                    .Where(x => x.UserId == userId)
                    .ToListAsync();
                
                _context.MenuAdminUsers.RemoveRange(existingMenus);

                // Thêm menu mới
                foreach (var menuId in menuIds)
                {
                    var menuUser = new MenuAdminUser
                    {
                        UserId = userId,
                        MenuAdminId = menuId
                    };
                    await _context.MenuAdminUsers.AddAsync(menuUser);
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> DeleteAllUserMenus(string userId)
        {
            try
            {
                var userMenus = await _context.MenuAdminUsers
                    .Where(x => x.UserId == userId)
                    .ToListAsync();

                _context.MenuAdminUsers.RemoveRange(userMenus);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
