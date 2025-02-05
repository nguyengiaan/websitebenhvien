using websitebenhvien.Models.Enitity;
using websitebenhvien.Models.EnitityVM;

namespace websitebenhvien.Service.Interface
{
    public interface IRecruitment
    {
        public Task<Boolean> AddRecruitment(RecruitmentpostVM recruitmentpost);
        //video 
        public Task<Boolean> AddVideo(VideosVM video);

        public Task<(int Totalpages, List<VideosVM> Data)> GetAllVideo(string search,int page, int pageSize);

        public Task<Boolean> DeleteVideo(int id);


        public Task<List<VideosVM>> GetVideosClient();

    }
}
