using Microsoft.AspNetCore.Mvc;
using websitebenhvien.Data;
using websitebenhvien.Models.Enitity;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using websitebenhvien.Service.Interface;
using websitebenhvien.Models.EnitityVM;

namespace websitebenhvien.Controllers
{
    public class PostController : Controller
    {
        private readonly IAllinone _allinone;
        private readonly IPost _post;
        public PostController(IAllinone allinone,IPost post)
        {
            _allinone = allinone;
            _post = post;
        }

        [HttpGet]
        public async Task<IActionResult> CatogeryPost(string Catogery, int page , int pagesize )
        {
            try
            {
                var data = await _allinone.ListCategorypost(page, pagesize, Catogery);
                if(data.ds == null)
                {
                    return NotFound();
                }
                return Json(new {
                    ds = data.ds,
                    totalpages = data.totalpages,
                    currentpage = page,
                    pagesize = pagesize
                });
            }
            catch(Exception ex)
            {
                return BadRequest(new { message = "Có lỗi xảy ra" });
            }
        }
        [HttpPost]
        public async Task<IActionResult> PostDetail(string alias_url)
        {
            try
            {
                var data= await _post.GetNewsById(alias_url);
                      if(data != null)
                     {
                     return Json(new {status = true,data = data});
                    }
             return Json(new { status = false,message = "Không tìm thấy bài viết"});

            }catch(Exception ex)
            {
                return Json(new {
                status = false,
                message = ex.Message
            });


            }
        
        }
        // lấy danh sách bài viết theo danh mục
        [HttpPost]
        public async Task<IActionResult> GetNewsByCategory(string alias_url)
        {
            try
            {
                var data = await _post.GetNewsByCategory(alias_url);
                if(data != null)
                {
                    return Json(new {status = true,data = data});
                }
                return Json(new {status = false,message = "Không tìm thấy bài viết"});

            }catch(Exception ex)
            {
                return Json(new {
                    status = false,
                    message = ex.Message
                });
            }
        }
        [HttpPost("/api/ungtuyencv")]
        public async Task<IActionResult> SubmitRecruitment(RecruitmentVM recruitment)
        {
            try
            {

                 if (!ModelState.IsValid)
                {
                    // Trả về thông báo lỗi từ ModelState
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                    return Json(new { status = false, message = string.Join(", ", errors), errors });
                }
                var data = await _post.SubmitRecruitment(recruitment);
                if(data)
                {
                    return Json(new { status = true,message = "Đăng ký ứng tuyển thành công" });
                }
                return Json(new { status = false,message = "Đăng ký ứng tuyển thất bại" });
            }catch(Exception ex)
            {
                return Json(new { status = false,message = ex.Message });
            }
        }
        [HttpGet("/api/thongbao")]
        public async Task<IActionResult> GetNotification()
        {
            try
            {
                var data = await _post.GetNotification();
                return Json(new { status = true,data = data });

            }catch(Exception ex)
            {
                return Json(new { status = false,message = ex.Message });
            }
        }
        [HttpGet("/api/xemthongbao")]
        public async Task<IActionResult> ViewNotification()
        {
            var data = await _post.ViewRecruitment();
            return Json(new { status = true,message = "Xem thông báo thành công" });
        }
        [HttpGet("/api/tuyendung")]
        public async Task<IActionResult> GetRecruitment(int page, int pageSize)
        {
            try{
                var data = await _post.GetRecruitment(page, pageSize);
                return Json(new { status = true,data = data.ds ,totalPages = data.total,page = page});

            }catch(Exception ex)
            {
                return Json(new { status = false,message = ex.Message });
            }
        }
        [HttpDelete("/api/xoatuyendung")]
        public async Task<IActionResult> DeleteRecruitment(string id)
        {
            try
            {
                var data = await _post.DelRecruitment(id);
                if(data)
                {
                    return Json(new { status = true,message = "Xóa thành công" });
                }
                return Json(new { status = false,message = "Xóa thất bại" });
            }catch(Exception ex)
            {
                return Json(new { status = false,message = ex.Message });
            }
        }
       
       // chat khách hàng
       [HttpPost("/api/gui-cho-admin")]
       public async Task<IActionResult> ChatCustomer(string message)
       {
            try
            {
                var data = await _post.Chatcustomer(message);
                return Json(new { status = true });
            }catch(Exception ex)
            {
                return Json(new { status = false,message = ex.Message });
            }
       }
    }
}
