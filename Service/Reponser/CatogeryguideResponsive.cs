using AutoMapper;
using Microsoft.EntityFrameworkCore;
using websitebenhvien.Data;
using websitebenhvien.Models.Enitity;
using websitebenhvien.Models.EnitityVM;
using websitebenhvien.Service.Interface;

namespace websitebenhvien.Service.Reponser
{
    public class CatogeryguideResponsive : ICatogeryguide
    {
        private readonly MyDbcontext _context;
        private readonly IMapper _mapper;

        public CatogeryguideResponsive(MyDbcontext context,IMapper mapper)
        {
            _context= context;
            _mapper=mapper;
        }
        public async Task<(string, bool)> AddCatogery(CatogeryguiderVM category)
        {
            
            try
            {
                if (category.Id_Catogeryguider != null)
                {
                    var data1 = await _context.Catogeryguiders.FindAsync(category.Id_Catogeryguider);
                    var map = _mapper.Map(category, data1);
                    await _context.SaveChangesAsync();
                    return ("Cập nhật thành công", true);
                }
                else
                {
                    var data = _mapper.Map<Catogeryguider>(category);
                    _context.Catogeryguiders.Add(data);
                    await _context.SaveChangesAsync();
                    return ("Thêm thành công", true);
                }
                return ("Có lỗi xảy ra khi thêm", false);

            }
            catch (Exception ex)
            {
                return ($"Có lỗi xảy ra: {ex.Message}", false);
            }
          
        }

        public async Task<(string, bool)> DeleteCatogery(int id)
        {
            try
            {
                var catogery = await _context.Catogeryguiders.FindAsync(id);
                if (catogery == null)
                {
                    return ("Không tìm thấy danh mục để xóa", false);
                }

                _context.Catogeryguiders.Remove(catogery);
                await _context.SaveChangesAsync();
                return ("Xóa thành công", true);

            }
            catch (Exception ex)
            {
                return ($"Lỗi: {ex.Message}", false);
            }
        }




        public async Task<List<CatogeryguiderVM>> ListCatogery()
        {
            try
            {
                var data = await _context.Catogeryguiders.ToListAsync();
                var map = _mapper.Map<List<CatogeryguiderVM>>(data);
                return map;


            }
            catch (Exception ex)
            {
                return new List<CatogeryguiderVM>();
            }
        }
    }
}
