using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using websitebenhvien.Models.EnitityVM;
using websitebenhvien.Service.Interface;

namespace websitebenhvien.Areas.Admin.Controllers
{
    public class AllinoneController : Controller
    {
        private readonly IAllinone _allinone;

        public AllinoneController(IAllinone allinone)

        {
            _allinone = allinone;
        }
        // controller danh mục sản phẩm
        [Authorize(Roles = "admin,Danhmucsanpham")]
        [HttpPost]
        public async Task<IActionResult> AddCatogery(CategorynewsVM categorynews)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    // Trả về thông báo lỗi từ ModelState
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                    return Json(new { status = false, message = string.Join(", ", errors), errors });
                }
                var data = await _allinone.AddCatogery(categorynews);
                if (data)
                {
                    return Json(new { status = true, message = "Thêm danh mục thành công" });
                }
                return Json(new { status = false, message = "Thêm danh mục thất bại" });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }
        }
        // danh sách danh mục tin tức
        [Authorize(Roles = "admin,Danhmucbaiviet")]
        [HttpGet]
        public async Task<IActionResult> ListCatogeryNews()
        {
            try
            {
                var data = await _allinone.ListCatogery();
                return Json(new { status = true, data });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }
        }
        // xóa danh mục
        [Authorize(Roles = "admin,Danhmucbaiviet")]
        [HttpPost]
        public async Task<IActionResult> DeleteCatogery(string id)
        {
            try
            {
                var data = await _allinone.DeleteCatogery(id);
                return Json(new { status = true, message = "Xóa danh mục thành công" });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }
        }
        //Tin tức
        [Authorize(Roles = "admin,Baiviet")]
        [HttpPost]
        public async Task<IActionResult> AddNews(NewsVM news)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                    return Json(new { status = false, message = string.Join(", ", errors), errors });
                }
                var data = await _allinone.AddNews(news);
                if (data)
                {
                    return Json(new { status = true, message = "Thêm tin tức thành công" });
                }
                return Json(new { status = false, message = "Thêm tin tức thất bại" });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }
        }
        [Authorize(Roles = "admin,Baiviet")]
        [HttpPost]
        public async Task<IActionResult> Listnews(string search,int page,int pagesize)
        {
            try
            {
                var data = await _allinone.Listnews(search,page,pagesize);
                return Json(new { status = true, data=data.ds ,pageindex=page ,totalpage=data.Totalpages});
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }
        }
        [Authorize(Roles = "admin,Baiviet")]
        [HttpPost]
        public async Task<IActionResult> DeleteNews(int  id)
        {
            try
            {
                var data = await _allinone.DeleteNews(id);
                return Json(new { status = true, message = "Xóa tin tức thành công" });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }
        }
         [Authorize(Roles = "admin,Baiviet")]
        [HttpPost]
        public async Task<IActionResult> GetNewsById(int id)
        {
            try
            {
                var data = await _allinone.GetNewsById(id);
                return Json(new { status = true, data });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }
        }
        //Danh mục sản phẩm
          [Authorize(Roles = "admin,Danhmucsanpham")]
        [HttpPost]
        public async Task<IActionResult> AddCatogeryProduct(CategoryproductVM categoryproduct)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                    return Json(new { status = false, message = string.Join(", ", errors), errors });
                }
                var data = await _allinone.AddCatogeryProduct(categoryproduct);
                if (data)
                {
                    return Json(new { status = true, message = "Thêm danh mục sản phẩm thành công" });
                }
                return Json(new { status = false, message = "Thêm danh mục sản phẩm thất bại" });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }
        }
          [Authorize(Roles = "admin,Danhmucsanpham")]
        [HttpGet]
        public async Task<IActionResult> ListCatogeryProduct()
        {
            try
            {
                var data = await _allinone.ListCatogeryProduct();
                return Json(new { status = true, data });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }
        }
                  [Authorize(Roles = "admin,Danhmucsanpham")]
        [HttpPost]
        public async Task<IActionResult> DeleteCatogeryProduct(string id)
        {
            try
            {
                var data = await _allinone.DeleteCatogeryProduct(id);
                return Json(new { status = true, message = "Xóa danh mục sản phẩm thành công" });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }
        }
        [Authorize(Roles = "admin,Danhmucsanpham")]
        [HttpPost]
        public async Task<IActionResult> GetCatogeryProductById(string id)
        {
            try
            {
                var data = await _allinone.GetCatogeryProductById(id);
                return Json(new { status = true, data });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }
        }
        // sản phẩm 
        [Authorize(Roles = "admin,Sanpham")]
        [HttpPost]
        public async Task<IActionResult> AddProduct(ProductVM product)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                    return Json(new { status = false, message = string.Join(", ", errors), errors });
                }
                var data = await _allinone.AddProduct(product);
                if (data)
                {
                    return Json(new { status = true, message = "Thêm sản phẩm thành công" });
                }
                return Json(new { status = false, message = "Thêm sản phẩm thất bại" });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ListProduct()
        {
            try
            {
                var data = await _allinone.ListProduct();
                return Json(new { status = true, data });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }
        }
        [Authorize(Roles = "admin,Sanpham")]
        [HttpPost]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            try
            {
                var data = await _allinone.DeleteProduct(id);
                return Json(new { status = true, message = "Xóa sản phẩm thành công" });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }

        }
        [Authorize(Roles = "admin,Sanpham")]
        [HttpPost]
        public async Task<IActionResult> GetProductById(string id)
        {
            try
            {
                var data = await _allinone.GetProductById(id);
                return Json(new { status = true, data });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }
        }
        [Authorize(Roles = "admin,Sanpham")]
        [HttpPost]
        public async Task<IActionResult> UpdatestatusPro(string id)
        {
            try
            {
                var data = await _allinone.UpdatestatusPro(id);
                return Json(new { status = true, message = "Cập nhật trạng thái thành công" });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Countdashboard()
        {
            var data = await _allinone.Countdashboard();
            return Json(new { status = true, data=data   });
        }
        [Authorize(Roles = "admin,Baiviet")]
        [HttpPost]
        public async Task<IActionResult> UpdateStatusNews(int id)
        {
            try
            {
                var data = await _allinone.UpdateStatusNews(id);
                return Json(new { status = true, message = "Cập nhật trạng thái tin tức thành công" });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }
        }
 
        [HttpGet]
        public async Task<IActionResult> ListService()
        {
            try
            {
                var data = await _allinone.ListService();
                return Json(new { status = true, data });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }
        }
        [HttpGet]
        public async Task<IActionResult> NewsList()
        {
            try
            {
                var data = await _allinone.ListNews();
                return Json(new { status = true, data });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }
        }
        
        [HttpGet]
        public async Task<IActionResult> ListShareCustomer()
        {
            var data = await _allinone.ListShareCustomer();
            return Json(new { status = true, data });
        }
    }
}
