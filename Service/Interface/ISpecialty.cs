using websitebenhvien.Models.EnitityVM;

namespace websitebenhvien.Service.Interface
{
    public interface ISpecialty
    {
        public Task<Boolean> AddSpecialty(SpecialtyVM specialty);

        public Task<(List<SpecialtyVM> ds,int TotalPages)> GetSpecialty(int page, int pageSize);

        public Task<Boolean> DeleteSpecialty(int id);

        public Task<Boolean> UpdateSpecialty(SpecialtyVM specialty);
    }
}
