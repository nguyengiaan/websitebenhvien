using websitebenhvien.Models.Enitity;
using websitebenhvien.Models.EnitityVM;
namespace websitebenhvien.Service.Interface
{
    public interface IPageMain
    {
        public Task<Boolean> Editheader(HeaderVM header);

        public Task<List<HeaderVM>> GetTitleheader();
        // thêm slide
        public Task<Boolean> AddSlide(SlideVM slidepage);
        // hiển thị slide
        public Task<List<Slidepage>> GetSlide();
        // xoá slide
        public Task<Boolean> DeleteSlide(string id_slidepage);
        // cập nhật trạng thái
        public Task<Boolean> UpdateStatus(string id_slidepage);
        // hiển thị slide theo tiêu đề 
        public Task<Slidepage> GetSlideByTitle(string id_slidepage);
        // sửa slide 
        public Task<Boolean> EditSlide(SlideVM slide);

        //footer
        public Task<Footer> GetFooter();
        public Task<Boolean> EditFooter(FooterVM footer);

        // logo khách hàng 
        public Task<Boolean> AddLogoCustomer(LogocustomerVM logo);

        public Task<(List<Logocustomer> ds, int totalpages)> Listlogo(int page, int pagesize);

        public Task<Boolean> DeleteLogoCustomer(string id_logocustomer);

        public Task<Boolean> UpdateStatuslogo(string id_logocustomer);

        public Task<Boolean> UpdatelogoCustomer(LogocustomerVM logo);

        // chia sẻ khách hàng 
        public Task<Boolean> AddShareCustomer(SharecustomerVM share);
        // danh sách khách hàng chia sẻ
        public Task<(List<Sharecustomer> ds, int totalpages)> ListShare(int page, int pagesize);
        // cập nhật trạng thái
        public Task<Boolean> UpdateStatusShare(string id_share);
        // xoá khách hàng chia sẻ
        public Task<Boolean> DeleteShareCustomer(string id_share);
        // cập nhật khách hàng 
        public Task<Boolean> UpdateShareCustomer(SharecustomerVM share);
        // thời gian làm việc
        public Task<Boolean> AddTimeWork(string content);
        public Task<TimeWork> TimeWork();
        // menu trang web
        public Task<Boolean> AddMenu(MenuVM menu);

        public Task<List<MenuVM>> Menu();

        // lấy danh sách menu 
        public Task<List<ListMenuVM>> ListMenu();

        // menu con 
        public Task<Boolean> AddSubMenu(SubMenuVM submenu);
        // xoá menu cha
        public Task<Boolean> DeleteMenu(string id_menu);
        // xoá menu con
        public Task<Boolean> DeleteSubMenu(string id_submenu);
        // sửa menu con 
        public Task<MenuVM> GetMenuvm(string id_menu);
        // sửa menu cha
        public Task<bool> Updatemenu(MenuVM menu);
        // update status
        public Task<Boolean> UpdateStatusMenu(string id);

    }

}
