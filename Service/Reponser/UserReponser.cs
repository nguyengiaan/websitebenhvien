
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using websitebenhvien.Data;
using websitebenhvien.Helper;
using websitebenhvien.Models.Enitity;
using websitebenhvien.Models.EnitityVM;
using websitebenhvien.Service.Interface;

namespace websitebenhvien.Service.Reponser
{
    public class UserReponser : IUser
    {
        private readonly MyDbcontext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly Hubnot _hubnot;

        public UserReponser(Hubnot hubnot,MyDbcontext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
            _hubnot = hubnot;
        }

     

        public async Task<bool> DeleteUser(string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    return false;
                }
                await _userManager.DeleteAsync(user);
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<List<RegisteruserVM>> GetRegisterUsers()
        {
            try
            {
                var users = await _context.ApplicationUser.ToListAsync(); // Lấy danh sách user trước
                var data = new List<RegisteruserVM>();

                foreach (var user in users)
                {
                    var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault(); // Gọi bất đồng bộ
                    data.Add(new RegisteruserVM
                    {
                        Id = user.Id,
                        Username = user.UserName,
                        FullName = user.FullName,
                        Role = role,
                        Password = user.PasswordHash,
                    });
                }

                return data;
            }
            catch (Exception ex)
            {
                // Ghi log lỗi nếu cần
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<Status> Login(LoginVM user)
        {
            try
            {
                var status = new Status();
                var username = await _userManager.FindByNameAsync(user.Username);

                if (username == null)
                {
                    status.status = 0;
                    status.messager = "Sai tên người dùng";
                    return status;
                }
                if (!await _userManager.CheckPasswordAsync(username, user.Password))
                {
                    status.status = 0;
                    status.messager = "Sai mật khẩu";
                    return status;
                }
                var signinResult = await _signInManager.PasswordSignInAsync(user.Username, user.Password, false, false);
                if (signinResult.Succeeded)
                {
                    var userRole = await _userManager.GetRolesAsync(username);
                    var authClaims = new List<Claim>
                   {
                       new Claim(ClaimTypes.Name, username.FullName),
                       new Claim(ClaimTypes.NameIdentifier, username.Id),

                   };
                    foreach (var userRoles in userRole)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, userRoles));
                    }
                    status.status = 1;
                    status.messager = "Đăng nhập thành công";
                    return status;
                }

            }
            catch (Exception ex)
            {
                return new Status { status = 0, messager = ex.Message };
            }
            return new Status { status = 0, messager = "Đăng nhập thất bại" };
        }

        public async Task<Status> RegisterUser(RegisteruserVM registeruser)
        {
            try
            {
                var status = new Status();

                // Kiểm tra xem user đã tồn tại chưa
            
                
                if (registeruser.Id != null)
                {
                        var existingUser = await _context.ApplicationUser.FirstOrDefaultAsync(u => u.Id == registeruser.Id);
                    // Cập nhật thông tin user nếu đã tồn tại
                    existingUser.FullName = registeruser.FullName;

                    
                    if(!string.IsNullOrEmpty(registeruser.Password) && registeruser.Password!= existingUser.PasswordHash)
                    {
                        existingUser.PasswordHash = _userManager.PasswordHasher.HashPassword(existingUser, registeruser.Password);
                    }
                    var updateResult = await _userManager.UpdateAsync(existingUser);
                    if (!updateResult.Succeeded)
                    {
                        status.status = 0;
                        status.messager = "Cập nhật thất bại";
                        return status;
                    }

                    // Cập nhật role nếu khác với role hiện tại
                    var currentRoles = await _userManager.GetRolesAsync(existingUser);
                    if (!currentRoles.Contains(registeruser.Role))
                    {
                        await _userManager.RemoveFromRolesAsync(existingUser, currentRoles);
                        await _userManager.AddToRoleAsync(existingUser, registeruser.Role);
                    }

                    status.status = 1;
                    status.messager = "Cập nhật thành công";
                    return status;
                }

                // Tạo user mới nếu chưa tồn tại
                var newUser = new ApplicationUser
                {
                    UserName = registeruser.Username,
                    Email = registeruser.Password,
                    FullName = registeruser.FullName
                };

                var result = await _userManager.CreateAsync(newUser, registeruser.Password);
                if (!result.Succeeded)
                {
                    status.status = 0;
                    status.messager = "Đăng ký thất bại";
                    return status;
                }

                if (!await _roleManager.RoleExistsAsync(registeruser.Role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(registeruser.Role));
                }
                if (await _roleManager.RoleExistsAsync(registeruser.Role))
                {
                    await _userManager.AddToRoleAsync(newUser, registeruser.Role);
                }

                status.status = 1;
                status.messager = "Đăng ký thành công";
                return status;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        
     
    
    }
}
