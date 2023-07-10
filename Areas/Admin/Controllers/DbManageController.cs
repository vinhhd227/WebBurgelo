using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBurgelo.Models;

namespace WebBurgelo.Areas_Admin_Controllers
{
    [Area("Admin")]
    [Route("/admin/database-manage/[action]")]
    public class DbManageController : Controller
    {
        private readonly BurgeloContext _dbContext;

        public DbManageController(BurgeloContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult DeleteDb()
        {
            return View();
        }
        [TempData]
        public string message { set; get; }
        [HttpPost]
        public async Task<IActionResult> DeleteDbAsync()
        {
            var success = await _dbContext.Database.EnsureDeletedAsync();
            message = success ? "Database deleted" : "Can't delete database";
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> Migrate()
        {
            await _dbContext.Database.MigrateAsync();
            message = "Database update successfully";
            return RedirectToAction(nameof(Index));
        }
    }
}



