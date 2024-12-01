
using Microsoft.AspNetCore.Mvc;
using websitebenhvien.Models.EnitityVM;
using websitebenhvien.Service.Interface;

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

    }
}
