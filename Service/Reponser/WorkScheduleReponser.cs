using Microsoft.EntityFrameworkCore;
using websitebenhvien.Data;
using websitebenhvien.Helper;
using websitebenhvien.Models.Enitity;
using websitebenhvien.Models.EnitityVM;
using websitebenhvien.Service.Interface;

namespace websitebenhvien.Service.Reponser
{
    public class WorkScheduleReponser : IWorkSchedule
    {
        private readonly MyDbcontext _context;
        private readonly Hubnot _hubnot;
        private readonly EmailSender _email;

        public WorkScheduleReponser(EmailSender email,MyDbcontext context,Hubnot hubnot)
        {
            _context = context;
            _hubnot = hubnot;
            _email=email;
        }
        public async Task<bool> AddWorkschedule(WorkdoctorVM workschedule)
        {
            if (workschedule == null) return false;

            try
            {
                var data = await _context.Worksdoctors
                    .Include(w => w.Workschedules)
                    .FirstOrDefaultAsync(w => w.Id_worksdoctor == workschedule.Id_workdoctor);

                if (data != null)
                {
                    // Cập nhật thông tin Worksdoctor
                    data.CreateAt = workschedule.CreateAt;
                    data.EndDate = workschedule.EndDate;
                    data.Note = workschedule.Note;

                    // Cập nhật hoặc thêm Workschedules
                    foreach (var item in workschedule.Workschedules)
                    {
                        var existingSchedule = data.Workschedules
                            .FirstOrDefault(w => w.Id_workschedule == item.Id_workschedule);

                        if (existingSchedule != null)
                        {
                            existingSchedule.date = item.Date;
                            existingSchedule.Morning = item.Morning;
                            existingSchedule.Afternoon = item.Afternoon;
                            existingSchedule.Evening = item.Evening;
                        }
                        else
                        {
                            var newSchedule = new Workschedule
                            {
                                date = item.Date,
                                Morning = item.Morning,
                                Afternoon = item.Afternoon,
                                Evening = item.Evening,
                                Id_worksdoctor = data.Id_worksdoctor // Gán khóa ngoại
                            };
                            data.Workschedules.Add(newSchedule);
                        }
                    }
                }
                else
                {
                    // Thêm mới Worksdoctor và Workschedules
                    var newWorkDoctor = new Worksdoctor
                    {
                        CreateAt = workschedule.CreateAt,
                        EndDate = workschedule.EndDate,
                        Id_doctor = workschedule.Id_doctor,
                        Note = workschedule.Note
                    };

                    await _context.Worksdoctors.AddAsync(newWorkDoctor);
                    await _context.SaveChangesAsync(); // Lưu để lấy Id_worksdoctor

                    // Thêm Workschedules với khóa ngoại
                    foreach (var item in workschedule.Workschedules)
                    {
                        var newSchedule = new Workschedule
                        {
                            date = item.Date,
                            Morning = item.Morning,
                            Afternoon = item.Afternoon,
                            Evening = item.Evening,
                            Id_worksdoctor = newWorkDoctor.Id_worksdoctor // Gán khóa ngoại
                        };

                        await _context.Workschedules.AddAsync(newSchedule);
                    }
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                // Log lỗi để kiểm tra khi cần
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<bool> DeleteWorkschedule(int id)
        {
           try{
            var data=await _context.Worksdoctors.FindAsync(id);
              if(data!=null){
                _context.Worksdoctors.Remove(data);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
           }
            catch(Exception ex)
            {
            return false;
           }
        }



        public async Task<WorkdoctorVM> GetWorkschedule(int id)
        {
            try
            {
                var data=await _context.Worksdoctors
                    .Include(w => w.Workschedules)
                    .Where(w => w.Id_doctor == id)
                    .Select(w => new WorkdoctorVM
                    {
                        Id_workdoctor = w.Id_worksdoctor,
                        CreateAt = w.CreateAt,
                        EndDate = w.EndDate,
                        Id_doctor = w.Id_doctor,
                        Note = w.Note,
                        Workschedules = w.Workschedules.Select(ws => new WorkscheduleVM
                        {
                            Id_workschedule = ws.Id_workschedule,
                            Date = ws.date,
                            Morning = ws.Morning,
                            Afternoon = ws.Afternoon,
                            Evening = ws.Evening
                        }).ToList()
                    }).FirstOrDefaultAsync();
                return data;
            }
            catch (Exception ex)
            {
       
                return null;
            }
        }

        public async Task<bool> RegisterWorkschedule(MakeanappointmentVM workschedule)
        {
            try
            {
                if(workschedule.Id_Specialty==0 || workschedule.Name_doctor=="Không có"){
                    var data1=new Registerhealth();
                    data1.Examinationtime=workschedule.Examinationtime;
                    data1.name=workschedule.name;
                    data1.phone=workschedule.phone;
                    data1.note=workschedule.note;
                    data1.Status=false;
                    var notfi1=new Notification();
                    notfi1.Id_Notification=Guid.NewGuid().ToString();
                    notfi1.Createat=DateTime.Now;
                    notfi1.Status=false;
                    notfi1.Url="/trang-quan-tri/dang-ky-kham-suc-khoe";
                    notfi1.Title="Bạn có một bệnh nhân mới";
                    notfi1.Description="Khách hàng "+workschedule.name+" đã đăng ký khám sức khoẻ vào lúc "+workschedule.Examinationtime;
                    await _context.Registerhealths.AddAsync(data1);
                    await _context.Notifications.AddAsync(notfi1);
                    await _hubnot.SendNotification();
                    await _email.SendEmailAsync("2024801030185@student.tdmu.edu.vn", "ĐĂNG KÝ KHÁM BỆNH", "Khách hàng " + workschedule.name + " đã đăng ký khám sức khoẻ vào lúc " + workschedule.Examinationtime+"Số điện thoại "+workschedule.phone,null);
                    await _context.SaveChangesAsync();
                    return true;
                }
                else
                {
                var data= new Makeanappointment();
                var notfi=new Notification();
                data.Id_Specialty=workschedule.Id_Specialty;
                data.Name_doctor=workschedule.Name_doctor;
                data.Examinationtime=workschedule.Examinationtime;
                data.name =workschedule.name;
                data.phone=workschedule.phone;
                data.note=workschedule.note;
                data.Status=false;
                await _context.Makeanappointments.AddAsync(data);
                notfi.Id_Notification=Guid.NewGuid().ToString();
                notfi.Createat=DateTime.Now;
                notfi.Status=false;
                notfi.Url="/trang-quan-tri/dang-ky-kham-benh";
                notfi.Title="Bạn có một bệnh nhân mới";
                notfi.Description="Khách hàng "+workschedule.name+" đã đăng ký khám bệnh vào lúc "+workschedule.Examinationtime;
                await _context.Notifications.AddAsync(notfi);
                await _context.SaveChangesAsync();
                await _hubnot.SendNotification();
               await _email.SendEmailAsync("2024801030185@student.tdmu.edu.vn", "ĐĂNG KÝ KHÁM BỆNH", "Khách hàng " + workschedule.name + " đã đăng ký khám bệnh vào lúc " + workschedule.Examinationtime+"Số điện thoại "+workschedule.phone,null);
                return true;
                }
                return false;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
        public async Task<List<DoctorVM>> GetDoctor(int id)
        {
            try
            {
                var data = await _context.Doctors.Where(x => x.Id_specialty == id).Select(x => new DoctorVM
                {
                    Id_doctor = x.Id_doctor,
                    Name = x.Name,
                }).ToListAsync();
                return data;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<(List<MakeanappointmentVM> ds, int total)> GetAppointment(int page, int pageSize, string search, int specialtyId)
        {
          try
          {
            var query = _context.Makeanappointments.AsQueryable();
            
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.name.Contains(search) || x.Name_doctor.Contains(search));
            }

            if (specialtyId > 0)
            {
                query = query.Where(x => x.Id_Specialty == specialtyId);
            }

            var totalItems = await query.CountAsync();
            var totalpages = (int)Math.Ceiling(totalItems / (double)pageSize);

                var result = await query.Select(x => new MakeanappointmentVM {
                    Id_makeanappointment = x.Id_Make,
                    Id_Specialty = x.Id_Specialty,
                    Name_doctor = x.Name_doctor,
                    Examinationtime = x.Examinationtime,
                    name = x.name,
                    phone = x.phone,
                    note = x.note,
                    Title_Specialty=x.Specialty.Title,
                    Status=x.Status,


                })
            .OrderByDescending(x => x.Id_makeanappointment)
            .Skip((page-1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

            return (result, totalpages);
          }
          catch(Exception ex)
          {
            return (null, 0);
          }
        }

        public async Task<bool> DeleteAppointment(int id)
        {
            try{
                var data=await _context.Makeanappointments.FindAsync(id);
                if(data!=null){
                    _context.Makeanappointments.Remove(data);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch(Exception ex){
                return false;
            }
        }

        public async Task<bool> UpdateAppointment(int id)
        {
            try{
                var data=await _context.Makeanappointments.FindAsync(id);
                if(data!=null){
                    data.Status=!data.Status;
                    await _context.SaveChangesAsync();
                    return true;
                }   
                return false;
            }
            catch(Exception ex){
                return false;
            }
        }

        public  async Task<(List<MakeanappointmentVM> ds, int total)> GetAppointmentSK(int page, int pageSize, string search)
        {
             try
          {
            var query = _context.Registerhealths.AsQueryable();
            
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.name.Contains(search) );
            }
            var totalItems = await query.CountAsync();
            var totalpages = (int)Math.Ceiling(totalItems / (double)pageSize);

                var result = await query.Select(x => new MakeanappointmentVM {
                    Id_makeanappointment = x.Id_Registerhealth,
                    Examinationtime = x.Examinationtime,
                    name = x.name,
                    phone = x.phone,
                    note = x.note,
                    Status=x.Status,


                })
            .OrderByDescending(x => x.Id_makeanappointment)
            .Skip((page-1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

            return (result, totalpages);
          }
          catch(Exception ex)
          {
            return (null, 0);
          }
        }

        public async Task<bool> DeleteAppointmentSK(int id)
        {
            try{
                var data=await _context.Registerhealths.FindAsync(id);
                if(data!=null){
                    _context.Registerhealths.Remove(data);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch(Exception ex){
                return false;
            }
        }

        public async Task<bool> UpdateAppointmentSK(int id)
        {
            try{
                var data=await _context.Registerhealths.FindAsync(id);
                if(data!=null){
                    data.Status=!data.Status;
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch(Exception ex){
                return false;
            }
        }

        public async Task<RegisterChart> GetRegisterChart()
        {
           try
           {
            var data=new RegisterChart();
            data.Songuoidksk=await _context.Registerhealths.CountAsync();
            data.Songuoidkskcd=await _context.Registerhealths.Where(x=>x.Status==false).CountAsync();
            data.Songuoidkdd=await _context.Registerhealths.Where(x=>x.Status==true).CountAsync();
            data.Songuoidkb=await _context.Makeanappointments.CountAsync();
            data.Songuoidkkbcd=await _context.Makeanappointments.Where(x=>x.Status==false).CountAsync();
            data.Songuoidkkbdd=await _context.Makeanappointments.Where(x=>x.Status==true).CountAsync();
            return data;
           }
           catch(Exception ex)
           {
                return null;
           }
        }

        public async Task<List<Charthealthdate>> GetCharthealthdate()
        {
          try
          {
            var result = await (from rh in _context.Registerhealths
                              group rh by rh.Examinationtime.Date into g
                              select new Charthealthdate
                              {
                                  Date = g.Key,
                                  CountSK = g.Count()
                              }).ToListAsync();

            var appointmentData = await (from ma in _context.Makeanappointments
                                       group ma by ma.Examinationtime.Date into g 
                                       select new
                                       {
                                           Date = g.Key,
                                           CountKB = g.Count()
                                       }).ToListAsync();

            foreach (var item in result)
            {
                var appointment = appointmentData.FirstOrDefault(x => x.Date == item.Date);
                if (appointment != null)
                {
                    item.CountKB = appointment.CountKB;
                }
            }

            foreach (var appointment in appointmentData)
            {
                if (!result.Any(x => x.Date == appointment.Date))
                {
                    result.Add(new Charthealthdate
                    {
                        Date = appointment.Date,
                        CountSK = 0,
                        CountKB = appointment.CountKB
                    });
                }
            }
            return result.OrderBy(x => x.Date).ToList();
          }
        catch(Exception ex)
          {
            return null;
          }
        }
    }
}
