using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBurgelo.Models;

namespace WebBurgelo.Areas_Admin_Controllers
{
    [Area("Admin")]
    [Route("/admin/[action]")]
    public class AdminManageController : Controller
    {
        // private readonly ILogger<UserController> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly BurgeloContext _burgeloContext;
        private readonly AccountService _accountService;
        public AdminManageController(IWebHostEnvironment env, BurgeloContext burgeloContext, AccountService accountService)
        {
            _env = env;
            _burgeloContext = burgeloContext;
            _accountService = accountService;
        }
        public async Task<IActionResult> Index()
        {
            var user = await (from u in _burgeloContext.users where u.UserId == _accountService.GetAccountInfo().UserId select u).FirstOrDefaultAsync();
            if (user is not null)
            {
                if (user.RoleId >= 3)
                {
                    return View();
                }
            }
            return RedirectToAction(nameof(AccessDenied));
        }
        public  IActionResult AccessDenied()
        {
            ViewData["Reffer"] = Request.Headers["Referer"].ToString();
            return View();
        }
    }
}