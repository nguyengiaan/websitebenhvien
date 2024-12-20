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

        public async Task<bool> DeleteSpecialty(int id)
        {
          try
          {
            var data = await _context.Specialties.FindAsync(id);
            if(data != null)
            {
                _context.Specialties.Remove(data);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;

          }
          catch(Exception e)
          {
            return false;
          }
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

        public async Task<SpecialtyVM> GetSpecialtyById(int id)
        {
            try{
                var data = await _context.Specialties.Where(x=>x.Id_Specialty == id).Select(x=>new SpecialtyVM{
                    Id_Specialty = x.Id_Specialty,
                    Title = x.Title,
                    Thumnail = x.Thumnail,
                    Alias_url = x.Alias_url,
                    Introduction = x.Introduction,
                    Machine = x.Machine,
                    Method = x.Method,
                    Service = x.Service,
                    Achievement = x.Achievement,
                  
                }).FirstOrDefaultAsync();
                return data;
            }catch(Exception e)
            {
                return null;
            }
        }

        public async Task<bool> AddDoctor(DoctorVM doctor)
        {
            try
            {
                var data = await _context.Doctors.FindAsync(doctor.Id_doctor);
                if(data != null)
                {
                    UpdateDoctorData(data,doctor);
                    if(doctor.ImageFile != null)
                    {
                        var result = SaveMedia(doctor.ImageFile);
                        if(result.Item1 == 1)
                        {
                            DeleteMedia(data.Thumnail);
                            data.Thumnail = result.Item2;
                        }
                    }
                    await _context.SaveChangesAsync();
                    return true;
                }
                else
                {
                    var newDoctor = new Doctor();
                    UpdateDoctorData(newDoctor,doctor);
                    if(doctor.ImageFile != null)
                    {
                        var result = SaveMedia(doctor.ImageFile);
                        if(result.Item1 == 1)
                        {
                            newDoctor.Thumnail = result.Item2;
                        }
                    }
                    await _context.Doctors.AddAsync(newDoctor);
                    await _context.SaveChangesAsync();
                    return true;
                 
                }
            }
            catch(Exception e)
            {
                return false;
            }
        }
        private void UpdateDoctorData(Doctor target, DoctorVM source)
        {
            target.Name = source.Name;
            target.Introduction = source.Introduction;
            target.Organization = source.Organization;
            target.Award = source.Award;
            target.Research = source.Research;
            target.Training = source.Training;
            target.Experiencework = source.Experiencework;
            target.Alias_url = source.Alias_url;
            target.Id_specialty = source.Id_specialty;
        }

        

        public async Task<bool> DeleteDoctor(int id)
        {
            try
            {
                var data = await _context.Doctors.FindAsync(id);
                if(data != null)
                {
                    _context.Doctors.Remove(data);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }catch(Exception e)
            {
                return false;
            }
        }

        public async Task<List<DoctorVM>> GetDoctorBySpecialty(int id)
        {
            try{
                var data = await _context.Doctors.Where(x=>x.Id_doctor == id).Select(x=>new DoctorVM{
                    Id_doctor = x.Id_doctor,
                    Name = x.Name,
                    Thumnail = x.Thumnail,
                    Alias_url = x.Alias_url,
                    Introduction = x.Introduction,
                    Organization = x.Organization,
                    Award = x.Award,
                    Research = x.Research,
                    Training = x.Training,
                    Experiencework = x.Experiencework,
                    Id_specialty = x.Id_specialty,
                
                  
                }).ToListAsync();
                return data;
            }catch(Exception e)
            {
                return null;
            }
        }

        public async Task<List<SpecialtyVM>> GetAllSpecialty()
        {
            try{
                var data = await _context.Specialties.Select(x=>new SpecialtyVM{
                    Id_Specialty = x.Id_Specialty,
                    Title = x.Title,
                    Alias_url = x.Alias_url,
                    Thumnail = x.Thumnail,
                }).ToListAsync();
                return data;
            }catch(Exception e)
            {
                return null;
            }
        }

        public async Task<(List<DoctorVM> ds, int TotalPages)> GetDoctorByAlias(int page, int pageSize)
        {
            try{
                var totalItems = await _context.Doctors.CountAsync(); 
                var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
                var query = await _context.Doctors.Include(x=>x.Specialty).AsNoTracking().Select(x=>new DoctorVM{
                    Id_doctor = x.Id_doctor,
                    Name = x.Name,
                    Thumnail = x.Thumnail,
                    Alias_url = x.Alias_url,
                    nameCategory = x.Specialty.Title,
                }).OrderByDescending(x=>x.Id_doctor).Skip((page-1)*pageSize).Take(pageSize).ToListAsync();
                return (query,totalPages);
            }catch(Exception e)
            {
                return (null,0);
            }
        }
    }
}
