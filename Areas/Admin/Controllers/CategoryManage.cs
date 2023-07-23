// using Microsoft.AspNetCore.Mvc;
// using WebBurgelo.Models;
// using Microsoft.EntityFrameworkCore;
// using System.Text;
// using System.Drawing;

// namespace WebBurgelo.Areas_Admin_Controllers
// {

//     [Area("Admin")]
//     [Route("/admin/category-manage/[action]")]
//     public class CategoryManageController : Controller
//     {
//         private readonly ILogger<CategoryManageController> _logger;
//         private readonly IWebHostEnvironment _env;
//         private readonly BurgeloContext _burgeloContext;
//         private readonly AccountService _accountService;
//         public CategoryManageController(ILogger<CategoryManageController> logger, IWebHostEnvironment env, BurgeloContext burgeloContext, AccountService accountService)
//         {
//             _logger = logger;
//             _env = env;
//             _burgeloContext = burgeloContext;
//             _accountService = accountService;
//         }

//         [TempData]
//         public string message { set; get; }
//         [HttpGet]
//         public async Task<IActionResult> Index(int categoryid = 0)
//         {
//             var user = await (from u in _burgeloContext.users where u.UserId == _accountService.GetAccountInfo().UserId select u).FirstOrDefaultAsync();
//             if (user is not null)
//             {
//                 if (user.RoleId >= 3)
//                 {
//                     {
//                         var model = await _burgeloContext.categories.ToListAsync();
//                         return View(model);
//                     }
//                 }
//             }
//             return RedirectToAction(nameof(AccessDenied));
//         }
//         [HttpPost("/admin-manage/category-manage/update/{id}"), ActionName("Update")]
//         public async Task<IActionResult> Update(InputProductModel model)
//         {
//             return View();
//         }
//         [HttpGet("/admin-manage/category-manage/delete/{id}"), ActionName("Delete")]
//         public IActionResult Delete(int id)
//         {
//             var product = (from p in _burgeloContext.products where p.ProductId == id select p).FirstOrDefault();
//             return View(product);
//         }
//         [HttpPost("/admin-manage/category-manage/delete/{id}"), ActionName("Delete")]
//         [ValidateAntiForgeryToken]
//         public async Task<IActionResult> DeleteConfirm(int id)
//         {
//             var product = await _burgeloContext.products.FindAsync(id);
//             _burgeloContext.products.Remove(product);
//             await _burgeloContext.SaveChangesAsync();
//             message = "Deleted";
//             return RedirectToAction(nameof(Index));
//         }
//         public IActionResult AccessDenied()
//         {
//             ViewData["Reffer"] = Request.Headers["Referer"].ToString();
//             return View();
//         }
//     }
// }