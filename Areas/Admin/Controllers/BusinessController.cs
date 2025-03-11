using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using websitebenhvien.Data;
using websitebenhvien.Migrations;
using websitebenhvien.Models.Enitity;
using websitebenhvien.Service.Interface;

namespace websitebenhvien.Areas.Admin.Controllers
{
    public class BusinessController : Controller
    {
        private readonly IForbusiness _forbusiness;

        public BusinessController(IForbusiness forbusiness)
        {
            _forbusiness = forbusiness;
        }
        [Authorize(Roles = "admin,Quanlydoanhnghiep")]
        [HttpPost("/api/quan-ly-doanh-nghiep")]
        public async Task<IActionResult> AddForbusiness(Forbusiness forbusiness)
        {
            try
            {

                var data = await _forbusiness.Addbusiness(forbusiness);
                if (data)
                {
                    return Json(new { status = true, message = "Thêm thành công" });
                }
                else
                {
                    return Json(new { status = false, message = "Thêm thất bại" });
                }
            }
            catch (Exception e)
            {
                return Json(new { status = false, message = e.Message });
            }
        }
        [Authorize(Roles = "admin,Quanlydoanhnghiep")]
        [HttpPost("/api/lay-ds-quan-ly-doanh-nghiep")]
        public async Task<IActionResult> Getbusiness(string search, int page, int pagesize)
        {
            try
            {
                var data = await _forbusiness.Getbusiness(search, page, pagesize);
                return Json(new { status = true, data = data.list, totalpage = data.Totalpage, pageindex = page });
            }
            catch (Exception e)
            {
                return Json(new { status = false, message = e.Message });
            }
        }
        [Authorize(Roles = "admin,Quanlydoanhnghiep")]
        [HttpPost("/api/xoa-quan-ly-doanh-nghiep")]
        public async Task<IActionResult> Deletebsn(int id)
        {
            try
            {
                var data = await _forbusiness.Deletebsn(id);
                if (data)
                {
                    return Json(new { status = true, message = "Xóa thành công" });
                }
                else
                {
                    return Json(new { status = false, message = "Xóa thất bại" });
                }
            }
            catch (Exception e)
            {
                return Json(new { status = false, message = e.Message });
            }
        }
        [Authorize(Roles= "admin,Quanlydoanhnghiep")]
        [HttpPost("/api/lay-1-quan-ly-doanh-nghiep")]
        public async Task<IActionResult> Getbusinessbyid(int id)
        {
            try
            {
                var data = await _forbusiness.Getbusinessbyid(id);
                return Json(new { status = true, data = data });
            }
            catch (Exception e)
            {
                return Json(new { status = false, message = e.Message });
            }
        }
        [Authorize(Roles = "admin,Quanlydoanhnghiep")]
        [HttpPost("/api/cap-nhat-quan-ly-doanh-nghiep")]
        public async Task<IActionResult> Updatebusiness(int id)
        {
            try
            {
                var data = await _forbusiness.Updatebusiness(id);
                if (data)
                {
                    return Json(new { status = true });
                }
                else
                {
                    return Json(new { status = false, message = "Cập nhật thất bại" });
                }
            }
            catch (Exception e)
            {
                return Json(new { status = false, message = e.Message });
            }
        }
    }
}
