using Microsoft.EntityFrameworkCore;
using websitebenhvien.Data;
using websitebenhvien.Migrations;
using websitebenhvien.Models.Enitity;
using websitebenhvien.Models.EnitityVM;
using websitebenhvien.Service.Interface;

namespace websitebenhvien.Service.Reponser
{
    public class SampleReponser : ISamplemessager
    {
        private readonly MyDbcontext _context;

        public SampleReponser(MyDbcontext context)
        {
           _context = context;
        }
        public async Task<bool> AddSamplemessager(SampleVM message)
        {
            try
            {
                if (message.Id_SampleMessager != null)
                {
                    var data1 = await _context.SampleMessages.FindAsync(message.Id_SampleMessager);
                    data1.Question = message.Question;
                    data1.Reply = message.Reply;
                    data1.Status = "Active";
                    if (message.ButtonSamples != null)
                    {
                        foreach (var item in message.ButtonSamples)
                        {
                            var data = await _context.ButtonSamples.FindAsync(item.Id_ButtonSample);
                            data.Title = item.Title;
                            data.Link = item.Link;
                        }
                    }
                    await _context.SaveChangesAsync();
                    return true;
                }
                else
                {
                    var data = new SampleMessage();
                    data.Question = message.Question;
                    data.Reply = message.Reply;
                    data.Status = "Active";
                    if (message.ButtonSamples != null)
                    {
                        foreach (var item in message.ButtonSamples)
                        {
                            var data1 = new ButtonSample();
                            data1.Title = item.Title;
                            data1.Link = item.Link;
                            data.ButtonSamples.Add(data1);
                        }
                    }
                    await _context.SampleMessages.AddAsync(data);
                    await _context.SaveChangesAsync();
                    return true;
                }
               
              }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<(List<SampleVM> ds, int TotalPages)> Getlistsp(int page, int pagesize)
        {
            try
            {
                var totalItems= await _context.SampleMessages.CountAsync();
                var totalPages= (int)Math.Ceiling(totalItems / (double)pagesize);
                var data = await _context.SampleMessages.Include(x => x.ButtonSamples).OrderByDescending(x => x.Id_SampleMessager).Select(x => new SampleVM
                {
                    Id_SampleMessager = x.Id_SampleMessager,
                    Question = x.Question,
                    Reply = x.Reply,
                    Status = x.Status,
             
                }).Skip((page - 1) * pagesize).Take(pagesize).ToListAsync();
                return (data, totalPages);
            }
            catch(Exception ex)
            {
                return (null, 0);
            }
        }

        public async Task<SampleVM> GetSamplemessager(int id)
        {
            try
            {
                var data = await _context.SampleMessages.Include(x => x.ButtonSamples).Where(x => x.Id_SampleMessager == id).Select(x => new SampleVM
                {
                    Id_SampleMessager = x.Id_SampleMessager,
                    Question = x.Question,
                    Reply = x.Reply,
                    Status = x.Status,
                    ButtonSamples = x.ButtonSamples.Select(x => new ButtonsampleVM
                    {
                        Id_ButtonSample = x.Id_ButtonSample,
                        Title = x.Title,
                        Link = x.Link
                    }).ToList()
                }).FirstOrDefaultAsync();
                return data;
            }catch(Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> DeleteSamplemessager(int id)
        {
            try
            {
                var data = await _context.SampleMessages.FindAsync(id);
                _context.SampleMessages.Remove(data);
                await _context.SaveChangesAsync();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> UpdateStatus(int id)
        {
            try
            {
                var data = await _context.SampleMessages.FindAsync(id);
                if (data.Status == "Active")
                {
                    data.Status = "InActive";
                    await _context.SaveChangesAsync();
                    return true;
                }
                else
                {
                    data.Status = "Active";
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

        public async Task<List<SampleVM>> GetListSamplemessager()
        {
            try
            {
                var data=await _context.SampleMessages.Include(x => x.ButtonSamples).Select(x => new SampleVM
                {
                    Id_SampleMessager = x.Id_SampleMessager,
                    Question = x.Question,
                    Reply = x.Reply,
                    Status = x.Status,
                    ButtonSamples = x.ButtonSamples.Select(x => new ButtonsampleVM
                    {
                        Id_ButtonSample = x.Id_ButtonSample,
                        Title = x.Title,
                        Link = x.Link
                    }).ToList()
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
