using Microsoft.AspNetCore.Mvc;
using websitebenhvien.Models.Enitity;
using websitebenhvien.Models.EnitityVM;
using websitebenhvien.Service.Interface;
using Microsoft.AspNetCore.Authorization;

namespace websitebenhvien.Areas.Admin.Controllers
{
    public class SpecialtyController : Controller
    {
       
        private readonly ISpecialty _specialty;

       
        public SpecialtyController(ISpecialty specialty)
        {
            _specialty = specialty;

        }
         // quản lý chuyên khoa
        [Authorize(Roles = "admin,Quanlychuyenkhoa")]
        [HttpPost]
        public async Task<IActionResult> AddSpecialty(SpecialtyVM specialty)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    // Trả về thông báo lỗi từ ModelState
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                    return Json(new { status = false, message = string.Join(", ", errors), errors });
                }
                else
                {
                    var result = await _specialty.AddSpecialty(specialty);
                    return Json(new { status = result, message = result == true ? "Thêm chuyên khoa thành công" : "Thêm chuyên khoa thất bại" });

                }
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }
        }
        [Authorize(Roles = "admin,Quanlychuyenkhoa")]
        [HttpPost("/api/lay-danh-sach-chuyen-khoa")]
        public async Task<IActionResult> GetSpecialty(int page, int pageSize)
        {
            try
            {
                var data = await _specialty.GetSpecialty(page, pageSize);
                return Json(new { success = true, data = data.ds, totalPages = data.TotalPages ,page=page});


            }
            catch(Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }
        }
        [Authorize(Roles = "admin,Quanlychuyenkhoa")]
        [HttpPost("/api/lay-chuyen-khoa-theo-id")]
        public async Task<IActionResult> GetSpecialtyById(int id)
        {
            try{
                var data = await _specialty.GetSpecialtyById(id);
                return Json(new { success = true, data = data });
            }catch(Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }   
        }
        [Authorize(Roles = "admin,Quanlychuyenkhoa")]
        [HttpPost("/api/xoa-chuyen-khoa")]
        public async Task<IActionResult> DeleteSpecialty(int id)
        {
            try
            {
                var result = await _specialty.DeleteSpecialty(id);
                return Json(new { success = result, message = result == true ? "Xóa chuyên khoa thành công" : "Xóa chuyên khoa thất bại" });

            }catch(Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }
        }
        [HttpGet("/api/chuyen-khoa-catogery")]
        public async Task<IActionResult> GetAllSpecialty()
        {
            try
            {
                var data = await _specialty.GetAllSpecialty();
                return Json(new { success = true, data = data });
            }
            catch(Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }
        }
        // quản lý bác sĩ 
        [Authorize]
        [HttpPost("/api/them-bac-si")]
        public async Task<IActionResult> AddDoctor(DoctorVM doctor)
        {
            try{    
                if (!ModelState.IsValid)
                {
                    // Trả về thông báo lỗi từ ModelState
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                    return Json(new { status = false, message = string.Join(", ", errors), errors });
                }
                var result = await _specialty.AddDoctor(doctor);
                if(result)
                {
                    return Json(new { success = true, message = "Thêm bác sĩ thành công" });
                }
                else
                {
                    return Json(new { success = false, message = "Thêm bác sĩ thất bại" });
                }
            }catch(Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        [Authorize]
        [HttpPost("/api/lay-danh-sach-bac-si")]
        public async Task<IActionResult> GetDoctorByAlias(int page, int pageSize,string search,int specialtyId)
        {
            try{
                var data = await _specialty.GetDoctorByAlias(page, pageSize,search,specialtyId);
                return Json(new { success = true, data = data.ds, totalPages = data.TotalPages, page = page });
            }
            catch(Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }
        }
        [Authorize]
        [HttpPost("/api/lay-bac-si-theo-chuyen-khoa")]
        public async Task<IActionResult> GetDoctorBySpecialty(int id)
        {
            try{
                var data = await _specialty.GetDoctorBySpecialty(id);
                if(data != null)
                {
                    return Json(new { success = true, data = data });
                }
                else
                {
                    return Json(new { success = false, message = "Không tìm thấy bác sĩ" });
                }
            }catch(Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }
        }
        [Authorize]
        [HttpPost("/api/xoa-bac-si")]
        public async Task<IActionResult> DeleteDoctor(int id)
        {
            try
            {
                var result = await _specialty.DeleteDoctor(id);
                if(result)
                {
                    return Json(new { success = true, message = "Xóa bác sĩ thành công" });
                }
                else
                {
                    return Json(new { success = false, message = "Xóa bác sĩ thất bại" });
                }
            }catch(Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }
        }
    }
}
