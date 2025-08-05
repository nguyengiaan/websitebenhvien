using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using websitebenhvien.Models.Enitity;
using websitebenhvien.Models.EnitityVM;
using websitebenhvien.Service.Interface;

namespace websitebenhvien.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class MenuadminuserController : Controller
    {
        private readonly IMenuadminuser _adminmenuuser;
        
        public MenuadminuserController(IMenuadminuser adminmenuuser)
        {
            _adminmenuuser = adminmenuuser;
        }
       
        [Route("/trang-quan-tri/quan-li-menu-user")]
        public async Task<IActionResult> Index(string id, string username)
        {
            try
            {
                List<MenuAdminVM> result;
                
                if (!string.IsNullOrEmpty(id))
                {
                    // Lấy menu của user cụ thể với trạng thái đã chọn
                    result = await _adminmenuuser.GetMenusByUserId(id);
                }
                else
                {
                    // Lấy tất cả menu
                    result = await _adminmenuuser.Listmenuadmin();
                }

                ViewBag.Id = id;
                ViewBag.Username = username;
                
                return View(result);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Có lỗi xảy ra khi lấy danh sách menu người dùng: " + ex.Message;
                return View(new List<MenuAdminVM>());
            }
        }

        // API: Lấy danh sách menu với trạng thái của user
        [HttpGet]
        [Route("/api/get-user-menus")]
        public async Task<IActionResult> GetUserMenus(string userId)
        {
            try
            {
                var allMenus = await _adminmenuuser.Listmenuadmin();
                var userMenus = await _adminmenuuser.GetMenusByUserId(userId);
                
                // Tạo response với thông tin menu và trạng thái
                var response = allMenus.Select(menu => new
                {
                    id = menu.id,
                    title = menu.Title,
                    icon = menu.Icon,
                    url = menu.Url,
                    isSelected = false // Sẽ được cập nhật dựa trên userMenus
                }).ToList();

                return Json(new { status = true, data = response });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = "Lỗi khi lấy danh sách menu: " + ex.Message });
            }
        }

        // API: Lưu menu cho user
        [HttpPost]
        [Route("/api/save-user-menu-admin")]
        public async Task<IActionResult> SaveUserMenuAdmin(string userId, List<int> menuIds)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    return Json(new { status = false, message = "User ID không hợp lệ" });
                }

                if (menuIds == null)
                {
                    menuIds = new List<int>();
                }

                var result = await _adminmenuuser.UpdateUserMenus(userId, menuIds);
                
                if (result)
                {
                    return Json(new { 
                        status = true, 
                        message = $"Đã cập nhật {menuIds.Count} menu cho người dùng" 
                    });
                }
                else
                {
                    return Json(new { status = false, message = "Không thể cập nhật menu cho người dùng" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = "Lỗi khi lưu menu: " + ex.Message });
            }
        }

        // API: Xóa một menu khỏi user
        [HttpPost]
        [Route("/api/remove-user-menu")]
        public async Task<IActionResult> RemoveUserMenu(string userId, int menuId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    return Json(new { status = false, message = "User ID không hợp lệ" });
                }

                var result = await _adminmenuuser.RemoveMenuFromUser(userId, menuId);
                
                if (result)
                {
                    return Json(new { 
                        status = true, 
                        message = "Đã xóa menu khỏi người dùng" 
                    });
                }
                else
                {
                    return Json(new { status = false, message = "Không thể xóa menu" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = "Lỗi khi xóa menu: " + ex.Message });
            }
        }

        // API: Xóa tất cả menu của user
        [HttpPost]
        [Route("/api/delete-all-user-menus")]
        public async Task<IActionResult> DeleteAllUserMenus(string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    return Json(new { status = false, message = "User ID không hợp lệ" });
                }

                var result = await _adminmenuuser.DeleteAllUserMenus(userId);
                
                if (result)
                {
                    return Json(new { 
                        status = true, 
                        message = "Đã xóa tất cả menu của người dùng" 
                    });
                }
                else
                {
                    return Json(new { status = false, message = "Không thể xóa menu" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = "Lỗi khi xóa menu: " + ex.Message });
            }
        }

        // API: Thêm menu cho user
        [HttpPost]
        [Route("/api/assign-menu-to-user")]
        public async Task<IActionResult> AssignMenuToUser(string userId, List<int> menuIds)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    return Json(new { status = false, message = "User ID không hợp lệ" });
                }

                if (menuIds == null || menuIds.Count == 0)
                {
                    return Json(new { status = false, message = "Vui lòng chọn ít nhất một menu" });
                }

                var result = await _adminmenuuser.AssignMenuToUser(userId, menuIds);
                
                if (result)
                {
                    return Json(new { 
                        status = true, 
                        message = $"Đã thêm {menuIds.Count} menu cho người dùng" 
                    });
                }
                else
                {
                    return Json(new { status = false, message = "Không thể thêm menu cho người dùng" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = "Lỗi khi thêm menu: " + ex.Message });
            }
        }
    }
}
