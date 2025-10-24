using websitebenhvien.Models.EnitityVM;

namespace websitebenhvien.Service.Interface
{
    public interface ICustomeguider
    {
        public Task<(string, bool)> AddCustomer(CustomerguideVM customerguide);

        public Task<(int Totalpages, List<CustomerguideVM> ds)> Listnews(string search, int page, int pagesize);

        public Task<bool> DeleteCustomerguide(int id);

        public Task<CustomerguideVM> GetCustomerById(int id);
    }
}
