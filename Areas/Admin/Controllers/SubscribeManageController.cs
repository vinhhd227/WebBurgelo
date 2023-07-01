using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBurgelo.Models;

namespace MVC.Areas_Admin_Controllers
{

    [Area("Admin")]
    [Route("/admin/subscribe-manage/[action]")]
    public class SubscribeManageController : Controller
    {
        // private readonly ILogger<UserController> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly BurgeloContext _burgeloContext;
        private readonly AccountService _accountService;
        public SubscribeManageController(IWebHostEnvironment env, BurgeloContext burgeloContext, AccountService accountService)
        {
            _env = env;
            _burgeloContext = burgeloContext;
            _accountService = accountService;
        }
        [TempData]
        string message { set; get; }
        public async Task<IActionResult> Index()
        {
            var user = await (from u in _burgeloContext.users where u.UserId == _accountService.GetAccountInfo().UserId select u).FirstOrDefaultAsync();
            if (user is not null)
            {
                if (user.RoleId >= 3)
                {
                    var subscribes = await _burgeloContext.subscribes.ToListAsync();
                    return View(subscribes);
                }
            }
            return RedirectToAction(nameof(AccessDenied));
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}