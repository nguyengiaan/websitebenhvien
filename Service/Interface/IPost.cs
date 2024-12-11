using System.Threading.Tasks;
using websitebenhvien.Models.Enitity;
using websitebenhvien.Models.EnitityVM;
namespace websitebenhvien.Service.Interface
{
    public interface IPost
    {
        public Task<News> GetNewsById(string alias_url);
    }
}

