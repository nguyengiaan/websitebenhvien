using websitebenhvien.Models.EnitityVM;

namespace websitebenhvien.Service.Interface
{
    public interface ICatogeryguide
    {
        public Task<(string, bool)> AddCatogery(CatogeryguiderVM category);

        public Task<List<CatogeryguiderVM>> ListCatogery();

        public Task<(string,bool)> DeleteCatogery(int id);
    }
}
