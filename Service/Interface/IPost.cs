using System.Threading.Tasks;
using websitebenhvien.Models.Enitity;
using websitebenhvien.Models.EnitityVM;
namespace websitebenhvien.Service.Interface
{
    public interface IPost
    {
        public Task<News> GetNewsById(string alias_url);

        // lấy list bài viết nỗi bật 
        public Task<List<NewsVM>> GetNewsByCategory(string alias_url);

        // nộp cv tuyển dụng
        public Task<Boolean> SubmitRecruitment(RecruitmentVM recruitment);

        // lấy danh sách tuyển dụng
        public Task<(List<RecruitmentVM> ds, int total)> GetRecruitment(int page, int pageSize);

        // lấy danh sách thông báo
        public Task<List<Notification>> GetNotification();

        // xem thông báo tuyển dụng 
        public Task<Boolean> ViewRecruitment();

        public Task<Boolean> DelRecruitment(string id);
        // chat khách hàng 
        
        public Task<Boolean> Chatcustomer(string message);
    }
}

