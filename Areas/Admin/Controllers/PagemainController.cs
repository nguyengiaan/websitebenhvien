using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using websitebenhvien.Models.Enitity;
using websitebenhvien.Models.EnitityVM;
using websitebenhvien.Service.Interface;

namespace websitebenhvien.Areas.Admin.Controllers
{

    public class PagemainController : Controller
    {
        private readonly IPageMain _page;

        public PagemainController(IPageMain page)
        {
            _page = page;
        }

        [HttpPost]
        public async Task<IActionResult> Editheader(HeaderVM header)
        {
            try
            {
                if (header.titlelist != null)
                {
                    var titleHeaders = JsonConvert.DeserializeObject<List<Titleheader>>(header.titlelist);
                    if (titleHeaders != null)
                    {
                        header.titleheader = titleHeaders.Select(th => new TitleheaderVM
                        {
                            Id_titleheader = th.Id_titleheader,
                            title = th.title,
                            link = th.link
                        }).ToList();
                    }
                }

                var data = await _page.Editheader(header);
                if (data)
                {
                    return Json(new { status = 1, message = "Cập nhật thành công" });
                }
                else
                {
                    return Json(new { status = 0, message = "Có lỗi xảy ra" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = 0, message = "Có lỗi xảy ra" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetTitleheader()
        {
            try
            {
                var data = await _page.GetTitleheader();
                return Json(new { success = true, data = data });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra" });
            }
        }
        // trang slide
        [HttpPost]
        public async Task<IActionResult> Addslide(SlideVM slidepage)
        {
            if (!ModelState.IsValid)
            {
                // Trả về thông báo lỗi từ ModelState
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return Json(new { status = 0, message = string.Join(", ", errors), errors });
            }

            try
            {
                var data = await _page.AddSlide(slidepage);
                if (data)
                {
                    return Json(new { status = 1, message = "Thêm thành công" });
                }
                else
                {
                    return Json(new { status = 0, message = "Có lỗi xảy ra" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = 0, message = "Có lỗi xảy ra" });
            }
        }
        // hiển thị slide

        [HttpGet]
        public async Task<IActionResult> GetSlides()
        {
            try
            {
                var data = await _page.GetSlide();
                return Json(new { success = true, data = data });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra" });
            }
        }
        [HttpPost]
        public async Task<IActionResult> DeleteSlide(string id_slidepage)
        {
            try
            {
                var data = await _page.DeleteSlide(id_slidepage);
                if (data)
                {
                    return Json(new { status = 1, message = "Xóa thành công" });
                }
                else
                {
                    return Json(new { status = 0, message = "Có lỗi xảy ra" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra" });
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdateStatus(string id_slidepage)
        {
            try
            {
                var data = await _page.UpdateStatus(id_slidepage);
                if (data)
                {
                    return Json(new { status = 1, message = "Cập nhật thành công" });
                }
                else
                {
                    return Json(new { status = 0, message = "Có lỗi xảy ra" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra" });
            }
        }
        // hiển thị slide chi tiết
        [HttpPost]
        public async Task<IActionResult> GetSlideByTitle(string id_slidepage)
        {
            try
            {
                var data = await _page.GetSlideByTitle(id_slidepage);
                return Json(new { success = true, data = data });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra" });
            }
        }
        // sửa slide
        [HttpPost]
        public async Task<IActionResult> EditSlide(SlideVM slide)
        {
            try
            {
                var data = await _page.EditSlide(slide);
                if (data)
                {
                    return Json(new { status = 1, message = "Cập nhật thành công" });
                }
                else
                {
                    return Json(new { status = 0, message = "Có lỗi xảy ra" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra" });
            }
        }
        //footer 
        [HttpGet]
        public async Task<IActionResult> GetFooter()
        {
            try
            {
                var data = await _page.GetFooter();
                return Json(new { success = true, data = data });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra" });
            }
        }
        [HttpPost]
        public async Task<IActionResult> EditFooter(FooterVM footer)
        {
            try
            {
                var data = await _page.EditFooter(footer);
                if (data)
                {
                    return Json(new { status = 1, message = "Cập nhật thành công" });
                }
                else
                {
                    return Json(new { status = 0, message = "Có lỗi xảy ra" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra" });
            }
        }
        // logo khách hàng
        [HttpPost]
        public async Task<IActionResult> AddLogoCustomer(LogocustomerVM logo)
        {
            if (!ModelState.IsValid)
            {
                // Trả về thông báo lỗi từ ModelState
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return Json(new { status = 0, message = string.Join(", ", errors), errors });
            }
            try
            {
                var data = await _page.AddLogoCustomer(logo);
                if (data)
                {
                    return Json(new { status = 1, message = "Thêm thành công" });
                }
                else
                {
                    return Json(new { status = 0, message = "Có lỗi xảy ra" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = 0, message = "Có lỗi xảy ra" });
            }
        }
        // hiển thị logo khách hàng
        [HttpPost]
        public async Task<IActionResult> Getlistlogo(int page, int pagesize)
        {
            try
            {
                var data = await _page.Listlogo(page, pagesize);
                if (data.ds != null)
                {
                    return Json(new { success = true, data = data.ds, totalpages = data.totalpages, page = page });
                }
                else
                {
                    return Json(new { success = false, message = "Có lỗi xảy ra" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra" });
            }
        }
        [HttpPost]
        public async Task<IActionResult> DeleteLogoCustomer(string id_logocustomer)
        {
            try
            {
                var data = await _page.DeleteLogoCustomer(id_logocustomer);
                if (data)
                {
                    return Json(new { status = 1, message = "Xóa thành công" });
                }
                else
                {
                    return Json(new { status = 0, message = "Có lỗi xảy ra" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra" });
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdateStatuslogo(string id_logocustomer)
        {
            try
            {
                var data = await _page.UpdateStatuslogo(id_logocustomer);
                if (data)
                {
                    return Json(new { status = 1, message = "Cập nhật thành công" });
                }
                else
                {
                    return Json(new { status = 0, message = "Có lỗi xảy ra" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra" });
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdateLogoCustomer(LogocustomerVM logo)
        {
            try
            {
                var data = await _page.UpdatelogoCustomer(logo);
                if (data)
                {
                    return Json(new { status = 1, message = "Cập nhật thành công" });
                }
                else
                {
                    return Json(new { status = 0, message = "Có lỗi xảy ra" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra" });
            }
        }
        // chia sẻ khách hàng 
        [HttpPost]
        public async Task<IActionResult> AddShareCustomer(SharecustomerVM share)
        {
            if (!ModelState.IsValid)
            {
                // Trả về thông báo lỗi từ ModelState
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return Json(new { status = 0, message = string.Join(", ", errors), errors });
            }
            try
            {
                var data = await _page.AddShareCustomer(share);
                if (data)
                {
                    return Json(new { status = 1, message = "Thêm thành công" });
                }
                else
                {
                    return Json(new { status = 0, message = "Có lỗi xảy ra" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = 0, message = "Có lỗi xảy ra" });
            }
        }
        // danh sách khách hàng chia sẻ
        [HttpPost]
        public async Task<IActionResult> ListShare(int page, int pagesize)
        {
            try
            {
                var data = await _page.ListShare(page, pagesize);
                if (data.ds != null)
                {
                    return Json(new { success = true, data = data.ds, totalpages = data.totalpages, page = page });
                }
                else
                {
                    return Json(new { success = false, message = "Có lỗi xảy ra" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra" });
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdateStatusShare(string id_share)
        {
            try
            {
                var data = await _page.UpdateStatusShare(id_share);
                if (data)
                {
                    return Json(new { status = 1, message = "Cập nhật thành công" });
                }
                else
                {
                    return Json(new { status = 0, message = "Có lỗi xảy ra" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra" });
            }
        }
        // xoá khách hàng chia sẻ
        [HttpPost]
        public async Task<IActionResult> DeleteShareCustomer(string id_share)
        {
            try
            {
                var data = await _page.DeleteShareCustomer(id_share);
                if (data)
                {
                    return Json(new { status = 1, message = "Xóa thành công" });
                }
                else
                {
                    return Json(new { status = 0, message = "Có lỗi xảy ra" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra" });
            }
        }
        // update khách hàng chia sẻ
        [HttpPost]
        public async Task<IActionResult> UpdateShareCustomer(SharecustomerVM share)
        {
            try
            {
                var data = await _page.UpdateShareCustomer(share);
                if (data)
                {
                    return Json(new { status = 1, message = "Cập nhật thành công" });
                }
                else
                {
                    return Json(new { status = 0, message = "Có lỗi xảy ra" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra" });
            }
        }
        // thời gian làm việc
        [HttpPost]
        public async Task<IActionResult> AddTimeWork(string content)
        {
            try
            {
                var data = await _page.AddTimeWork(content);
                if (data)
                {
                    return Json(new { status = 1, message = "Cập nhật thành công" });
                }
                else
                {
                    return Json(new { status = 0, message = "Có lỗi xảy ra" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra" });
            }
        }
        [HttpGet]
        public async Task<IActionResult> TimeWork()
        {
            try
            {
                var data = await _page.TimeWork();
                return Json(new { success = true, data = data });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra" });
            }
        }
        //menu trang web
        [HttpPost]
        public async Task<IActionResult> AddMenu(MenuVM menu)
        {
            try
            {
                var data = await _page.AddMenu(menu);
                if (data)
                {
                    return Json(new { status = 1, message = "Thêm thành công" });
                }
                else
                {
                    return Json(new { status = 0, message = "Có lỗi xảy ra" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = 0, message = "Có lỗi xảy ra" });
            }
        }
        [HttpGet]
        public async Task<IActionResult> Menu()
        {
            try
            {
                var data = await _page.Menu();
                return Json(new { success = true, data = data });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra" });
            }
        }
        // menu con
        [HttpPost]

        public async Task<IActionResult> AddSubMenu(SubMenuVM submenu)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    // Trả về thông báo lỗi từ ModelState
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                    return Json(new { success = false, message = string.Join(", ", errors), errors });
                }
                var data = await _page.AddSubMenu(submenu);
                if (data)
                {
                    return Json(new { success = true, message = "Thành công" });
                }
                else
                {
                    return Json(new { success = false, message = "Có lỗi xảy ra" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ListMenu()
        {
            try
            {
                var data = await _page.ListMenu();
                return Json(new { success = true, data = data });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra" });
            }
        }
        [HttpPost]
        public async Task<IActionResult> DeleteMenu(string id_menu)
        {
            try
            {
                var data = await _page.DeleteMenu(id_menu);
                if (data)
                {
                    return Json(new { success = true, message = "Xóa thành công" });
                }
                else
                {
                    return Json(new { success = false, message = "Có lỗi xảy ra" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra" });
            }
        }
        [HttpPost]
        public async Task<IActionResult> DeleteMenuSub(string id_menu)
        {
            try
            {
                var data = await _page.DeleteSubMenu(id_menu);
                if (data)
                {
                    return Json(new { success = true, message = "Xóa thành công" });
                }
                else
                {
                    return Json(new { success = false, message = "Có lỗi xảy ra" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra" });
            }
        }
        //lấy menu id
        [HttpPost]
        public async Task<IActionResult> GetMenuvm(string id_menu)
        {
            try
            {
                var data = await _page.GetMenuvm(id_menu);
                return Json(new { success = true, data = data });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra" });
            }
        }
        [HttpPost]
        public async Task<IActionResult> Updatemenu(MenuVM menu)
        {
            try
            {
                var data = await _page.Updatemenu(menu);
                if (data)
                {
                    return Json(new { success = true, message = "Cập nhật thành công" });
                }
                else
                {
                    return Json(new { success = false, message = "Có lỗi xảy ra" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra" });
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdateStatusMenu(string id)
        {
            try
            {
                var data = await _page.UpdateStatusMenu(id);
                if (data)
                {
                    return Json(new { success = true, message = "Cập nhật thành công" });
                }
                else
                {
                    return Json(new { success = false, message = "Có lỗi xảy ra" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra" });
            }
        }
    }
}
