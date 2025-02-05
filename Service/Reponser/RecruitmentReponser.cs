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
        public async Task<bool> AddRecruitment(RecruitmentpostVM recruitmentpost)
        {
            try
            {
                if (recruitmentpost.id_recruitmentpost > 0)
                {
                    var data = await _context.Recruitmentposts.FindAsync(recruitmentpost.id_recruitmentpost);
                    if (data != null)
                    {
                        data.title_recruitmentpost = recruitmentpost.title_recruitmentpost;
                        data.Content_recruitmentpost = recruitmentpost.Content_recruitmentpost;
                        data.Status = recruitmentpost.Status;
                        data.Statuson =true;
                        await _context.SaveChangesAsync();
                        return true;
                    }
                }
                else
                {
                    var data = new Recruitmentpost
                    {
                        title_recruitmentpost = recruitmentpost.title_recruitmentpost,
                        Date_recruitmentpost = DateTime.Now,
                        Content_recruitmentpost = recruitmentpost.Content_recruitmentpost,
                        Status = recruitmentpost.Status,
                        Statuson = true,
                    };
                    await _context.Recruitmentposts.AddAsync(data);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
               
          
            }
            catch(Exception ex)
            {
                throw ex;
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
                            var link = _uploadfile.SaveMedia(video.formFile);
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
                        var link = _uploadfile.SaveMedia(video.formFile);
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
    }
}
