using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using websitebenhvien.Models.EnitityVM;
using websitebenhvien.Service.Interface;

namespace websitebenhvien.Areas.Admin.Controllers
{
    public class SMessagerController : Controller
    {
        private readonly ISamplemessager _samplemessager;

        public SMessagerController(ISamplemessager samplemessager)
        {
            _samplemessager = samplemessager;
        }
        [HttpPost("/api/Them-tin-nhan-mau")]
        [Authorize]
        public async Task<IActionResult> AddSpmess(SampleVM sample)
        {
            try
            {
       
      
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                    return Json(new { status = false, message = string.Join(", ", errors), errors });
                }
                if(sample.SamplesButton != null)
                {
                    sample.ButtonSamples = JsonConvert.DeserializeObject<List<ButtonsampleVM>>(sample.SamplesButton);
                }
                var data = await _samplemessager.AddSamplemessager(sample);

                if (data)
                {
                    return Json(new { status = true, message = "Thêm tin nhắn mẫu thành công" });
                }
                else
                {
                    return Json(new { status = false, message = "Thêm tin nhắn mẫu thất bại" });
                }

            }
            catch(Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }
        }
        [HttpPost("/api/lay-tin-nhan-mau")]
        [Authorize]
        public async Task<IActionResult> GetSpmess(int page,int pageSize)
        {
            try
            {
                var data= await _samplemessager.Getlistsp(page,pageSize);
                if (data.ds != null)
                {
                    return Json(new { status = true, data=data.ds, total = data.TotalPages,page=page });
                }
                else
                {
                    return Json(new { status = false, message = "Không có dữ liệu" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }
        }
        [HttpPost("/api/lay-tin-nhan-mau-theo-id")]
        [Authorize]
        public async Task<IActionResult> GetSpmessById(int id)
        {
            try
            {
                var data = await _samplemessager.GetSamplemessager(id);
                if (data != null)
                {
                    return Json(new { status = true, data = data });
                }
                else
                {
                    return Json(new { status = false, message = "Không có dữ liệu" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }
        }
        [HttpPost("/api/xoa-tin-nhan-mau")]
        [Authorize]
        public async Task<IActionResult> DeleteSpmess(int id)
        {
            try
            {
                var data = await _samplemessager.DeleteSamplemessager(id);
                if (data)
                {
                    return Json(new { status = true, message = "Xóa tin nhắn mẫu thành công" });
                }
                else
                {
                    return Json(new { status = false, message = "Xóa tin nhắn mẫu thất bại" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }
        }
        [HttpPost("/api/cap-nhat-trang-thai")]
        [Authorize]
        public async Task<IActionResult> UpdateStatus(int id)
        {
            try
            {
                var data = await _samplemessager.UpdateStatus(id);
                if (data)
                {
                    return Json(new { status = true, message = "Cập nhật trạng thái thành công" });
                }
                else
                {
                    return Json(new { status = false, message = "Cập nhật trạng thái thất bại" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }
        }
    }
}
