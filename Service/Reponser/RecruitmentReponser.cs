using websitebenhvien.Data;
using websitebenhvien.Models.Enitity;
using websitebenhvien.Models.EnitityVM;
using websitebenhvien.Service.Interface;

namespace websitebenhvien.Service.Reponser
{
    public class RecruitmentReponser : IRecruitment
    {
        private readonly MyDbcontext _context;

        public RecruitmentReponser(MyDbcontext context) 
        {
            _context = context;
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
                        data.Statuson = (bool)recruitmentpost.Statuson;
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
    }
}
