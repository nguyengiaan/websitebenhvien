using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using websitebenhvien.Models.EnitityVM;
using websitebenhvien.Service.Interface;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Security.Claims;

namespace websitebenhvien.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        private readonly IUser _user;

        public UserController(IUser user)
        {
            _user = user;
        }
        [HttpPost]
        public  async Task<IActionResult> RegisterUser(RegisteruserVM registeruser)
        {
            try
            {
               
                  if (!ModelState.IsValid)
                    {
                        // Trả về thông báo lỗi từ ModelState
                        var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                        return Json(new { status = false, message = string.Join(", ", errors), errors });
                    }
                var data = await _user.RegisterUser(registeruser);
                if (data.status == 1)
                {
                    return Json(new { status = true, message = data.messager });
                }
                return Json(new { status = false, message = data.messager });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });

            }  
        }
        [HttpGet]
        public async Task<IActionResult> GetRegisterUsers()
        {
            try
            {
                var data = await _user.GetRegisterUsers();
                return Json(new { status = true, data=data });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }
        }
        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            try
            {
                var data = await _user.DeleteUser(id);
                if (data)
                {
                    return Json(new { status = true, message = "Xoá thành công" });
                }
                return Json(new { status = false, message = "Xoá thất bại" });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM login)
        {
            try
            {
                var data = await _user.Login(login);
                if (data.status == 1)
                {
                    return Json(new { status = true, message = data.messager });
                }
                return Json(new { status = false, message = data.messager });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }
        }
        
        [Authorize(Roles = "admin")]
        [HttpGet("/api/ds-phan-quyen")]
        public async Task<IActionResult> GetRoles()
        {
            try
            {
                var data = await _user.GetRoles();
                return Json(new { status = true, data = data });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }
        }
        [Authorize]
        [HttpPost("/api/them-phan-quyen")]
        
        public async Task<IActionResult> AddRole(string role)
        {
            try
            {
                var data = await _user.AddRole(role);
                return Json(new { status = true, message = data.messager });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPost("/api/xoa-quyen")]
   

        public async Task<IActionResult> DelRole(string id)
        {
            try
            {
                var data = await _user.DelRole(id);
                if(data)
                {
                    return Json(new { status = true, message = "Xoá thành công" });
                }
                return Json(new { status = false, message = "Xoá thất bại" });
            }
            catch(Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }
        }
        [Authorize(Roles = "admin")]
        [HttpGet("/api/ds-phan-quyen-chuc-nang")]

        public async Task<IActionResult> Getrole()
        {
            try
            {
                var data = await _user.GetPermissonUser();
                return Json(new { status = true ,data=data});
            
            }
            catch(Exception ex)
            {
                return Json(new { status = false, messager = ex.Message });
            }
        }
        [HttpPost("/api/them-phan-quyen-chuc-nang")]
        [Authorize]
        public async Task<IActionResult> AddPeremissionUser(PermissionUserVM pemissionUser)
        {
            try
            {
                var data = await _user.AddPeremissionUser(pemissionUser);
                if (data)
                {
                    return Json(new { status = true, message = "Thêm thành công" });
                }
                return Json(new { status = false, message = "Thêm thất bại" });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }
        }
        [Authorize(Roles = "admin")]
        [HttpPost("/api/ds-roles-user")]
        public async Task<IActionResult> GetRolesUser(string id)
        {
            try
            {
                var data = await _user.GetRolesUser(id);
                return Json(new { status = true, data = data });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }
        }
        [Authorize(Roles = "admin")]
        [HttpPost("/api/update-roles-user")]
        public async Task<IActionResult> UpdateRolesUser(string id, string idrole)
        {
            try
            {
                var data = await _user.UpdateRolesUser(id, idrole);
                return Json(new { status = true, message = "Cập nhật thành công" });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }
        }
        [Authorize]
        [Route("/trang-quan-tri/dang-xuat")]
        public async Task<IActionResult> Logout()
        {       
            try
            {
                var data = await _user.Logout();
                if(data)
                {
                    return RedirectToAction("Dangnhap", "Trangquantri", new { area = "Admin" });
                }
                return Json(new { status = false, message = "Đăng xuất thất bại" });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }
        }
    }
}
