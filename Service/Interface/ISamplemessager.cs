using websitebenhvien.Models.EnitityVM;

namespace websitebenhvien.Service.Interface
{
    public interface ISamplemessager
    {
        public Task<Boolean> AddSamplemessager(SampleVM message);
    }
}
