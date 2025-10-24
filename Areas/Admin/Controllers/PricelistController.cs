using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using websitebenhvien.Models.EnitityVM;


namespace websitebenhvien.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin,Quanlybanggia")]
    public class PricelistController : Controller
    {
        private readonly websitebenhvien.Service.Interface.IPricelist _pricelist;

        public PricelistController(websitebenhvien.Service.Interface.IPricelist pricelist)
        {
            _pricelist = pricelist;
        }
        [HttpGet("/trang-quan-tri/quan-ly-bang-gia")]
        public IActionResult Index()
        {
            return View();
        }

    [Authorize(Roles = "admin,Quanlybanggia")]
    [HttpPost("/api-bang-gia-them-cap-nhat")]
    public async Task<IActionResult> AddOrUpdatePricelist(PricelistVM model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                    return Json(new { status = false, message = string.Join("; ", errors) });
                }

                var result = await _pricelist.AddPriceOrUpdate(model);
                return Json(new { status = result.Item2, message = result.Item1 });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }
        }

    [Authorize(Roles = "admin,Quanlybanggia")]
    [HttpPost("/api-bang-gia-danh-sach")]
    public async Task<IActionResult> ListPricelist(string search, int page, int pagesize)
        {
            try
            {
                var data = await _pricelist.ListPricelist(search, page, pagesize);
                return Json(new { status = true, data = data.ds, totalpage = data.Totalpages, pageindex = page });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }
        }

    [Authorize(Roles = "admin,Quanlybanggia")]
    [HttpPost("/api-bang-gia-lay")]
    public async Task<IActionResult> GetPricelistById(int id)
        {
            try
            {
                var data = await _pricelist.GetPricelistById(id);
                if (data != null)
                    return Json(new { status = true, data = data });
                return Json(new { status = false, message = "Không tìm thấy" });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }
        }

    [Authorize(Roles = "admin,Quanlybanggia")]
    [HttpPost("/api-bang-gia-xoa")]
    public async Task<IActionResult> DeletePricelist(int id)
        {
            try
            {
                var data = await _pricelist.DeletePricelist(id);
                return Json(new { status = data.Item2, message = data.Item1 });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }
        }

    [Authorize(Roles = "admin,Quanlybanggia")]
    [HttpPost("/api-bang-gia-chuyen-trang-thai")]
    public async Task<IActionResult> ToggleStatusPricelist(int id)
        {
            try
            {
                var data = await _pricelist.UpdateStatusPricelist(id);
                return Json(new { status = data.Item2, message = data.Item1 });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }
        }



    }
}
