using System.Linq;
using Microsoft.EntityFrameworkCore;
using websitebenhvien.Data;
using websitebenhvien.Helper;
using websitebenhvien.Models.Enitity;
using websitebenhvien.Models.EnitityVM;
using websitebenhvien.Service.Interface;

namespace websitebenhvien.Service.Reponser
{
    public class ForbusinessReponser : IForbusiness
    {
        private readonly MyDbcontext _context;
        private readonly Uploadfile _uploadfile;

        public ForbusinessReponser(MyDbcontext context,Uploadfile uploadfile)
        {
            _context = context;
            _uploadfile=uploadfile;
        }
        public async Task<bool> Addbusiness(ForbusinessVM forbusiness)
        {
            if(forbusiness.Id_Forbusiness !=0)
            {
                var data = await _context.Forbusinesses.FindAsync(forbusiness.Id_Forbusiness);
                data.Name_Forbusiness = forbusiness.Name_Forbusiness;
                data.Content_Forbusiness = forbusiness.Content_Forbusiness;
                data.link_Forbusiness = forbusiness.link_Forbusiness;
                data.link_Forbusiness_1 = forbusiness.link_Forbusiness_1;
                if(forbusiness.formFile != null)
                {
                  var file= await _uploadfile.SaveMedia(forbusiness.formFile);
                    if(file.Item1== 1)
                    {
                        _uploadfile.DeleteMedia(data.Icon_Forbusiness);
                        data.Icon_Forbusiness = file.Item2;
                    }
                }
                await _context.SaveChangesAsync();
                return true;



            }
            else
            {
                var forbusiness1 = new Forbusiness();
                forbusiness1.Name_Forbusiness = forbusiness.Name_Forbusiness;
                forbusiness1.Content_Forbusiness = forbusiness.Content_Forbusiness;
                forbusiness1.link_Forbusiness = forbusiness.link_Forbusiness;
                forbusiness1.link_Forbusiness_1 = forbusiness.link_Forbusiness_1;
                if (forbusiness.formFile != null)
                {
                    var file = await _uploadfile.SaveMedia(forbusiness.formFile);
                    if (file.Item1 == 1)
                    {
                        forbusiness1.Icon_Forbusiness = file.Item2;
                    }
                }


                forbusiness1.Create_at = DateTime.Now;
                forbusiness1.Status_Forbusiness = true;
                await _context.Forbusinesses.AddAsync(forbusiness1);
                await _context.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> Deletebsn(int id)
        {
            try
            {
                var data = await _context.Forbusinesses.FindAsync(id);
                if(data==null)
                {
                    return false;
                }
              _uploadfile.DeleteMedia(data.Icon_Forbusiness);
        
              _context.Forbusinesses.Remove(data);
                await _context.SaveChangesAsync();
                return true;
            }catch(Exception ex)
            {
                return false;
            }
        }

        public async Task<(int Totalpage, List<Forbusiness> list)> Getbusiness(string search, int page, int pagesize)
        {
            try
            {
                IQueryable<Forbusiness> query = _context.Forbusinesses.AsNoTracking();

                if (!string.IsNullOrEmpty(search))
                {
                    query = query.Where(x => x.Name_Forbusiness.Contains(search));
                }

                var totalItems = await query.CountAsync();
                var totalPages = (int)Math.Ceiling(totalItems / (double)pagesize);

                var data = await query
                    .OrderByDescending(x => x.Create_at)
                    .Skip((page - 1) * pagesize)
                    .Take(pagesize)
                    .Select(x => new Forbusiness
                    {
                        Id_Forbusiness = x.Id_Forbusiness,
                        Name_Forbusiness = x.Name_Forbusiness,
                        Status_Forbusiness = x.Status_Forbusiness,
                        Create_at = x.Create_at,
                        Icon_Forbusiness=x.Icon_Forbusiness,
                    })
                    .ToListAsync();

                return (totalPages, data);
            }
            catch (Exception)
            {
                return (0, null);
            }
        }


        public async Task<Forbusiness> Getbusinessbyid(int id)
        {
            try
            {
                var data = await _context.Forbusinesses.FindAsync(id);
                return data;
            }
            catch (Exception)
            {
                return null;


            }
        }



        public async Task<bool> Updatebusiness(int id)
        {
            try
            {
                var data = await _context.Forbusinesses.FindAsync(id);
                if (data == null)
                {
                    return false;
                }
                data.Status_Forbusiness = !data.Status_Forbusiness;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<List<Forbusiness>> Getbusinesstrue()
        {
            try
            {
                IQueryable<Forbusiness> query = _context.Forbusinesses.AsNoTracking();
                query = query.Where(x => x.Status_Forbusiness == true);
                return query.ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
