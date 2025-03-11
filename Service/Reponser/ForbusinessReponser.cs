using System.Linq;
using Microsoft.EntityFrameworkCore;
using websitebenhvien.Data;
using websitebenhvien.Models.Enitity;
using websitebenhvien.Models.EnitityVM;
using websitebenhvien.Service.Interface;

namespace websitebenhvien.Service.Reponser
{
    public class ForbusinessReponser : IForbusiness
    {
        private readonly MyDbcontext _context;

        public ForbusinessReponser(MyDbcontext context)
        {
            _context = context;
        }
        public async Task<bool> Addbusiness(Forbusiness forbusiness)
        {
            if(forbusiness.Id_Forbusiness !=0)
            {
                var data = await _context.Forbusinesses.FindAsync(forbusiness.Id_Forbusiness);
                data.Name_Forbusiness = forbusiness.Name_Forbusiness;
                data.Content_Forbusiness = forbusiness.Content_Forbusiness;



      
                await _context.SaveChangesAsync();
                return true;



            }
            else
            {
                forbusiness.Create_at = DateTime.Now;
                forbusiness.Status_Forbusiness = true;
                await _context.Forbusinesses.AddAsync(forbusiness);
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
                        Create_at = x.Create_at
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
