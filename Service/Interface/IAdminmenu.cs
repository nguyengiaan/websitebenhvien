using websitebenhvien.Models.EnitityVM;

namespace websitebenhvien.Service.Interface
{
    public interface IAdminmenu
    {
        public Task<PaginatedMenuAdminVM> Danhsachmenu(int page, int pagesize, string? searchTerm = null);

        public Task<Boolean> Create(MenuAdminVM menuAdminVM);

        public Task<bool> Update(MenuAdminVM menuAdminVM);

        public Task<bool> Delete(int id);

        public Task<MenuAdminVM?> MenuId(int id);
    }
}
