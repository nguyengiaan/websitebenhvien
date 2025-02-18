using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using websitebenhvien.Models.Enitity;
using websitebenhvien.Models.EnitityVM;


namespace websitebenhvien.Service.Interface
{
    public interface IUser
    {
        public Task<Status> RegisterUser(RegisteruserVM registeruser);

        public Task<List<RegisteruserVM>> GetRegisterUsers();

        // xoá user
        public Task<bool> DeleteUser(string id);

        public Task<Status> Login(LoginVM login);

        // Phần quyền
        public Task<List<IdentityRole>> GetRoles();

        public Task<Status> AddRole(string role);

        public Task<Boolean> DelRole(string Id);
        public Task<List<PemissionUserVM>> GetPermissonUser();

        public Task<bool> AddPeremissionUser(PermissionUserVM pemissionUser);

        public Task<List<RolesuserVM>> GetRolesUser(string id);

        public Task<bool> UpdateRolesUser(string id, string idrole);


        public Task<bool> Logout();
        // quản lý email 
        public Task<Models.Enitity.Email> GetEmail();


        public Task<Boolean> Updateemail(int id, string email);





















    }
}
