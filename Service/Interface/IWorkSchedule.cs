﻿using websitebenhvien.Models.EnitityVM;

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

        public Task<(List<MakeanappointmentVM> ds, int total)> GetAppointment(int page, int pageSize, string search, int specialtyId);

        // xóa lịch khám bệnh
        public Task<Boolean> DeleteAppointment(int id);
        // cập nhật lịch khám bệnh 
        public Task<Boolean> UpdateAppointment(int id);

        // danh sách lịch khám sức khoẻ
        public Task<(List<MakeanappointmentVM> ds, int total)> GetAppointmentSK(int page, int pageSize, string search);

        // xóa lịch khám sức khoẻ
        public Task<Boolean> DeleteAppointmentSK(int id);
        // cập nhật lịch khám sức khoẻ
        public Task<Boolean> UpdateAppointmentSK(int id);
        // sơ đồ các thông số khám bệnh 
        
        public Task<RegisterChart> GetRegisterChart();  

        // sơ đồ dăng ký khám bệnh theo ngày 
        public Task<List<Charthealthdate>> GetCharthealthdate();  





    }
}

