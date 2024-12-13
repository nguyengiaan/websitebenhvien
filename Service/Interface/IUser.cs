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

        // chat


    }
}
