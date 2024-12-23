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
    }
}
