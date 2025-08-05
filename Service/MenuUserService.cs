using Microsoft.EntityFrameworkCore;
using websitebenhvien.Data;
using websitebenhvien.Models.Enitity;

namespace websitebenhvien.Service
{
    public class MenuUserService : IMenuUserService
    {
        private readonly MyDbcontext _context;

        public MenuUserService(MyDbcontext context)
        {
            _context = context;
        }

        public async Task<List<MenuAdmin>> GetMenusForUserAsync(string userId)
        {
            try
            {
                var userMenus = await _context.MenuAdminUsers
                    .Where(mau => mau.UserId == userId)
                    .Include(mau => mau.MenuAdmin)
                    .Select(mau => mau.MenuAdmin)
                    .OrderBy(m => m.Title)
                    .ToListAsync();

                return userMenus;
            }
            catch (Exception)
            {
                return new List<MenuAdmin>();
            }
        }

        public async Task<bool> UserHasMenuAccessAsync(string userId, int menuId)
        {
            try
            {
                return await _context.MenuAdminUsers
                    .AnyAsync(mau => mau.UserId == userId && mau.MenuAdminId == menuId);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UserHasMenuAccessAsync(string userId, string menuTitle)
        {
            try
            {
                return await _context.MenuAdminUsers
                    .Include(mau => mau.MenuAdmin)
                    .AnyAsync(mau => mau.UserId == userId && mau.MenuAdmin.Title == menuTitle);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
