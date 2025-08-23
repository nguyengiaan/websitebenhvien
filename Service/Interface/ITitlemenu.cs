using websitebenhvien.Models.Enitity;
using websitebenhvien.Models.EnitityVM;

namespace websitebenhvien.Service.Interface
{
    public interface ITitlemenu
    {
        // Lấy danh sách có phân trang và tìm kiếm
        Task<PagedTitlemenuResult> GetPagedTitlemenusAsync(TitlemenuSearchVM searchModel);
        
        // Lấy theo ID
        Task<TitlemenuVM?> GetTitlemenuByIdAsync(int id);
        
        // Thêm mới
        Task<bool> AddTitlemenuAsync(TitlemenuVM model);
        
        // Cập nhật
        Task<bool> UpdateTitlemenuAsync(TitlemenuVM model);
        
        // Xóa
        Task<bool> DeleteTitlemenuAsync(int id);
        
        // Kiểm tra tồn tại
        Task<bool> TitlemenuExistsAsync(int id);
        
        // Kiểm tra trùng tên (cho validation)
        Task<bool> IsNameExistsAsync(string name, int? excludeId = null);
        
        // Lấy tất cả (cho dropdown)
        Task<List<TitlemenuVM>> GetAllTitlemenusAsync();
    }
}
