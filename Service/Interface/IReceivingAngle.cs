using System.Collections.Generic;
using System.Threading.Tasks;
using websitebenhvien.Models.EnitityVM;

namespace websitebenhvien.Service.Interface
{
    public interface IReceivingAngle
    {
        // Define method signatures here
        public Task<List<ReceivingAngleVM>> GetAllReceivingAnglesAsync(int page, int pageSize, string searchString);

        public Task<ReceivingAngleVM?> GetReceivingAngleByIdAsync(int id);

        public Task<bool> CreateReceivingAngleAsync(ReceivingAngleVM receivingAngle);

        public Task<bool> UpdateReceivingAngleAsync(ReceivingAngleVM receivingAngle);

        public Task<bool> DeleteReceivingAngleAsync(int id);

        public Task<int> GetTotalCountAsync(string? searchString = null);


        public Task<List<ReceivingAngleVM>> GetAllReceivingAnglesAsync();
    }
}