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
    [Route("/admin/user-manage/[action]")]
    public class UserManageController : Controller
    {
        // private readonly ILogger<UserController> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly BurgeloContext _burgeloContext;
        private readonly AccountService _accountService;
        public UserManageController(IWebHostEnvironment env, BurgeloContext burgeloContext, AccountService accountService)
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
                if (user.RoleId > 3)
                {
                    var users = await _burgeloContext.users.ToListAsync();
                    return View(users);
                }
            }
            return RedirectToAction(nameof(AccessDenied));
        }
        [HttpPost]
        public IActionResult UpdateUser(int userId, int roleId)
        {
            Console.WriteLine("UserId:" + userId);
            var users = _burgeloContext.users.ToList();
            var user = users.Find(u => u.UserId == userId);
            user.RoleId = roleId;
            _burgeloContext.users.Update(user);
            _burgeloContext.SaveChanges();
            ViewData["Reffer"] = Request.Headers["Referer"].ToString();
            message = "Success";
            return Ok();
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}