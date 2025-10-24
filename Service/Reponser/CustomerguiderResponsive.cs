using AutoMapper;
using Microsoft.EntityFrameworkCore;
using websitebenhvien.Data;
using websitebenhvien.Helper;
using websitebenhvien.Models.Enitity;
using websitebenhvien.Models.EnitityVM;
using websitebenhvien.Service.Interface;
using Websitebenhvien.Models.Enitity;

namespace websitebenhvien.Service.Reponser
{
    public class CustomerguiderResponsive : ICustomeguider
    {
        private readonly MyDbcontext _context;
        private readonly IMapper _mapper;
        private readonly Uploadfile _uploadfile;

        public CustomerguiderResponsive(MyDbcontext context,IMapper mapper,Uploadfile uploadfile)
        {
            _context=context;
            _mapper= mapper;
            _uploadfile = uploadfile;

        }
        public async Task<(string, bool)> AddCustomer(CustomerguideVM customerguide)
        {
            try
            {
                Customerguide data;

                // Update
                if (customerguide.Id_Customerguide != null)
                {
                    data = await _context.Customerguides.FindAsync(customerguide.Id_Customerguide);
                    if (data == null)
                        return ("Không tìm thấy bài viết", false);

                    // Map updated fields from VM to entity
                    _mapper.Map(customerguide, data);

                    // Defensive: ensure category exists (prevent FK violation)
                    if (!await _context.Catogeryguiders.AnyAsync(c => c.Id_Catogeryguider == data.Id_Catogeryguider))
                    {
                        return ("Danh mục được chọn không tồn tại.", false);
                    }

                    // Handle file upload
                    if (customerguide.formFile != null)
                    {
                        var result = await _uploadfile.SaveMedia(customerguide.formFile);
                        if (result.Item1 == 1 && data.Url != null)
                        {
                            _uploadfile.DeleteMedia(data.Url);
                            data.Url = result.Item2;
                        }
                    }
                }
                else // Create
                {

                    data = new Customerguide();

                    // Cập nhật thông tin khác
                    data.Title = customerguide.Title;
                    data.Alias_url = customerguide.Alias_url;
                    data.Description = customerguide.Description;
                    data.Id_Catogeryguider = customerguide.Id_Catogeryguider.Value;
                    data.Descriptionshort = customerguide.Descriptionshort;
                    data.Keyword = customerguide.Keyword;
                    data.SchemaMakup = customerguide.SchemaMakup;
                    data.Createat = customerguide.Createat ?? DateTime.UtcNow;

                    // Handle file upload
                    if (customerguide.formFile != null)
                    {
                        var result = await _uploadfile.SaveMedia(customerguide.formFile);
                        if (result.Item1 == 1)
                        {
                            data.Url = result.Item2;
                        }
                    }

                    data.Status = true;
                    await _context.Customerguides.AddAsync(data);

                }

                await _context.SaveChangesAsync();
                return ("thêm tin tức thành công", true);
            }
            catch (Exception ex)
            {
                return (ex.Message, false);
            }
        }


        public async Task<bool> DeleteCustomerguide(int id)
        {
            try
            {
                var data = await _context.Customerguides.FindAsync(id);
                _context.Customerguides.Remove(data);
                _uploadfile.DeleteMedia(data.Url);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<CustomerguideVM> GetCustomerById(int id)
        {
            try
            {
                // Truy vấn dữ liệu với AsNoTracking để cải thiện hiệu năng
                var data = await _context.Customerguides
                    .AsNoTracking()
                    .Where(x => x.Id_Customerguide == id)
                    .Select(x => new CustomerguideVM
                    {
                        Id_Customerguide = x.Id_Customerguide,
                        Title = x.Title,
                        Description = x.Description,
                        Url = x.Url,
                        Alias_url = x.Alias_url,
                        Id_Catogeryguider = x.Id_Catogeryguider,
                        Createat = x.Createat,
                        Descriptionshort = x.Descriptionshort,
                        Keyword = x.Keyword,
                        SchemaMakup = x.SchemaMakup,

                    })
                    .FirstOrDefaultAsync();

                return data;
            }
            catch (Exception ex)
            {
                // Log lỗi (có thể sử dụng thư viện logging như Serilog hoặc NLog)
                Console.WriteLine($"Error in GetNewsById: {ex.Message}");
                return null;
            }
        }

        public async Task<(int Totalpages, List<CustomerguideVM> ds)> Listnews(string search, int page, int pagesize)
        {
            try
            {
                IQueryable<Customerguide> query = _context.Customerguides.AsNoTracking();

                // Nếu có từ khóa tìm kiếm, thêm điều kiện vào query
                if (!string.IsNullOrEmpty(search))
                {
                    query = query.Where(x => x.Title.Contains(search));
                }

                // Tính tổng số bản ghi
                var totalItems = await query.CountAsync();

                // Tính số trang
                var totalPages = (int)Math.Ceiling(totalItems / (double)pagesize);

                // Lấy dữ liệu cần thiết
                var data = await query
                    .OrderByDescending(x => x.Createat)
                .Skip((page - 1) * pagesize)
                    .Take(pagesize)
                    .Select(x => new CustomerguideVM
                    {
                        Id_Customerguide = x.Id_Customerguide,
                        Title = x.Title,
                        Url = x.Url,
                        Alias_url = x.Alias_url,
                        Id_Catogeryguider = x.Id_Catogeryguider,
                        Createat = x.Createat,
                        Status = x.Status
                    })
                    .ToListAsync();

                return (totalPages, data);
            }
            catch (Exception)
            {
                // Log lỗi nếu cần thiết
                return (0, null);
            }
        }
    }
}
