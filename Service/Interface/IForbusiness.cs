using websitebenhvien.Models.Enitity;

namespace websitebenhvien.Service.Interface
{
    public interface IForbusiness
    {
        public Task<Boolean> Addbusiness(Forbusiness forbusiness);

        public Task<(int Totalpage,List<Forbusiness> list)> Getbusiness(string search,int page, int pagesize);

        public Task<Forbusiness> Getbusinessbyid(int id);

        public Task<Boolean> Deletebsn(int id);

        public Task<Boolean> Updatebusiness(int id);

        public Task<List<Forbusiness>> Getbusinesstrue();






    }
}
