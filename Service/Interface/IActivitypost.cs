using websitebenhvien.Models.EnitityVM;

namespace websitebenhvien.Service.Interface
{
    public interface IActivitypost
    {
        Task<PagedPostactivityResult> GetPagedPostactivitiesAsync(PostactivitySearchVM searchModel);
        Task<PostactivityVM?> GetPostactivityByIdAsync(int id);
        Task<bool> AddPostactivityAsync(PostactivityVM model);
        Task<bool> UpdatePostactivityAsync(PostactivityVM model);
        Task<bool> DeletePostactivityAsync(int id);
        Task<bool> PostactivityExistsAsync(int id);
        Task<bool> IsTitleExistsAsync(string title, int? excludeId = null);
        Task<bool> IsUrlExistsAsync(string url, int? excludeId = null);
        Task<List<PostactivityVM>> GetAllPostactivitiesAsync();
    }
}
