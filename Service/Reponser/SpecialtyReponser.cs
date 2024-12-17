using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using websitebenhvien.Data;
using websitebenhvien.Migrations;
using websitebenhvien.Models.Enitity;
using websitebenhvien.Models.EnitityVM;
using websitebenhvien.Service.Interface;
using Specialty = websitebenhvien.Models.Enitity.Specialty;

namespace websitebenhvien.Service.Reponser
{
    public class SpecialtyReponser : ISpecialty
    {
        private readonly MyDbcontext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public SpecialtyReponser(MyDbcontext context, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _hostingEnvironment = hostingEnvironment;
             
        }
        public async Task<bool> AddSpecialty(SpecialtyVM specialty)
        {
            try
            {
                // Tìm specialty đã tồn tại
                var data = specialty.Id_Specialty != null
                    ? await _context.Specialties.FindAsync(specialty.Id_Specialty)
                    : null;

                // Nếu đã tồn tại, cập nhật dữ liệu
                if (data != null)
                {
                    UpdateSpecialtyData(data, specialty);

                    // Xử lý hình ảnh nếu có
                    if (specialty.formFile != null)
                    {
                        var result = SaveMedia(specialty.formFile);
                        if (result.Item1 == 1)
                        {
                            DeleteMedia(data.Thumnail);
                            data.Thumnail = result.Item2;
                        }
                    }

                    await _context.SaveChangesAsync();
                    return true;
                }
                else // Nếu chưa tồn tại, thêm mới
                {
                    var newSpecialty = new Specialty();
                    UpdateSpecialtyData(newSpecialty, specialty);

                    // Xử lý hình ảnh nếu có
                    if (specialty.formFile != null)
                    {
                        var result = SaveMedia(specialty.formFile);
                        if (result.Item1 == 1)
                        {
                            newSpecialty.Thumnail = result.Item2;
                        }
                    }

                    await _context.Specialties.AddAsync(newSpecialty);
                    await _context.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception e)
            {
                // Log lỗi (nếu cần)
                return false;
            }
        }

        // Phương thức dùng chung để cập nhật dữ liệu
        private void UpdateSpecialtyData(Specialty target, SpecialtyVM source)
        {
            target.Title = source.Title;
            target.Introduction = source.Introduction;
            target.Machine = source.Machine;
            target.Method = source.Method;
            target.Service = source.Service;
            target.Achievement = source.Achievement;
            target.Alias_url = source.Alias_url;
        }


        public Task<bool> DeleteSpecialty(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<(List<SpecialtyVM> ds, int TotalPages)> GetSpecialty(int page, int pageSize)
        {
            try
            {
                 var totalItems = await _context.Specialties.CountAsync(); 
                 var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
                 var query = await _context.Specialties
                 .Select(x=>new SpecialtyVM{
                    Id_Specialty = x.Id_Specialty,
                    Title = x.Title,
                    Thumnail = x.Thumnail,
                    Alias_url = x.Alias_url,
                 }).OrderByDescending(x=>x.Id_Specialty).Skip((page-1)*pageSize).Take(pageSize).ToListAsync();
                 return (query,totalPages);
            }
            catch(Exception e)
            {
                return (null,0);
            }
        }

        public Task<bool> UpdateSpecialty(SpecialtyVM specialty)
        {
            throw new NotImplementedException();
        }
        public Tuple<int, string> SaveMedia(IFormFile imageFile)
        {
            try
            {
                var contentPath = this._hostingEnvironment.ContentRootPath;
                // path = "c://projects/productminiapi/uploads" ,not exactly something like that
                var path = Path.Combine(contentPath, "Uploads");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                // Check the allowed extenstions
                var ext = Path.GetExtension(imageFile.FileName);
                var allowedExtensions = new string[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".mp4", ".avi", ".mov", ".wmv", ".flv", ".mkv" };
                if (!allowedExtensions.Contains(ext))
                {
                    string msg = string.Format("Only {0} extensions are allowed", string.Join(",", allowedExtensions));
                    return new Tuple<int, string>(0, msg);
                }
                string uniqueString = Guid.NewGuid().ToString();
                var fileName = Path.GetFileName(imageFile.FileName);
                // we are trying to create a unique filename here
                var newFileName = Guid.NewGuid().ToString().Substring(0, 4) + "__" + fileName;
                var fileWithPath = Path.Combine(path, newFileName);
                var stream = new FileStream(fileWithPath, FileMode.Create);
                imageFile.CopyTo(stream);
                stream.Close();
                return new Tuple<int, string>(1, newFileName);
            }
            catch (Exception ex)
            {
                return new Tuple<int, string>(0, "Error has occured");
            }
        }
        public bool DeleteMedia(string fileName)
        {
            try
            {
                var contentPath = this._hostingEnvironment.ContentRootPath;
                var path = Path.Combine(contentPath, "Uploads");
                var filePath = Path.Combine(path, fileName);

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }


    }
}
