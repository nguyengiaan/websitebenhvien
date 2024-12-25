using websitebenhvien.Models.EnitityVM;

namespace websitebenhvien.Service.Interface
{
    public interface ISamplemessager
    {
        public Task<Boolean> AddSamplemessager(SampleVM message);

        public Task<(List<SampleVM> ds, int TotalPages)> Getlistsp(int page, int pagesize);

        public Task<SampleVM> GetSamplemessager(int id);

        public Task<bool> DeleteSamplemessager(int id);

        public Task<bool> UpdateStatus(int id);
    }
}
