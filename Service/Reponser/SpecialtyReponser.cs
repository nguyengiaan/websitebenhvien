using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using websitebenhvien.Data;
using websitebenhvien.Helper;
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
        private readonly Uploadfile _upload;

        public SpecialtyReponser(Uploadfile upload,MyDbcontext context, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _hostingEnvironment = hostingEnvironment;
            _upload=upload;
             
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
                        var result = await _upload.SaveMedia(specialty.formFile);
                        if (result.Item1 == 1)
                        {
                            _upload.DeleteMedia(data.Thumnail);
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
                        var result = await _upload.SaveMedia(specialty.formFile);
                        if (result.Item1 == 1)
                        {
                            newSpecialty.Thumnail = result.Item2;
                        }
                        else
                        {
                            newSpecialty.Thumnail = "avt.jpg";
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
            target.Service = source.Service;
   
            target.Alias_url = source.Alias_url;
        }

        public async Task<bool> DeleteSpecialty(int id)
        {
          try
          {
            var data = await _context.Specialties.FindAsync(id);
            if(data != null)
            {
                if(data.Thumnail != null && data.Thumnail!= "avt.jpg")
                {
                    _upload.DeleteMedia(data.Thumnail);
                }
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
            
                    Service = x.Service,
               
                  
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
        var existingDoctor = await _context.Doctors.FindAsync(doctor.Id_doctor);
        var targetDoctor = existingDoctor ?? new Doctor();
        
        UpdateDoctorData(targetDoctor, doctor);
        
        if (doctor.ImageFile != null)
        {
            var (success, fileName) = await _upload.SaveMedia(doctor.ImageFile);
            if (success == 1)
            {
                if (existingDoctor != null && existingDoctor.Thumnail != "avt.jpg")
                {
                    _upload.DeleteMedia(existingDoctor.Thumnail);
                }
                targetDoctor.Thumnail = fileName;
            }
        }
        else if (existingDoctor == null)
        {
            targetDoctor.Thumnail = "avt.jpg";
        }

        if (existingDoctor == null)
        {
            await _context.Doctors.AddAsync(targetDoctor);
        }

        await _context.SaveChangesAsync();
        return true;
    }
    catch (Exception)
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
                    if(data.Thumnail != null && data.Thumnail != "avt.jpg")
                    {
                        _upload.DeleteMedia(data.Thumnail);
                    }
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
                
                  
                }).AsNoTracking().ToListAsync();
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
                 
                }).AsNoTracking().ToListAsync();
                return data;
            }catch(Exception e)
            {
                return null;
            }
        }

        public async Task<(List<DoctorVM> ds, int TotalPages)> GetDoctorByAlias(int page, int pageSize,string search,int specialtyId)
        {
            try{
                var query = _context.Doctors.AsQueryable();
                if(!string.IsNullOrEmpty(search))
                {
                    query = query.Where(x=>x.Name.Contains(search));
                }
                if(specialtyId != 0)
                {
                    query = query.Where(x=>x.Id_specialty == specialtyId);
                }
                var totalItems = await query.CountAsync(); 
                var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
                var data = await query.Include(x=>x.Specialty).AsNoTracking().Select(x=>new DoctorVM{
                    Id_doctor = x.Id_doctor,
                    Name = x.Name,
                    Thumnail = x.Thumnail,
                    Alias_url = x.Alias_url,
                    nameCategory = x.Specialty.Title,
                }).OrderByDescending(x=>x.Id_doctor).Skip((page-1)*pageSize).Take(pageSize).AsNoTracking().ToListAsync();
                return (data,totalPages);
            }catch(Exception e)
            {
                return (null,0);
            }
        }

  
        public Task<List<DoctorSpeciallyVM>> GetDoctorSpecialty()
        {
            try
            {
                var data = _context.Specialties.Select(s => new DoctorSpeciallyVM
                {
                    Id_Specialty = s.Id_Specialty,
                    Name_Specialty = s.Title,
                    Thumnail = s.Thumnail,
                    Doctors = s.Doctor.Select(d => new DoctorVM
                    {
                        Id_doctor = d.Id_doctor,
                        Name = d.Name,
                        Thumnail = d.Thumnail,
                        Alias_url = d.Alias_url,
                        Introduction = d.Introduction,
                        Organization = d.Organization,
                        Award = d.Award,
                        Research = d.Research,
                        Training = d.Training,
                        Experiencework = d.Experiencework,
                        Id_specialty = d.Id_specialty
                    }).ToList()
                }).ToListAsync();

                return data;
            }
            catch (Exception)
            {
                return Task.FromResult<List<DoctorSpeciallyVM>>(null);
            }
        }
    }
}
