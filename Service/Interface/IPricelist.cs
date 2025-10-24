using websitebenhvien.Models.EnitityVM;

namespace websitebenhvien.Service.Interface
{
    public interface IPricelist
    {
        public Task<(string, bool)> AddPriceOrUpdate(PricelistVM pricelistVM );

        public Task<(int Totalpages, List<PricelistVM> ds)> ListPricelist(string search, int page, int pagesize);

        public Task<(string,bool)> DeletePricelist(int id);

        public Task<PricelistVM> GetPricelistById(int id);

        public Task<(string, bool)> UpdateStatusPricelist(int id);

        public Task<List<PricelistVM> > ListPricelistVM();



    }
}
