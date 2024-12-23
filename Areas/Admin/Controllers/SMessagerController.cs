using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using websitebenhvien.Service.Interface;

namespace websitebenhvien.Areas.Admin.Controllers
{
    public class SMessagerController : Controller
    {
        private readonly ISamplemessager _samplemessager;

        public SMessagerController(ISamplemessager samplemessager)
        {
            _samplemessager = samplemessager;
        }
        [HttpPost("Them-tin-nhan-mau")]
        [Authorize]

        public IActionResult AddSpmess()
        {
            return View();
        }
    }
}
