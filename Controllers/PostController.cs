using Microsoft.AspNetCore.Mvc;
using websitebenhvien.Data;
using websitebenhvien.Models.Enitity;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using websitebenhvien.Service.Interface;
using websitebenhvien.Models.EnitityVM;
using Microsoft.Extensions.Caching.Memory;

namespace websitebenhvien.Controllers
{
    public class PostController : Controller
    {
        private readonly IAllinone _allinone;
        private readonly IPost _post;

        private readonly IMemoryCache _cache;

        public PostController(IAllinone allinone,IPost post,IMemoryCache cache)
        {
            _allinone = allinone;
            _post = post;
            _cache = cache;
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
        // lấy danh sách chat khách hàng 
        [Authorize]
        [HttpGet("/api/lay-danh-sach-chat")]
        public async Task<IActionResult> GetChat()
        {
            try
            {
                var data = await _post.GetChat();
                return Json(new { status = true,data = data });
            }catch(Exception ex)
            {
                return Json(new { status = false,message = ex.Message });
            }
        }
        // hiển thị chi tiet tin nhan 
        [HttpPost("/api/lay-tin-nhan")]
        public async Task<IActionResult> GetChatById(string id)
        {
           try

           {
            var data = await _post.GetChatById(id);
            return Json(new { status = true,data = data });

           }
           catch(Exception ex)
           {
            return Json(new { status = false,message = ex.Message });
           }
        }
        // admin rep tin nhắn khách hàng 
        [Authorize]
        [HttpPost("/api/rep-tin-nhan")]
        public async Task<IActionResult> RepChat(string id,string message)
        {
           try
           {
            var data = await _post.RepChat(id,message);
            if(data)
            {
                return Json(new { status = true,message = "Trả lời thành công" });
            }
            return Json(new { status = false,message = "Trả lời thất bại" });
           }catch(Exception ex)
           {
            return Json(new { status = false,message = ex.Message });
           }
        }
        // xem tin nhắn khách hàng 
        [Authorize]
        [HttpPost("/api/xem-tin-nhan")]
        public async Task<IActionResult> ViewChat(string id)
        {
            try
            {
                var data = await _post.ViewChat(id);
                return Json(new { status = true,message = "Xem tin nhắn thành công" });
            }catch(Exception ex)
            {
                return Json(new { status = false,message = ex.Message });
            }
        }
        // xóa tin nhắn khách hàng 
        [Authorize]
        [HttpPost("/api/xoa-tin-nhan")]
        public async Task<IActionResult> DelChat(string id)
        {
           try
           {
            var data = await _post.DelChat(id);
            if(data)
            {
                return Json(new { status = true,message = "Xóa tin nhắn thành công" });
            }
            return Json(new { status = false,message = "Xóa tin nhắn thất bại" });
           }catch(Exception ex)
           {
            return Json(new { status = false,message = ex.Message });
           }
        }
        // lấy chi tiết chuyên khoa 
        [HttpPost("/api/lay-chi-tiet-chuyen-khoa")]
        public async Task<IActionResult> GetSpecialtyById(string alias_url)
        {
            try
            {
            if (!_cache.TryGetValue(alias_url, out var cachedData))
            {
                var data = await _post.GetSpecialtyById(alias_url);
                if (data.Item1 != null || data.Item2 != null)
                {
                cachedData = new { status = true, data = data.Item1, Doctor = data.Item2 };
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(30));
                _cache.Set(alias_url, cachedData, cacheEntryOptions);
                }
                else
                {
                return Json(new { status = false, message = "Không tìm thấy chuyên khoa" });
                }
            }
            return Json(cachedData);
            }
            catch (Exception ex)
            {
            return Json(new { status = false, message = ex.Message });
            }
        }
        // lấy danh sách bác sĩ theo chuyên khoa
        [HttpPost("/api/lay-chi-tiet-bac-si")]
        public async Task<IActionResult> GetDoctorById(string alias_url)
        {
            try
            {
                var data = await _post.GetDoctorById(alias_url);
                return Json(new { status = true,data = data });
            }catch(Exception ex)
            {
                return Json(new { status = false,message = ex.Message });
            }
        }
        //đăng ký kham benh
     
    }
}
