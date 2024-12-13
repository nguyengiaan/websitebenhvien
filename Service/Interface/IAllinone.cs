using websitebenhvien.Models.EnitityVM;

namespace websitebenhvien.Service.Interface
{
    public interface IAllinone
    {
        //interface danh mục sản phẩm 
        public Task<bool> AddCatogery (CategorynewsVM category);

        public Task<List<CategorynewsVM>> ListCatogery();

        public Task<bool>DeleteCatogery(string id);
        //Thêm tin tức
        public Task<bool> AddNews(NewsVM news);

        public Task<List<NewsVM>>Listnews();

        public Task<bool> DeleteNews(string id);

        public Task<NewsVM> GetNewsById(string id);
        // danh mục sản phẩm 
        public Task<bool> AddCatogeryProduct(CategoryproductVM category);
        public Task<List<CategoryproductVM>> ListCatogeryProduct();
        public Task<bool> DeleteCatogeryProduct(string id);
        public Task<CategoryproductVM> GetCatogeryProductById(string id);
        // sản phẩm 
        public Task<bool> AddProduct(ProductVM product);
        public Task<List<ProductVM>> ListProduct();
        public Task<bool> DeleteProduct(string id);
        public Task<ProductVM> GetProductById(string id);

        public Task<bool> UpdatestatusPro(string id);

        public Task<CountdashboardVM> Countdashboard();
        public Task<Boolean> UpdateStatusNews(string id);

        // dịch vụ nổi bật
        public Task<List<NewsVM>> ListService();
        // tin tức nổi bật
        public Task<List<NewsVM>> ListNews();

        // list logo khách hàng
        public Task<List<LogocustomerVM>> ListShareCustomer();

        // danh mục bài viết
        public Task<(List<NewsVM> ds, int totalpages)> ListCategorypost(int page, int pagesize,string Catogery);

    }
}
