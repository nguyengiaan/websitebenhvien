using websitebenhvien.Models.Enitity;
using websitebenhvien.Models.EnitityVM;

namespace websitebenhvien.Service.Interface
{
    public interface IRecruitment
    {
        public Task<Boolean> AddRecruitment(Postpersonnel recruitmentpost);
        //video 
        public Task<Boolean> AddVideo(VideosVM video);

        public Task<(int Totalpages, List<VideosVM> Data)> GetAllVideo(string search,int page, int pageSize);

        public Task<Boolean> DeleteVideo(int id);

        public Task<List<VideosVM>> GetVideosClient();
        //machine
        public Task<Boolean> AddMachine(MachineVM machine);

        public Task<(int Totalpages, List<MachineVM> Data)> GetAllMachine(string search, int page, int pageSize);
        public Task<Boolean> DeleteMachine(int id); 


        public Task<List<MachineVM>> GetMachineClient();

    }
}
