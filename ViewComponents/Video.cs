using Microsoft.AspNetCore.Mvc;
using websitebenhvien.Service.Interface;

namespace websitebenhvien.ViewComponents
{
    public class VideoViewComponent : ViewComponent
    {
        private readonly IRecruitment _recruitment;

        public VideoViewComponent(IRecruitment recruitment)
        {
            _recruitment=recruitment;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                var video = await _recruitment.GetVideosClient();
                return View(video);
            }
            catch (Exception ex)
            {
    
                return View(null);
            } 
       }
    }
}