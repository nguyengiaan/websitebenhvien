using websitebenhvien.Models.Enitity;

namespace websitebenhvien.Service
{
    public interface IMenuUserService
    {
        Task<List<MenuAdmin>> GetMenusForUserAsync(string userId);
        Task<bool> UserHasMenuAccessAsync(string userId, int menuId);
        Task<bool> UserHasMenuAccessAsync(string userId, string menuTitle);
    }
}
