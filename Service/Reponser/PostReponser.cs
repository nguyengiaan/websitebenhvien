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

        public PostReponser(Hubnot hubnot,MyDbcontext context, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _hostingEnvironment = hostingEnvironment;
            _hubnot = hubnot;

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
                var md=SaveMedia(recruitment.CV_Url);
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
               return true;
            }
            catch(Exception ex){
                return false;
            }
        }         public Tuple<int, string> SaveMedia(IFormFile imageFile)
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
                var allowedExtensions = new string[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".mp4", ".avi", ".mov", ".wmv", ".flv", ".mkv", ".doc", ".docx", ".pdf", ".txt", ".rtf" };
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
                _context.Recruitments.Remove(data);
                await _context.SaveChangesAsync();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

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
                        HttpOnly = true,
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
               return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
    }
}
