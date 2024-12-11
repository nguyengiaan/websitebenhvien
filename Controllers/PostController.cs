using Microsoft.AspNetCore.Mvc;
using websitebenhvien.Data;
using websitebenhvien.Models.Enitity;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using websitebenhvien.Service.Interface;

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
    }
}
