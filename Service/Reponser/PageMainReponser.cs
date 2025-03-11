using AspNetCoreGeneratedDocument;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using websitebenhvien.Data;
using websitebenhvien.Models.Enitity;
using websitebenhvien.Models.EnitityVM;
using websitebenhvien.Service.Interface;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace websitebenhvien.Service.Reponser
{
    public class PageMainReponser : IPageMain
    {
        private readonly MyDbcontext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public PageMainReponser(MyDbcontext context, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _hostingEnvironment = hostingEnvironment;

        }
        public async Task<Boolean> Editheader(HeaderVM header)
        {
            try
            {
                var headerEntity = await _context.Header.FirstOrDefaultAsync();
                if (header.formFile != null)
                {
                    var a = DeleteMedia(headerEntity.logo);
                    var md = SaveMedia(header.formFile);
                    if (md.Item1 == 1)
                    {
                        headerEntity.logo = md.Item2;
                    }
                }
                headerEntity.telephone = header.telephone;
                if (header.titleheader.Count == 0)
                {
                    return false;
                }
                else
                {
                    foreach (var item in header.titleheader)
                    {
                        var titleheaderEntity = await _context.Titleheader.FindAsync(item.Id_titleheader);
                        titleheaderEntity.title = item.title;
                        titleheaderEntity.link = item.link;
                    }
                }
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }


        }

        public async Task<List<HeaderVM>> GetTitleheader()
        {
            try
            {
                var data = await _context.Header.Select(x => new HeaderVM
                {
                    logo = x.logo,
                    telephone = x.telephone,
                    titleheader = _context.Titleheader
                     .Where(y => y.Id_header == x.Id_header) // Nếu có khóa liên kết giữa Header và Titleheader
                      .Select(y => new TitleheaderVM
                      {
                          Id_titleheader = y.Id_titleheader,
                          title = y.title,
                          link = y.link
                      }).ToList()
                }).ToListAsync();
                return data;

            }
            catch (Exception ex)
            {
                return null;
            }
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

        public async Task<bool> AddSlide(SlideVM slide)
        {
            try
            {
                var md = SaveMedia(slide.formFile);

                var slide1 = new Slidepage();
                slide1.Id_slidepage = Guid.NewGuid().ToString();
                slide1.title = slide.title;
                slide1.sort = slide.sort;
                slide1.link = slide.link;
                slide1.Time = DateTime.Now;

                if (md.Item1 == 1)
                {

                    slide1.slide = md.Item2;
                }
                slide1.status = true;
                _context.Slidepage.Add(slide1);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<List<Slidepage>> GetSlide()
        {
            try
            {
                var data = await _context.Slidepage.ToListAsync();
                return data.OrderByDescending(x => x.Time).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<Boolean> DeleteSlide(string id_slidepage)
        {
            try
            {
                var slide = await _context.Slidepage.FindAsync(id_slidepage);
                if (slide != null)
                {
                    var a = DeleteMedia(slide.slide);
                    _context.Slidepage.Remove(slide);
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

        public async Task<bool> UpdateStatus(string id_slidepage)
        {
            try
            {
                var slide = await _context.Slidepage.FindAsync(id_slidepage);
                if (slide != null)
                {
                    slide.status = !slide.status;
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

        public async Task<Slidepage> GetSlideByTitle(string Id_slidepage)
        {
            try
            {
                var data = await _context.Slidepage.FirstOrDefaultAsync(x => x.Id_slidepage == Id_slidepage);
                return data;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> EditSlide(SlideVM slide)
        {
            try
            {
                var data = await _context.Slidepage.FindAsync(slide.id_slidepage);
                if (slide.formFile != null)
                {
                    var a = DeleteMedia(data.slide);
                    var md = SaveMedia(slide.formFile);
                    if (md.Item1 == 1)
                    {
                        data.slide = md.Item2;
                    }
                }
                data.title = slide.title;
                data.sort = slide.sort;
                data.link = slide.link;
                await _context.SaveChangesAsync();
                return true;


            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> EditFooter(FooterVM footer)
        {
            try
            {
                var footerEntity = await _context.Footers.FirstOrDefaultAsync();
                footerEntity.address = footer.address;
                footerEntity.telephone = footer.telephone;
                footerEntity.Email = footer.Email;
                footerEntity.linkgt = footer.linkgt;
                footerEntity.linktn = footer.linktn;
                footerEntity.linkdnbs = footer.linkdnbs;
                footerEntity.linktd = footer.linktd;
                footerEntity.linkkb = footer.linkkb;
                footerEntity.linkxn = footer.linkxn;
                footerEntity.linkcdha = footer.linkcdha;
                footerEntity.linknt = footer.linknt;
                footerEntity.linkdn = footer.linkdn;
                footerEntity.linkface = footer.linkface;
                footerEntity.linktwiter = footer.linktwiter;
                footerEntity.linkyoutube = footer.linkyoutube;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<Footer> GetFooter()
        {
            try
            {
                var data = await _context.Footers.FirstOrDefaultAsync();
                return data;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> AddLogoCustomer(LogocustomerVM logo)
        {
            try
            {
                var data = new Logocustomer();
                data.Id_logocustomer = Guid.NewGuid().ToString();
                data.CustomerName = logo.CustomerName;
                data.Attime = DateTime.Now;
                data.Status = true;
                if (logo.formFile != null)
                {
                    var md = SaveMedia(logo.formFile);
                    if (md.Item1 == 1)
                    {
                        data.Logo = md.Item2;
                    }
                }
                _context.Logocustomer.Add(data);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        // danh sách logo khách hàng
        public async Task<(List<Logocustomer> ds, int totalpages)> Listlogo(int page, int pagesize)
        {
            try
            {
                var totalItems = await _context.Logocustomer.CountAsync();
                var totalpages = (int)Math.Ceiling(totalItems / (double)pagesize);
                var data = await _context.Logocustomer.OrderByDescending(x => x.Attime).Skip((page - 1) * pagesize)
                    .Take(pagesize).ToListAsync();
                return (data, totalpages);

            }
            catch (Exception ex)
            {
                return (null, 0);
            }
        }
        public async Task<Boolean> DeleteLogoCustomer(string id_logocustomer)
        {
            try
            {
                var data = await _context.Logocustomer.FindAsync(id_logocustomer);
                if (data != null)
                {
                    var a = DeleteMedia(data.Logo);
                    _context.Logocustomer.Remove(data);
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

        public async Task<bool> UpdateStatuslogo(string id_logocustomer)
        {
            try
            {
                var data = _context.Logocustomer.Find(id_logocustomer);
                if (data != null)
                {
                    data.Status = !data.Status;
                    _context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> UpdatelogoCustomer(LogocustomerVM logo)
        {
            try
            {
                var data = await _context.Logocustomer.FindAsync(logo.Id_logo);
                if (logo.formFile != null)
                {
                    var a = DeleteMedia(data.Logo);
                    var md = SaveMedia(logo.formFile);
                    if (md.Item1 == 1)
                    {
                        data.Logo = md.Item2;
                    }
                }
                data.CustomerName = logo.CustomerName;

                await _context.SaveChangesAsync();
                return true;


            }
            catch (Exception ex)
            {
                return false;
            }
        }
        // Chia sẻ khách hàng
        public async Task<bool> AddShareCustomer(SharecustomerVM share)
        {
            try
            {
                var data = new Sharecustomer();
                data.Id_share = Guid.NewGuid().ToString();
                data.title = share.title;
                data.description = share.description;
                data.Createat = DateTime.Now;
                data.status = true;
                data.aliasurl = share.aliasurl;
                if (share.File != null)
                {
                    var md = SaveMedia(share.File);
                    if (md.Item1 == 1)
                    {
                        data.image = md.Item2;
                    }
                }
                await _context.Sharecustomers.AddAsync(data);
                await _context.SaveChangesAsync();
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }
        // danh sách chia sẻ khách hàng
        public async Task<(List<Sharecustomer> ds, int totalpages)> ListShare(int page, int pagesize)
        {
            try
            {
                var totalItems = _context.Sharecustomers.Count();
                var totalpages = (int)Math.Ceiling(totalItems / (double)pagesize);
                var data = await _context.Sharecustomers.OrderByDescending(x => x.Createat).Skip((page - 1) * pagesize)
                    .Take(pagesize).ToListAsync();
                return (data, totalpages);

            }
            catch (Exception ex)
            {
                return (null, 0);
            }
        }

        public async Task<bool> UpdateStatusShare(string id_share)
        {
            try
            {
                var data = await _context.Sharecustomers.FindAsync(id_share);
                if (data != null)
                {
                    data.status = !data.status;
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

        public async Task<bool> DeleteShareCustomer(string id_share)
        {
            try
            {
                var data = await _context.Sharecustomers.FindAsync(id_share);
                if (data != null)
                {
                    var a = DeleteMedia(data.image);
                    _context.Sharecustomers.Remove(data);
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

        public async Task<bool> UpdateShareCustomer(SharecustomerVM share)
        {
            try
            {
                var data = await _context.Sharecustomers.FindAsync(share.Id_share);
                if (share.File != null)
                {
                    var a = DeleteMedia(data.image);
                    var md = SaveMedia(share.File);
                    if (md.Item1 == 1)
                    {
                        data.image = md.Item2;
                    }
                }
                data.title = share.title;
                data.description = share.description;
                data.aliasurl = share.aliasurl;
                await _context.SaveChangesAsync();
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> AddTimeWork(string content)
        {
            try
            {
                if (content == null)
                {
                    return false;
                }
                var data = await _context.TimeWork.FirstOrDefaultAsync();
                if (data != null && data.Content != content)
                {
                    data.Content = content;
                }
                else
                {
                    var data1 = new TimeWork();
                    data1.Id_timework = Guid.NewGuid().ToString();
                    data1.Content = content;
                    _context.TimeWork.Add(data1);
                }
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<TimeWork> TimeWork()
        {
            try
            {
                var data = await _context.TimeWork.FirstOrDefaultAsync();
                return data;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> AddMenu(MenuVM menu)
        {
            try
            {
                var data = new Menu();
                data.Id_menu = Guid.NewGuid().ToString();
                data.Title_menu = menu.Title_menu;
                data.Link_menu = menu.Link_menu;
                data.Content = menu.Content;
                data.Status = true;
                data.Order_menu = menu.Order_menu;
                _context.Menus.Add(data);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<List<MenuVM>> Menu()
        {
            try
            {
                var data = await _context.Menus.Select(x => new MenuVM
                {
                    Id_menu = x.Id_menu,
                    Title_menu = x.Title_menu,
                    Link_menu = x.Link_menu,
                    Content = x.Content,
                    Status = x.Status,
                    Order_menu = x.Order_menu
                }).ToListAsync();
                return data;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> AddSubMenu(SubMenuVM submenu)
        {
            try
            {
                if(submenu.Id_MenuChild !=null)
                {
                    var data1 = await _context.Menuchilds.FindAsync(submenu.Id_MenuChild);
                    if (data1 != null)
                    {
                        data1.Title_MenuChild = submenu.Title_MenuChild;
                        data1.Link_MenuChild = submenu.Link_MenuChild;
                        data1.Id_menu = submenu.Id_menu;
                        data1.Status = true;
                        data1.Order_menu = submenu.Order_menu;
                        await _context.SaveChangesAsync();
                        return true;
                    }
                }
                else
                {
                    var data = new Menuchild();
                    data.Id_MenuChild = Guid.NewGuid().ToString();
                    data.Title_MenuChild = submenu.Title_MenuChild;
                    data.Link_MenuChild = submenu.Link_MenuChild;
                    data.Id_menu = submenu.Id_menu;
                    data.Status = true;
                    data.Order_menu = submenu.Order_menu;
                    _context.Menuchilds.Add(data);
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

        public async Task<List<ListMenuVM>> ListMenu()
        {
            try
            {
                var data = await _context.Menus
                    .Select(menu => new ListMenuVM
                    {
                        Id_menu = menu.Id_menu,
                        Title_menu = menu.Title_menu,
                        Link_menu = menu.Link_menu,
                   
                        Status = menu.Status,
                        Order_menu = menu.Order_menu,
                        Menu = _context.Menuchilds
                            .Where(mc => mc.Id_menu == menu.Id_menu)
                            .Select(mc => new SubMenuVM
                            {
                                Id_MenuChild = mc.Id_MenuChild,
                                Title_MenuChild = mc.Title_MenuChild,
                                Link_MenuChild = mc.Link_MenuChild,
                                Id_menu = mc.Id_menu,
                                Status = mc.Status,
                                Order_menu = mc.Order_menu
                            }).ToList()
                    }).ToListAsync();

                return data;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        // xoá menu cha 
        public async Task<bool> DeleteMenu(string id_menu)
        {
            try
            {
                var data = await _context.Menus.FindAsync(id_menu);
                if (data != null)
                {
                    _context.Menus.Remove(data);
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
        // xoá menu con 
        public async Task<bool> DeleteSubMenu(string id_submenu)
        {
            try
            {
                var data = await _context.Menuchilds.FindAsync(id_submenu);
                if (data != null)
                {
                   _context.Menuchilds.Remove(data);
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

        public async Task<MenuVM> GetMenuvm(string id_menu)
        {
            try
            {
                var data =await _context.Menus.Select(x => new MenuVM
                {
                    Id_menu = x.Id_menu,
                    Title_menu = x.Title_menu,
                    Link_menu = x.Link_menu,
                    Content = x.Content,
                    Status = x.Status,
                    Order_menu = x.Order_menu
                }).FirstOrDefaultAsync(x => x.Id_menu == id_menu);
                return data;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> Updatemenu(MenuVM menu)
        {
            try
            {
                var data = await _context.Menus.FindAsync(menu.Id_menu);
                data.Title_menu = menu.Title_menu;
                data.Link_menu = menu.Link_menu;
                data.Content = menu.Content;
                data.Order_menu = menu.Order_menu;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        // update status
        public async Task<bool> UpdateStatusMenu(string id)
        {
            try
            {
                var data =await _context.Menus.FindAsync(id);
                if (data != null)
                {
                    data.Status = !data.Status;
                    await _context.SaveChangesAsync();
                    return true;
                }
                else
                {
                    var data1 = await _context.Menuchilds.FindAsync(id);
                    if (data1 != null)
                    {
                        data1.Status = !data1.Status;
                        await _context.SaveChangesAsync();
                        return true;
                    }
                }
                return false;

            }
            catch (Exception ex)
            {
                return false;
            }
        }
    
    }
}
