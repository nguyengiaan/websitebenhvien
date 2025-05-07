using Microsoft.AspNetCore.Mvc;
using websitebenhvien.Service.Interface;

namespace websitebenhvien.ViewComponents
{
    public class Danhmucbaiviet : ViewComponent
    {
        private readonly IAllinone _allinone;

        public Danhmucbaiviet(IAllinone allinone)

        {
            _allinone= allinone; 
        }
      public async Task<IViewComponentResult> InvokeAsync()
        {
    try
    {
        // Lấy dữ liệu từ service
        var data = await _allinone.ListCatogery();

        // Kiểm tra nếu dữ liệu null hoặc rỗng
        if (data == null || !data.Any())
        {
            // Trả về View lỗi hoặc View thông báo không có dữ liệu
            return View( "Không có danh mục bài viết nào để hiển thị.");
        }

        // Trả về View với dữ liệu
        return View(data);
    }
    catch (Exception ex)
    {
        // Trả về View lỗi với thông báo lỗi
        return View("Error", ex.Message);
    }
}
    }
}