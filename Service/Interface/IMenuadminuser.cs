using websitebenhvien.Models.EnitityVM;

namespace websitebenhvien.Service.Interface
{
    public interface IMenuadminuser
    {
        public Task<List<MenuAdminVM>> Listmenuadmin();
        public Task<List<MenuAdminVM>> GetMenusByUserId(string userId);
        public Task<bool> AssignMenuToUser(string userId, List<int> menuIds);
        public Task<bool> RemoveMenuFromUser(string userId, int menuId);
        public Task<bool> UpdateUserMenus(string userId, List<int> menuIds);
        public Task<bool> DeleteAllUserMenus(string userId);
    }
}
