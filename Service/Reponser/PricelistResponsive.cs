using AutoMapper;
using Microsoft.EntityFrameworkCore;
using websitebenhvien.Data;
using websitebenhvien.Migrations;
using websitebenhvien.Models.Enitity;
using websitebenhvien.Models.EnitityVM;
using websitebenhvien.Service.Interface;

namespace websitebenhvien.Service.Reponser
{
    public class PricelistResponsive : IPricelist
    {
        private readonly MyDbcontext _dbcontext;
        private readonly IMapper _mapper;

        public PricelistResponsive(MyDbcontext dbcontext,IMapper mapper)
        {
            _dbcontext = dbcontext;
            _mapper = mapper;

        }
        public async Task<(string, bool)> AddPriceOrUpdate(PricelistVM pricelistVM)
        {
            try
            {
                if(pricelistVM == null)
                {
                    return ("không có dữ liệu",false);

                }
                if (pricelistVM.Id_pricelist == null || pricelistVM.Id_pricelist == 0)
                {
                    //thêm mới
                    var pricelist = _mapper.Map<PricelistVM, Pricelist>(pricelistVM);
                    await _dbcontext.Pricelists.AddAsync(pricelist);
                    await _dbcontext.SaveChangesAsync();
                    return ("Thêm mới thành công", true);

                }
                else
                {
                    // cập nhật: fetch tracked entity and map incoming values onto it to avoid duplicate tracking
                    var existingPricelist = await _dbcontext.Pricelists.FindAsync(pricelistVM.Id_pricelist);
                    if (existingPricelist == null)
                    {
                        return ("Không tìm thấy bảng giá để cập nhật", false);
                    }

                    // Map values from VM into the tracked entity instance
                    _mapper.Map(pricelistVM, existingPricelist);

                    // existingPricelist is already tracked by the context, just save changes
                    await _dbcontext.SaveChangesAsync();
                    return ("Cập nhật thành công", true);
                }
                return ("Thêm thất bại", false);
            }
            catch (Exception ex)
            {
                return ($"Lỗi: {ex.Message}", false);

            }
        }

        public async Task<(string, bool)> DeletePricelist(int id)
        {
            try
            {
                var data = await _dbcontext.Pricelists.FindAsync(id);
                if (data != null)
                {
                    _dbcontext.Pricelists.Remove(data);
                    await _dbcontext.SaveChangesAsync();
                    return ("Xóa bảng giá thành công", true);
                }
                return ("Không tìm thấy bảng giá để xóa", false);

            }
            catch (Exception ex)
            {
                return ($"Lỗi: {ex.Message}", false);

            }
        }

        public async Task<PricelistVM> GetPricelistById(int id)
        {
            try
            {
                var data = _dbcontext.Pricelists.FirstOrDefault(p => p.Id_pricelist == id);
                if (data != null)
                {
                    var pricelistVM = _mapper.Map<Pricelist, PricelistVM>(data);
                    return pricelistVM;
                }
                return null;



            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<(int Totalpages, List<PricelistVM> ds)> ListPricelist(string search, int page, int pagesize)
        {
            try
            {
                var query = _dbcontext.Pricelists.AsQueryable();
                if (!string.IsNullOrEmpty(search))
                {
                    query = query.Where(p => p.Title.Contains(search));
                }
                var totalItems = query.Count();
                var totalPages = (int)Math.Ceiling((double)totalItems / pagesize);
                var pricelists = query
                    .OrderByDescending(p => p.Created_At)
                    .Skip((page - 1) * pagesize)
                    .Take(pagesize)
                    .Select(p => new PricelistVM { Id_pricelist = p.Id_pricelist, Title = p.Title, IsActive = p.IsActive, Created_At = p.Created_At })
                    .AsNoTracking()
                    .ToList();
                return (totalPages, pricelists);

            }
            catch (Exception ex)
            {
                return (0, null);
            }
        }

        public Task<List<PricelistVM>> ListPricelistVM()
        {
            try
            {
                var data = _dbcontext.Pricelists
                    .Where(p => p.IsActive==true)
                    .OrderByDescending(p => p.Created_At)
                    .Select(p => new PricelistVM
                    {
                        Id_pricelist = p.Id_pricelist,
                        Title = p.Title,
                        Description = p.Description,
                    })
                    .AsNoTracking()
                    .ToListAsync();
                return data;
            }
            catch (Exception ex)
            {
                return null;

            }
        }
        public async Task<(string, bool)> UpdateStatusPricelist(int id)
        {
            try
            {
                var data = await _dbcontext.Pricelists.FindAsync(id);
                if (data != null)
                {
                    data.IsActive = !data.IsActive;
                    _dbcontext.Pricelists.Update(data);
                    await _dbcontext.SaveChangesAsync();
                    return ("Cập nhật trạng thái thành công", true);
                }
                return ("Không tìm thấy bảng giá để cập nhật trạng thái", false);

            }
            catch (Exception ex)
            {
                return ($"Lỗi: {ex.Message}", false);
            }
        }
    }
}
