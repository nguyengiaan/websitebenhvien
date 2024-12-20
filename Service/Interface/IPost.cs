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

        // hiển thị danh sách chat khách hàng 
        public Task<List<Chat>> GetChat();  

        // hiển thị chi tiet tin nhan   
        public Task<List<Chat>> GetChatById(string id);

        // rep tin nhắn khách hàng 
        public Task<Boolean> RepChat(string id,string message);
        // xem tin nhắn khách hàng 
        public Task<Boolean> ViewChat(string id);
        // XOÁ KHÁCH HÀNG 
        public Task<Boolean> DelChat(string id);

        // lấy chi tiết chuyên khoa 
        public Task<(Specialty,List<DoctorVM>)> GetSpecialtyById(string alias_url);

        // lấy chi tiết bác sĩ
        public Task<DoctorVM> GetDoctorById(string alias_url);


    }
}

