using Microsoft.AspNetCore.Mvc;
using websitebenhvien.Service.Interface;

namespace websitebenhvien.Controllers
{
    public class PartialViewController : Controller
    {
        private readonly IPageMain _pagemain;

        public PartialViewController(IPageMain pageMain) 
        {
            _pagemain = pageMain;

        }  
        
        public IActionResult Headertrangchu()
        {
            return PartialView("Headertrangchu");
        }
    }
}
