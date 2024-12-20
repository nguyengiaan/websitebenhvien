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
        public WorkScheduleReponser(MyDbcontext context,Hubnot hubnot)
        {
            _context = context;
            _hubnot = hubnot;
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
            catch(Exception ex){
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
                var data= new Makeanappointment();
                var notfi=new Notification();
                data.Id_Specialty=workschedule.Id_Specialty;
                data.Name_doctor=workschedule.Name_doctor;
                data.Examinationtime=workschedule.Examinationtime;
                data.name =workschedule.name;
                data.phone=workschedule.phone;
                data.note=workschedule.note;
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
                return true;
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

    }
}
