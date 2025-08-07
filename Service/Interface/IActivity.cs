using websitebenhvien.Models.Enitity;
using websitebenhvien.Models.EnitityVM;

namespace websitebenhvien.Service.Interface
{
    public interface IActivity
    {
        // Lấy danh sách có phân trang và tìm kiếm
        Task<PagedActivityCategoryResult> GetPagedActivityCategoriesAsync(ActivityCategorySearchVM searchModel);
        
        // Lấy theo ID
        Task<ActivitycategoryVM?> GetActivityCategoryByIdAsync(int id);
        
        // Thêm mới
        Task<bool> AddActivityCategoryAsync(ActivitycategoryVM model);
        
        // Cập nhật
        Task<bool> UpdateActivityCategoryAsync(ActivitycategoryVM model);
        
        // Xóa
        Task<bool> DeleteActivityCategoryAsync(int id);
        
        // Kiểm tra tồn tại
        Task<bool> ActivityCategoryExistsAsync(int id);
        
        // Kiểm tra trùng tên (cho validation)
        Task<bool> IsTitleExistsAsync(string title, int? excludeId = null);
        
        // Lấy tất cả (cho dropdown)
        Task<List<ActivitycategoryVM>> GetAllActivityCategoriesAsync();
    }
}
