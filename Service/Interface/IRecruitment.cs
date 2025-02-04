using websitebenhvien.Models.Enitity;
using websitebenhvien.Models.EnitityVM;

namespace websitebenhvien.Service.Interface
{
    public interface IRecruitment
    {
        public Task<Boolean> AddRecruitment(RecruitmentpostVM recruitmentpost);
    }
}
