using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using websitebenhvien.Data;
using websitebenhvien.Helper;
using websitebenhvien.Models.Enitity;
using websitebenhvien.Models.EnitityVM;
using websitebenhvien.Service.Interface;

namespace websitebenhvien.Service.Reponser
{
    public class PostReponser : IPost
    {
        private readonly MyDbcontext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly Hubnot _hubnot;
        private readonly Uploadfile _uploadfile;
        private readonly EmailSender _email;

        public PostReponser(EmailSender email,Uploadfile uploadfile,Hubnot hubnot,MyDbcontext context, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _hostingEnvironment = hostingEnvironment;
            _hubnot = hubnot;
            _uploadfile=uploadfile;
            _email = email;

        }
        public async Task<News> GetNewsById(string alias_url)
        {
            try
            {
                var news = await _context.News.Where(x => x.Alias_url == alias_url).FirstOrDefaultAsync();
                return news;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        // danh mục bài viết nổi bật 
        
        public async Task<List<NewsVM>> GetNewsByCategory(string alias_url)
        {
            try
            {
                var GetNewsByCategory = await _context.News.Where(x => x.Alias_url == alias_url && x.Status == true).Select(x=>x.Id_Categorynews).FirstOrDefaultAsync();
                var News = await _context.News.Where(x => x.Id_Categorynews == GetNewsByCategory && x.Status == true).Select(x=>new NewsVM{
                    Title = x.Title,
                    Alias_url = x.Alias_url,
                    Url = x.Url,
                    Createat = x.Createat,
                    Id_Categorynews = x.Id_Categorynews,
                }).ToListAsync();
                return News;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        // nộp cv tuyển dụng
        public async Task<bool> SubmitRecruitment(RecruitmentVM recruitment)
        {
            var email = await _context.Emails.FirstOrDefaultAsync();
            try
            {
                var data=new Recruitment();
                data.Id_Recruitment=Guid.NewGuid().ToString();
                data.Name=recruitment.Name;
                data.Phone=recruitment.Phone;
                data.Address=recruitment.Address;
                data.CreatedAt=DateTime.Now;
                data.Note=recruitment.Note;
                data.Position=recruitment.Position;
                data.Sex=recruitment.Sex;
                var md=_uploadfile.SaveMedia(recruitment.CV_Url);
                if(md.Item1==1)
                {
                    data.CV_Url=md.Item2;
                }
                var notification=new Notification();
                notification.Id_Notification=Guid.NewGuid().ToString();
                notification.Title="Nộp hồ sơ tuyển dụng";
                notification.Description=recruitment.Name+" "+" đã nộp hồ sơ tuyển dụng vị trí "+" "+recruitment.Position;
                notification.Createat=DateTime.Now;
                notification.Url="/trang-quan-tri/quan-ly-tuyen-dung";
                notification.Status=false ;
                await _context.Recruitments.AddAsync(data);
                await _context.Notifications.AddAsync(notification);
                await _context.SaveChangesAsync();
                await _hubnot.SendNotification();
                await _email.SendEmailAsync(email.email,"Nộp hồ sơ tuyển dụng",recruitment.Name+" "+" đã nộp hồ sơ tuyển dụng vị trí "+" "+recruitment.Position,recruitment.CV_Url);
            
               return true;
            }
            catch(Exception ex){
                return false;
            }
        }
        public async Task<List<Notification>> GetNotification()
        {
           try
           {
                var notification=await _context.Notifications.OrderByDescending(x=>x.Createat).Take(10).ToListAsync();
                return notification;
           }
           catch(Exception ex)
           {
                return null;
           }
        }

        public async Task<bool> ViewRecruitment()
        {
            try
            {
                var data=await _context.Notifications.Where(x=>x.Status==false).ToListAsync();
                foreach(var item in data)
                {
                    item.Status=true;
                }
                await _context.SaveChangesAsync();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public async Task<(List<RecruitmentVM> ds, int total)> GetRecruitment(int page, int pageSize)
        {
            try
            {
                var totalItems = await _context.Recruitments.CountAsync();
                var totalpages = (int)Math.Ceiling(totalItems / (double)pageSize);
                var query = await _context.Recruitments.Select(x=>new RecruitmentVM{
                    Id_Recruitment = x.Id_Recruitment,
                    Name = x.Name,
                    Phone = x.Phone,
                    Address = x.Address,
                    CreatedAt = x.CreatedAt,
                    Note = x.Note,
                    Position = x.Position,
                    Sex = x.Sex,
                    CV = x.CV_Url
                }).OrderByDescending(x=>x.CreatedAt).Skip((page-1)*pageSize).Take(pageSize).ToListAsync();
                return (query, totalpages);

            }catch(Exception ex)
            {
                return (null, 0);
            }
        }
        // xoá cv tuyển dụng
        public async Task<bool> DelRecruitment(string id)
        {
            try
            {
                var data=await _context.Recruitments.Where(x=>x.Id_Recruitment==id).FirstOrDefaultAsync();
                if (data != null)
                {
                    if (data.CV_Url != null)
                    {
                        _uploadfile.DeleteMedia(data.CV_Url);
                    }

                    _context.Recruitments.Remove(data);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
        // chat interface

        public async Task<bool> Chatcustomer(string message)
        {
            try
            {
                var sessionId = _httpContextAccessor.HttpContext.Request.Cookies["SessionId"];
               

                if (sessionId == null)
                {
                    // Tạo SessionId mới nếu không tồn tại
                    sessionId = Guid.NewGuid().ToString();

                    // Lưu SessionId vào cookie
                    _httpContextAccessor.HttpContext.Response.Cookies.Append("SessionId", sessionId, new CookieOptions
                    {
                        Expires = DateTime.UtcNow.AddDays(1),  // Cookie tồn tại 1 ngày
                        HttpOnly = false,
                        Secure = false // Chấp nhận cả HTTP và HTTPS
                    });
                    
                }
                var Chat = new Chat()
                {
                    Id_chat= Guid.NewGuid().ToString(),
                    Message = message,
                    Id_Sender = sessionId,
                    Id_Receiver = "admin",
                    Time = DateTime.Now,
                    Status = false,
                };
                var Notification = new Notification()
                {
                    Id_Notification=Guid.NewGuid().ToString(),
                    Title="Khách hàng đã gửi tin nhắn",
                    Description=message,
                    Createat=DateTime.Now,
                    Url="/trang-quan-tri/quan-ly-chat",
                    Status=false,
                };
               await _context.Chats.AddAsync(Chat);
               await _context.Notifications.AddAsync(Notification);
               await _context.SaveChangesAsync();
               await _hubnot.SendNotification();
               var name = sessionId.Substring(0, 3);
                
               await _hubnot.SendChat(sessionId,name );
               return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
        // lấy danh sách chat khách hàng 
        
        public async Task<List<Chat>> GetChat()
        {
            try
            {
                var chat = await _context.Chats.Where(x=>x.Id_Sender!="admin")
                    .GroupBy(x => x.Id_Sender == "admin" ? x.Id_Receiver : x.Id_Sender)
                    .Select(g => g.OrderByDescending(x => x.Time).First())
                    .ToListAsync();
                    
                return chat.OrderByDescending(x=>x.Time).ToList();
            }
            catch(Exception ex)
            {
                return null;
            }
        }
        // chat hiển thị danh sách tin nhắn
        public async Task<List<Chat>> GetChatById(string id)
        {
            try
            {
                var chat = await _context.Chats.Where(x=>x.Id_Sender==id  || x.Id_Receiver==id).ToListAsync();
                return chat.OrderBy(x=>x.Time).ToList();
            }
            catch(Exception ex)
            {
                return null;
            }
        }
        // admin rep tin nhắn khách hàng 
        public async Task<bool> RepChat(string id, string message)
        {
            try
            {
                var data=new Chat()
                {
                    Id_chat=Guid.NewGuid().ToString(),
                    Message=message,
                    Id_Sender="admin",
                    Id_Receiver=id,
                    Time=DateTime.Now,
                    Status=false,
                };
                await _context.Chats.AddAsync(data);
                await _context.SaveChangesAsync();
                await _hubnot.SendNotification();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> ViewChat(string id)
        {
            try
            {
                var data = await _context.Chats.Where(x => x.Id_Sender == id).ToListAsync();
                if (data != null && data.Any())
                {
                    foreach (var item in data)
                    {
                        item.Status = true;
                        _context.Chats.Update(item);
                    }
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> DelChat(string id)
        {
            try
            {
                var data=await _context.Chats.Where(x=>x.Id_Sender==id).ToListAsync();
                _context.Chats.RemoveRange(data);
                await _context.SaveChangesAsync();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public async Task<(Specialty,List<DoctorVM>)> GetSpecialtyById(string alias_url)
        {
            try
            {
                var data=await _context.Specialties.AsNoTracking().Where(x=>x.Alias_url==alias_url).FirstOrDefaultAsync();
                var doctor=await _context.Doctors.AsNoTracking().Where(x=>x.Id_specialty==data.Id_Specialty)
                .Select(x=> new DoctorVM{
                    Id_doctor=x.Id_doctor,
                    Name=x.Name,
                    Introduction=x.Introduction,
                    Alias_url=x.Alias_url,
                    Thumnail=x.Thumnail,
                    nameCategory=data.Title,
                }).ToListAsync();
                return (data,doctor);
            }
            catch(Exception ex)
            {
                return (null,null);
            }
        }

        public async Task<DoctorVM> GetDoctorById(string alias_url)
        {
            try
            {
                var data=await _context.Doctors.AsNoTracking().Where(x=>x.Alias_url==alias_url).Select(x=>new DoctorVM{
                    Id_doctor=x.Id_doctor,
                    Name=x.Name,
                    Introduction=x.Introduction,
                    Alias_url=x.Alias_url,
                    Thumnail=x.Thumnail,
                    nameCategory=x.Specialty.Title,
                    Experiencework =x.Experiencework,
                    Organization=x.Organization,
                    Award=x.Award,
                    Research=x.Research,
                    Training=x.Training,
                   
                   
                    

                }).FirstOrDefaultAsync();
                return data;
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        // rep tin nhắn khách hàng


    }
}
