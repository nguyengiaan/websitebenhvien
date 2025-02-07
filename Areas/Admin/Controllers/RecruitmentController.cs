﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using websitebenhvien.Models.Enitity;
using websitebenhvien.Models.EnitityVM;
using websitebenhvien.Service.Interface;

namespace websitebenhvien.Areas.Admin.Controllers
{
    public class RecruitmentController : Controller
    {
        private readonly IRecruitment _recruitment;
        public RecruitmentController(IRecruitment recruitment)
        {
            _recruitment = recruitment;
        }
        [Authorize]
        [HttpPost("/api-tuyen-dung-benh-vien")]
        public async Task<IActionResult> dangtintuyendung(int id, string title_recruitmentpost, string Content_recruitmentpost, string Status)
        {
            try
            {
                Postpersonnel recruitmentpost = new Postpersonnel();
                recruitmentpost.id_recruitmentpost = id;
                recruitmentpost.title_recruitmentpost = title_recruitmentpost;
                recruitmentpost.Content_recruitmentpost = Content_recruitmentpost;
                recruitmentpost.Status = Status;
               
                var data = await _recruitment.AddRecruitment(recruitmentpost);
                if (data == true)
                {
                    return Json(new { status = true, message = "Đăng tin tuyển dụng thành công" });
                }
                else
                {
                    return Json(new { status = false, message = "Đăng tin tuyển dụng thất bại" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }

        }
        // video 
        [Authorize]
        [HttpPost("/api-video-benh-vien")]
        public async Task<IActionResult> dangtinvideo(VideosVM video)
        {
            try
            {
                var data = await _recruitment.AddVideo(video);
                if (data == true)
                {
                    return Json(new { status = true, message = "Đăng tin video thành công" });
                }
                else
                {
                    return Json(new { status = false, message = "Đăng tin video thất bại" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }
        }

        [Authorize(Roles = "admin")]
        [HttpGet("/api-get-all-video")]
        public async Task<IActionResult> getallvideo(string search, int page, int pageSize)
        {
            try
            {
                var data = await _recruitment.GetAllVideo(search, page, pageSize);
                return Json(new { status = true, data = data.Data, totalpage = data.Totalpages, pageindex = page });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }
        }
        [Authorize]
        [HttpPost("/api-delete-video")]
        public async Task<IActionResult> deletevideo(int id)
        {
            try
            {
                var data = await _recruitment.DeleteVideo(id);
                if (data == true)
                {
                    return Json(new { status = true, message = "Xóa video thành công" });
                }
                else
                {
                    return Json(new { status = false, message = "Xóa video thất bại" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }
        }
        [HttpGet("/api-lay-tat-ca-video")]
        public async Task<IActionResult> getallvideo()
        {
            try
            {
                var data = await _recruitment.GetVideosClient();
                return Json(new { status = true, data = data });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }
        }
        // machine
        [Authorize]
        [HttpPost("/api-machine-benh-vien")]
        public async Task<IActionResult> Addmachine(MachineVM machine)
        {
            try
            {

                var data = await _recruitment.AddMachine(machine);
                if (data == true)
                {
                    return Json(new { status = true, message = "Thêm máy thành công" });
                }
                else
                {
                    return Json(new { status = false, message = "Thêm máy thất bại" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }
        }
        [Authorize]
        [HttpPost("/api-get-all-machine")]
        public async Task<IActionResult> getallmachine(string search, int page, int pageSize)
        {
            try
            {
                var data = await _recruitment.GetAllMachine(search, page, pageSize);
                return Json(new { status = true, data = data.Data, totalpage = data.Totalpages, pageindex = page });

            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }
        }
        [Authorize]
        [HttpPost("/api-delete-machine")]
        public async Task<IActionResult> Deletemachine(int id)
        {
            try
            {

                var data = await _recruitment.DeleteMachine(id);
                if (data)
                {
                    return Json(new { status = true, message = "Xóa máy thành công" });

                }
                else
                {
                    return Json(new { status = false, message = "Xóa máy thất bại" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }
        }
        [HttpGet("/api-lay-tat-ca-may")]
        public async Task<IActionResult> getallmachine()
        {
            try
            {
                var data = await _recruitment.GetMachineClient();
                return Json(new { status = true, data = data });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }
        }
    }
}
