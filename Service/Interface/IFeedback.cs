using websitebenhvien.Models.EnitityVM;

namespace websitebenhvien.Service.Interface
{
    public interface IFeedback
    {
        public Task<(string Message, bool Success)> AddFeedbackAsync(FeedbackVM model);

        public Task<(List<FeedbackVM> ds, int TotalPages)> GetAllFeedbacksAsync(string search, int page, int pagesize);

        public Task<FeedbackVM> GetFeedbackByIdAsync(int id);

        public Task<bool> DeleteFeedbackAsync(int id);

        public Task<(string Message, bool Success)> UpdateFeedbackAsync(FeedbackVM model);
    }
}
