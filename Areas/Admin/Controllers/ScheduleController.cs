using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using websitebenhvien.Models.EnitityVM;
using websitebenhvien.Service.Interface;

namespace websitebenhvien.Areas.Admin.Controllers
{
    public class ScheduleController : Controller
    {
        private readonly IWorkSchedule _workSchedule;

        public ScheduleController(IWorkSchedule workSchedule)
        {
            _workSchedule = workSchedule;
        }
        // thêm lịch làm việc
        [Authorize]
        [HttpPost("/api/them-lich-lam-viec")]
        public async Task<IActionResult> AddSchedule(WorkdoctorVM workdoctor)
        {
            try
            {
                var data = await _workSchedule.AddWorkschedule(workdoctor);
                if (data)
                {
                    return Json(new { status = true, message = "Đã thêm lịch làm việc" });
                }
                else
                {
                    return Json(new { status = false, message = "Không thể thêm lịch làm việc" });

                }
            }
            catch (Exception ex)
            {
                return Json(new { message = "Có lỗi xảy ra" });
            }
        }
      
        [HttpPost("/api/lich-lam-viec")]
        public async Task<IActionResult> GetSchedule(int id_doctor)
        {
            try
            {
                var data = await _workSchedule.GetWorkschedule(id_doctor);
                if (data != null)
                {
                    return Json(new { status = true, data = data });
                }
                return Json(new { status = false, message = "hãy thêm lịch làm việc" });
            }
            catch (Exception ex)
            {
                return Json(new { message = "Có lỗi xảy ra" });
            }
        }
        [Authorize]
        [HttpPost("/api/xoa-lich-lam-viec")]
        public async Task<IActionResult> DeleteSchedule(int id)
        {
            try{
                var data=await _workSchedule.DeleteWorkschedule(id);
                if(data){
                    return Json(new { status = true, message = "Đã xóa lịch làm việc" });
                }
                return Json(new { status = false, message = "Không thể xóa lịch làm việc" });
            }catch(Exception ex){
                return Json(new { message = "Có lỗi xảy ra" });
            }
        }
        // lấy bác sĩ
        [HttpPost("/api/bac-si")]
        public async Task<IActionResult> GetDoctor(int id)
        {
            try
            {
                var data = await _workSchedule.GetDoctor(id);
                if (data != null)
                {
                    return Json(new { status = true, data = data });
                }
                return Json(new { status = false, message = "Không có bác sĩ" });
            }
            catch (Exception ex)
            {
                return Json(new { message = "Có lỗi xảy ra" });
            }
        }
        // đăng ký khám bệnh 
        [HttpPost("/api/dang-ky-kb")]
        public async Task<IActionResult> Makeanappointment(MakeanappointmentVM makeanappointment)
        {
            try
            { 
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                    return Json(new { status = false, message = string.Join(", ", errors), errors });
                }
                var data = await _workSchedule.RegisterWorkschedule(makeanappointment);
                if(data){
                    return Json(new { status = true,message = "Đăng ký khám bệnh thành công" });
                }
                return Json(new { status = false,message = "Đăng ký khám bệnh thất bại" });
            }
            catch(Exception ex){
                return Json(new { status = false,message = ex.Message });
            }
        }
        [Authorize]
        [HttpPost("/api/lay-danh-sach-dang-ky-kb")]
        public async Task<IActionResult> GetAppointment(int page, int pageSize, string search, int specialtyId)
        {
            try{
                var data = await _workSchedule.GetAppointment(page, pageSize, search, specialtyId);
                return Json(new { status = true, data = data.ds, total = data.total, page = page, pageSize = pageSize });
            }
            catch(Exception ex){
                return Json(new { status = false, message = ex.Message });
            }
        }   
        [Authorize]
        [HttpPost("/api/xoa-lich-khambenh")]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            try
            {
                var data = await _workSchedule.DeleteAppointment(id);
                if(data)
                {
                    return Json(new { status = true, message = "Đã xóa lịch khám bệnh" });
                }
                return Json(new { status = false, message = "Không thể xóa lịch khám bệnh" });
            }
            catch(Exception ex){
                return Json(new { status = false, message = ex.Message });
            }
        }
        [Authorize]
        [HttpPost("/api/cap-nhat-lich-kham-benh")]
        public async Task<IActionResult> UpdateAppointment(int id)
        {
            try
            {
                var data = await _workSchedule.UpdateAppointment(id);
                if(data)
                {
                    return Json(new { status = true, message = "Đã cập nhật trạng thái" });
                }
                return Json(new { status = false, message = "Không thể cập nhật trạng thái" });
            }
            catch(Exception ex){
                return Json(new { status = false, message = ex.Message });
            }
     
        }
        // danh sách lịch khám sức khoẻ
        [Authorize]
        [HttpPost("/api/lay-danh-sach-dang-ky-kham-sk")]
        public async Task<IActionResult> GetAppointmentSK(int page, int pageSize, string search)
        {
            try{
                var data = await _workSchedule.GetAppointmentSK(page, pageSize, search);
                return Json(new { status = true, data = data.ds, total = data.total, page = page, pageSize = pageSize });
            }
            catch(Exception ex){
                return Json(new { status = false, message = ex.Message });
            }
        }
        [Authorize]
        [HttpPost("/api/xoa-lich-kham-sk")]
        public async Task<IActionResult> DeleteAppointmentSK(int id)
        {
            try{
                var data = await _workSchedule.DeleteAppointmentSK(id);
                if(data){
                    return Json(new { status = true, message = "Đã xóa lịch khám sức khoẻ" });
                }
                return Json(new { status = false, message = "Không thể xóa lịch khám sức khoẻ" });
            }
            catch(Exception ex){
                return Json(new { status = false, message = ex.Message });
            }
        }
        [Authorize]
        [HttpPost("/api/cap-nhat-lich-kham-sk")]
        public async Task<IActionResult> UpdateAppointmentSK(int id)
        {
            try{
                var data = await _workSchedule.UpdateAppointmentSK(id);
                if(data){
                    return Json(new { status = true, message = "Đã cập nhật trạng thái" });
                }
                return Json(new { status = false, message = "Không thể cập nhật trạng thái" });
            }
            catch(Exception ex){
                return Json(new { status = false, message = ex.Message });
            }
        }
    }
}
