﻿using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
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
                    // Lấy roles và permissions
                    var userRole = await _userManager.GetRolesAsync(username);
                    var permissionUser = await _context.PermissionUser
                        .Include(x => x.Permissions)
                        .Where(p => p.id_user == username.Id)
                        .ToListAsync();

                    // Tạo danh sách claims
                    var authClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, username.FullName),
                        new Claim(ClaimTypes.NameIdentifier, username.Id),
                    };

                    // Thêm role claims
                    foreach (var role in userRole)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, role));
                    }

                    // Thêm permission claims
                    foreach (var permission in permissionUser)
                    {
                        if (permission.Permissions != null && !string.IsNullOrEmpty(permission.Permissions.Title_permision))
                        {
                            Console.WriteLine($"Adding permission: {permission.Permissions.Title_permision}"); // Log để debug
                            authClaims.Add(new Claim("Permission", permission.Permissions.Title_permision));
                        }
                    }

                    // Tạo identity và principal
                    var claimsIdentity = new ClaimsIdentity(authClaims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                    // Sign in với claims mới
                    await _httpContextAccessor.HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        claimsPrincipal,
                        new AuthenticationProperties
                        {
                            IsPersistent = true,
                            ExpiresUtc = DateTime.UtcNow.AddHours(24)
                        });

                    // Log để debug
                    var currentClaims = claimsPrincipal.Claims.Select(c => $"{c.Type}: {c.Value}");
                    Console.WriteLine("Current claims after login: " + string.Join(", ", currentClaims));

                    status.status = 1;
                    status.messager = "Đăng nhập thành công";
                    return status;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Login error: {ex.Message}"); // Log lỗi
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
        // phần quyền
        public async Task<List<IdentityRole>> GetRoles()
        {
            try
            {
                return await _roleManager.Roles.ToListAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Status> AddRole(string roleName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(roleName))
                {
                    return new Status { status = 0, messager = "Tên vai trò không được để trống" };
                }

                var existingRole = await _roleManager.FindByNameAsync(roleName);
                if (existingRole != null)
                {
                    return new Status { status = 0, messager = "Vai trò đã tồn tại" };
                }

                var newRole = new IdentityRole(roleName);
                var result = await _roleManager.CreateAsync(newRole);

                if (result.Succeeded)
                {
                    return new Status { status = 1, messager = "Thêm vai trò thành công" };
                }
                else
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    return new Status { status = 0, messager = $"Lỗi: {errors}" };
                }
            }
            catch (Exception ex)
            {
                return new Status { status = 0, messager = $"Lỗi ngoại lệ: {ex.Message}" };
            }
        }

        public async Task<bool> DelRole(string id)
        {
            try
            {
                // Tìm vai trò theo ID
                var role = await _roleManager.FindByIdAsync(id);
                if (role == null)
                {
                    // Vai trò không tồn tại
                    return false;
                }

                // Xóa vai trò
                var result = await _roleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    // Xóa thành công
                    return true;
                }
                else
                {
                    // Xóa thất bại, có thể ghi log chi tiết lỗi từ result.Errors
                    return false;
                }
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ, ghi log nếu cần
                return false;
            }
        }

        public async Task<List<PemissionUserVM>> GetPermissonUser()
        {
            try
            {
                var data = await _context.ApplicationUser
            .Select(user => new PemissionUserVM
             {
                 Id_user = user.Id,
                  Fullname = user.FullName,
                ds = _context.PermissionUser
                 . Where(p => p.id_user == user.Id)
                    .ToList() // Chuyển đổi danh sách đồng bộ tại đây
              })
               .ToListAsync();


                return data;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> AddPeremissionUser(PermissionUserVM pemissionUser)
        {
            try
            {
                if (pemissionUser.id_Permission == 0)
                {
                    Guid guid = Guid.NewGuid();
                    int randomId = Math.Abs(BitConverter.ToInt32(guid.ToByteArray(), 0)) % 10000;
                    var existingPermissionUser = await _context.PermissionUser
                        .FirstOrDefaultAsync(p => p.id_Permission == pemissionUser.id_Permission );

                    if (existingPermissionUser != null)
                    {
                        return false; // PermissionUser already exists
                    }

                    var permissionUser = new PermissionUser
                    {
                        id_Permission = randomId,
                        id_PermissionUser = (int)pemissionUser.id_PermissionUser,
                        id_user = pemissionUser.id_user
                    };
                    _context.PermissionUser.Add(permissionUser);
                    await _context.SaveChangesAsync();
                    return true;
                }
                else
                {
                    var permissionUser = await _context.PermissionUser
                        .FirstOrDefaultAsync(p => p.id_PermissionUser == pemissionUser.id_PermissionUser);

                    if (permissionUser == null)
                    {
                        return false;
                    }

                    _context.PermissionUser.Remove(permissionUser);
                    await _context.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception ex)
            {
                // Log the exception (ex) for further analysis
                return false;
            }
        }

    }
}
