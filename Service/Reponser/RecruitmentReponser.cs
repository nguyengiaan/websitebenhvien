using Microsoft.EntityFrameworkCore;
using websitebenhvien.Data;
using websitebenhvien.Helper;
using websitebenhvien.Models.Enitity;
using websitebenhvien.Models.EnitityVM;
using websitebenhvien.Service.Interface;

namespace websitebenhvien.Service.Reponser
{
    public class RecruitmentReponser : IRecruitment
    {
        private readonly MyDbcontext _context;
        private readonly Uploadfile _uploadfile;

        public RecruitmentReponser(MyDbcontext context,Uploadfile uploadfile) 
        {
            _context = context;
            _uploadfile = uploadfile;
        }
        public async Task<bool> AddRecruitment(Postpersonnel recruitmentpost)
        {
            try
            {
                if (recruitmentpost.id_recruitmentpost > 0)
                {
                    var data = await _context.postpersonnel.FindAsync(recruitmentpost.id_recruitmentpost);
                    if (data != null)
                    {
                        data.title_recruitmentpost = recruitmentpost.title_recruitmentpost ?? throw new ArgumentException("Title cannot be null");
                        data.Content_recruitmentpost = recruitmentpost.Content_recruitmentpost;
                        data.Status = recruitmentpost.Status;
                        data.Statuson = true;
                        await _context.SaveChangesAsync();
                        return true;
                    }
                }
                else
                {
                    var data = new Postpersonnel
                    {
                        title_recruitmentpost = recruitmentpost.title_recruitmentpost ?? throw new ArgumentException("Title cannot be null"),
                        Date_recruitmentpost = DateTime.Now,
                        Content_recruitmentpost = recruitmentpost.Content_recruitmentpost,
                        Status = recruitmentpost.Status,
                        Statuson = true
                    };

                    await _context.postpersonnel.AddAsync(data);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<(int Totalpages, List<Postpersonnel> Data)> GetAllRecruitment(string search, int page, int pageSize)
        {
            try
            {
                if (search == null)
                {
                    var totalitem = await _context.postpersonnel.CountAsync();
                    var totalpage = (int)Math.Ceiling((double)totalitem / pageSize);
                    var data = new List<Postpersonnel>();
                    data = await _context.postpersonnel.AsNoTracking().Select(x => new Postpersonnel
                    {
                        id_recruitmentpost = x.id_recruitmentpost,
                        title_recruitmentpost = x.title_recruitmentpost,
                        Date_recruitmentpost = x.Date_recruitmentpost,
                        Status = x.Status,
                        Statuson = x.Statuson,
                    }).OrderByDescending(x => x.Date_recruitmentpost).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
                    return (totalpage, data);
                }
                else
                {
                    var totalitem = await _context.postpersonnel.Where(x => x.title_recruitmentpost.Contains(search)).CountAsync();
                    var totalpage = (int)Math.Ceiling((double)totalitem / pageSize);
                    var data = new List<Postpersonnel>();
                    data = await _context.postpersonnel.AsNoTracking().Select(x => new Postpersonnel
                    {
                        id_recruitmentpost = x.id_recruitmentpost,
                        title_recruitmentpost = x.title_recruitmentpost,
                        Date_recruitmentpost = x.Date_recruitmentpost,
                        Status = x.Status,
                        Statuson = x.Statuson,
                    }).Where(x => x.title_recruitmentpost.Contains(search)).OrderByDescending(x => x.Date_recruitmentpost).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
                    return (totalpage, data);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<bool> DeleteRecruitment(int id)
        {
            try
            {
                var data=await _context.postpersonnel.FindAsync(id);

                if(data == null)
                {
                    return false;
                }
                else
                {
                    _context.postpersonnel.Remove(data);
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
        public async Task<Postpersonnel> GetPostpersonnelID(int id)
        {
            try
            {
                var data = await _context.postpersonnel.FindAsync(id);
                if(data==null)
                {
                    return null;

                }
                else
                {
                    return data;
                }
                return null;
            }
            catch(Exception ex)
            {
                return null;
            }
        }
        public async Task<bool> OnchangePost(int id)
        {
            try
            {
                var data=await _context.postpersonnel.FindAsync(id);
                if (data == null)
                {
                    return false;
                }
                else
                {
                    data.Statuson = !data.Statuson;
                    await _context.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<List<Postpersonnel>> Getallpostrecruiment()
        {
            try
            {
                var data = await _context.postpersonnel.Where(x => x.Statuson == true).ToListAsync();
                return data;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        // video
        public async Task<bool> AddVideo(VideosVM video)
        {
            try
            {
                if (video.Id_video > 0)
                {
                    var data = await _context.Videos.FindAsync(video.Id_video);
                    if (data != null)
                    {
                        if (video.formFile != null)
                        {
                            var link =await  _uploadfile.SaveMedia(video.formFile);
                            if (link.Item1 == 1)
                            {
                                _uploadfile.DeleteMedia(data.Link_video);
                                data.Link_video = link.Item2;

                            }
                        }
                        data.Title_video = video.Title_video;
                        data.Description_video = video.Description_video;
                        data.Status_video = video.Status_video;
                        await _context.SaveChangesAsync();
                        return true;
                    }
                }
                else
                {
                
                    var data = new Videos
                    {
                        Title_video = video.Title_video,
                        Description_video = video.Description_video,
                        Status_video = video.Status_video,
                        Created_at_video=DateTime.Now,
                    };
                    if (video.formFile != null)
                    {
                        var link =await _uploadfile.SaveMedia(video.formFile);
                        if (link.Item1 == 1)
                        {
                            data.Link_video = link.Item2;
                        }
                    }
                    await _context.Videos.AddAsync(data);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteVideo(int id)
        {
            try
            {
                var data = await _context.Videos.FindAsync(id);
                if (data != null)
                {
                    _uploadfile.DeleteMedia(data.Link_video);
                    _context.Videos.Remove(data);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<(int Totalpages, List<VideosVM> Data)> GetAllVideo(string search,int page, int pageSize)
        {
            try
            {
                if(search == null)
                {
                    var totalitem = await _context.Videos.CountAsync();
                    var totalpage = (int)Math.Ceiling((double)totalitem / pageSize);
                    var data = new List<VideosVM>();
                    data = await _context.Videos.OrderByDescending(x => x.Created_at_video).Skip((page - 1) * pageSize).Take(pageSize).Select(x => new VideosVM
                    {
                        Id_video = x.Id_video,
                        Title_video = x.Title_video,
                        Description_video = x.Description_video,
                        Status_video = x.Status_video,
                        Link_video = x.Link_video,
                        Created_at_video = x.Created_at_video,
                    }).ToListAsync();

                return (totalpage, data);
                }
                else
                {
                    var totalitem = await _context.Videos.Where(x => x.Title_video.Contains(search)).CountAsync();
                    var totalpage = (int)Math.Ceiling((double)totalitem / pageSize);
                    var data = new List<VideosVM>();
                    data = await _context.Videos.Where(x => x.Title_video.Contains(search)).OrderByDescending(x => x.Created_at_video).Skip((page - 1) * pageSize).Take(pageSize).Select(x => new VideosVM
                    {
                        Id_video = x.Id_video,
                        Title_video = x.Title_video,
                        Description_video = x.Description_video,
                        Status_video = x.Status_video,
                        Link_video = x.Link_video,
                        Created_at_video = x.Created_at_video,
                    }).ToListAsync();
                    return (totalpage, data);
                }
            
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<VideosVM>> GetVideosClient()
        {
            try
            {
                var data=await _context.Videos.Where(x=>x.Status_video==true).Select(x=>new VideosVM {
                    Link_video=x.Link_video,
                    Created_at_video=x.Created_at_video,
                }).ToListAsync();
                return data;

            }
            catch(Exception ex)
            {
                return null;
            }
        }
        // machine
        public async Task<bool> AddMachine(MachineVM machine)
        {
            try
            {
                if(machine.Id_machine > 0)
                {
                    var data = await _context.Machines.FindAsync(machine.Id_machine);
                    if (data != null)
                    {
                        if (machine.formFile != null)
                        {
                            var link = await _uploadfile.SaveMedia(machine.formFile);
                            if (link.Item1 == 1)
                            {
                                _uploadfile.DeleteMedia(data.Image_machine);
                                data.Image_machine = link.Item2;
                            }
                        }
                        data.Name_machine = machine.Name_machine;
                        data.Description_machine = machine.Description_machine;
                        data.Status = machine.Status;
           

                        await _context.SaveChangesAsync();
                        return true;
                    }
                }
                else
                {
                    var data = new Machine
                    {
                        Name_machine = machine.Name_machine,
                        Description_machine = machine.Description_machine,
                        Status = machine.Status,
                        CreatedBy = DateTime.Now,
                    };
                    if (machine.formFile != null)
                    {
                        var link = await _uploadfile.SaveMedia(machine.formFile);
                        if (link.Item1 == 1)
                        {
                            data.Image_machine = link.Item2;
                        }
                    }
                    await _context.Machines.AddAsync(data);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<(int Totalpages, List<MachineVM> Data)> GetAllMachine(string search, int page, int pageSize)
        {
            try
            {
                if(search==null)
                {
                    var totalpage = (int)Math.Ceiling((double)_context.Machines.Count() / pageSize);
                   
                    var data = new List<MachineVM>();
                    data = await _context.Machines.OrderByDescending(x => x.CreatedBy).Skip((page - 1) * pageSize).Take(pageSize).Select(x => new MachineVM
                    {
                        Id_machine = x.Id_machine,
                        Name_machine = x.Name_machine,
                        Description_machine = x.Description_machine,
                        Status = x.Status,
                        Image_machine = x.Image_machine,
                        CreatedBy = x.CreatedBy,
                    }).ToListAsync();
                    return (totalpage, data);

                }
                else
                {
                    var totalpage = (int)Math.Ceiling((double)_context.Machines.Where(x => x.Name_machine.Contains(search)).Count() / pageSize);
                    var data = new List<MachineVM>();
                    data = await _context.Machines.Where(x => x.Name_machine.Contains(search)).OrderByDescending(x => x.CreatedBy).Skip((page - 1) * pageSize).Take(pageSize).Select(x => new MachineVM
                    {
                        Id_machine = x.Id_machine,
                        Name_machine = x.Name_machine,
                        Description_machine = x.Description_machine,
                        Status = x.Status,
                        Image_machine = x.Image_machine,
                        CreatedBy = x.CreatedBy,
                    }).ToListAsync();
                    return (totalpage, data);

                }
                
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteMachine(int id)
        {
            try
            {
                var data = await _context.Machines.FindAsync(id);
                if(data!=null)
                {
                    _uploadfile.DeleteMedia(data.Image_machine);
                     _context.Machines.Remove(data);
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

        public async Task<List<MachineVM>> GetMachineClient()
        {
            try
            {
                var data= await _context.Machines.Where(x => x.Status == true).Select(x => new MachineVM
                {
                    Name_machine = x.Name_machine,
                    Description_machine = x.Description_machine,
                    Image_machine = x.Image_machine,
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
