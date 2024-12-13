using Microsoft.AspNetCore.Identity;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.SignalR;
using websitebenhvien.Data;
using websitebenhvien.Models.Enitity;

namespace websitebenhvien.Helper
{
    public class Hubnot :Hub
    {
        private readonly IHubContext<Hubnot> _hubContext;
        private readonly MyDbcontext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;
        private static ConcurrentDictionary<string, string> OnlineUsers = new ConcurrentDictionary<string, string>();
        public Hubnot(MyDbcontext context, IHubContext<Hubnot> hubContext, IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager)
        {
            _hubContext = hubContext;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }
        public async Task SendNotification()
        {
            await _hubContext.Clients.All.SendAsync("ReceiveNotification");
        }

    }
}
