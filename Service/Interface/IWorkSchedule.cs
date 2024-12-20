using websitebenhvien.Models.EnitityVM;

namespace websitebenhvien.Service.Interface
{
    public interface IWorkSchedule
    {
        public Task<Boolean> AddWorkschedule(WorkdoctorVM workschedule);
        public Task<WorkdoctorVM> GetWorkschedule(int id);

        public Task<Boolean> DeleteWorkschedule(int id);

        // đăng ký lịch khám bệnh

        public Task<List<DoctorVM>> GetDoctor(int id);
        public Task<Boolean> RegisterWorkschedule(MakeanappointmentVM workschedule);

        public Task<List<MakeanappointmentVM>> GetAppointment(int page, int pageSize, string search, int specialtyId);

    }
}
