using websitebenhvien.Models.EnitityVM;

namespace websitebenhvien.Service.Interface
{
    public interface ISpecialty
    {
        public Task<Boolean> AddSpecialty(SpecialtyVM specialty);

        public Task<(List<SpecialtyVM> ds,int TotalPages)> GetSpecialty(int page, int pageSize);

        public Task<Boolean> DeleteSpecialty(int id);

        public Task<Boolean> UpdateSpecialty(SpecialtyVM specialty);

        public Task<SpecialtyVM> GetSpecialtyById(int id);
        // doctor
        public Task<Boolean> AddDoctor(DoctorVM doctor);

        public Task<Boolean> DeleteDoctor(int id);

        public Task<List<DoctorVM>> GetDoctorBySpecialty(int id);

        public Task<(List<DoctorVM> ds, int TotalPages)> GetDoctorByAlias(int page, int pageSize,string search,int specialtyId);

        public Task<List<SpecialtyVM>> GetAllSpecialty();

        

        

    }
}
