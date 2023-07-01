using Microsoft.AspNetCore.Mvc;
using WebBurgelo.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace MVC.Areas_Admin_Controllers
{

    [Area("Admin")]
    [Route("/admin/product-manage/[action]")]
    public class ProductManageController : Controller
    {
        private readonly ILogger<ProductManageController> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly BurgeloContext _burgeloContext;
        public ProductManageController(ILogger<ProductManageController> logger, IWebHostEnvironment env, BurgeloContext burgeloContext)
        {
            _logger = logger;
            _env = env;
            _burgeloContext = burgeloContext;
        }
        
        [BindProperty]
        public InputProductModel inputProductModel { get; set; }
        [BindProperty]
        public ProductModel productModel { set; get; }
        [TempData]
        public string message { set; get; }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var products = await _burgeloContext.products.ToListAsync();
            return View(products);
        }
        [HttpGet]
        public IActionResult Read(int id)
        {

            var product = (from p in _burgeloContext.products where p.ProductId == id select p).FirstOrDefault();
            return View(product);
        }
        [HttpGet]
        public IActionResult Create()
        {
            // var lsProducts = new List<ProductModel>();
            // _burgeloContext.products.ToList().ForEach(product =>
            // {
            //     lsProducts.Add(new ProductModel(product.ProId, product.Name, product.Description, product.Price, product.CateId));
            // });
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(InputProductModel model) // [Bind("FileUpload")] IFormFile FileUpload
        {
            message = "";
            Console.WriteLine(ModelState.IsValid);
            if (ModelState.IsValid)
            {
                if (model.FileUpload != null)
                {
                    try
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.Append(model.ProductName.ToLower().Replace(" ", "_")).Append(".png");
                        string imgName = sb.ToString();
                        var qr = (from c in _burgeloContext.categories where c.CategoryId == model.CategoryId select c).FirstOrDefault();
                        var filePath = Path.Combine(_env.WebRootPath, "content", "images", "menu", qr.CategoryName, imgName);
                        using var fileStream = new FileStream(filePath, FileMode.Create);
                        model.FileUpload.CopyTo(fileStream);
                    }
                    catch (Exception e)
                    {
                        return Content(e.Message);
                    }
                }
                ProductModel product = new ProductModel()
                {
                    ProductName = model.ProductName,
                    Description = model.Description,
                    Price = model.Price,
                    CategoryId = model.CategoryId
                };
                _burgeloContext.products.Add(product);
                await _burgeloContext.SaveChangesAsync();
                message = "Thêm món ăn mới thành công";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                message = "Thêm món ăn thất bại";
            }
            return View();
        }
        
        [HttpGet("/admin-manage/product-manage/update/{id}"), ActionName("Update")]
        public IActionResult Update(int id)
        {
            var product = (from p in _burgeloContext.products where p.ProductId == id select p).FirstOrDefault();
            if(product!=null)
            {
                  return View(product);
            }
            else
            {
                return(Content("SomeThing Wrong :("));
            }
        }
        [HttpPost("/admin-manage/product-manage/update/{id}"), ActionName("Update")]
        public async Task<IActionResult> Update(InputProductModel model)
        {
            message = "";
            Console.WriteLine(ModelState.IsValid);
            if (ModelState.IsValid)
            {
                if (model.FileUpload != null)
                {
                    try
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.Append(model.ProductName.ToLower().Replace(" ", "_")).Append(".png");
                        string imgName = sb.ToString();
                        var qr = (from c in _burgeloContext.categories where c.CategoryId == model.CategoryId select c).FirstOrDefault();
                        var filePath = Path.Combine(_env.WebRootPath, "content", "images", "menu", qr.CategoryName, imgName);
                        using var fileStream = new FileStream(filePath, FileMode.Create);
                        await model.FileUpload.CopyToAsync(fileStream);
                    }
                    catch (Exception e)
                    {
                        return Content(e.Message);
                    }
                }
                try
                {
                    var product = _burgeloContext.products.Find(model.ProductId);
                    product.ProductName = model.ProductName;
                    product.Description = model.Description;
                    product.Price = model.Price;
                    product.CategoryId = model.CategoryId;
                    _burgeloContext.products.Update(product);
                    await _burgeloContext.SaveChangesAsync();
                    message = "Sửa thông tin thành công";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception e)
                {
                    return Content(e.Message);
                }
            }
            else
            {
                message = "Sửa thông tin thất bại";
            }
            return View();
        }
        [HttpGet("/admin-manage/product-manage/delete/{id}"), ActionName("Delete")]
        public IActionResult Delete(int id)
        {
            var product = (from p in _burgeloContext.products where p.ProductId == id select p).FirstOrDefault();
            return View(product);
            // if (id == null)
            // {
            //     return NotFound();
            // }
            // ProductModel product = _burgeloContext.products.Find(id);
            // if (product == null)
            // {
            //     return NotFound();
            // }
            // ProductModel delModel = new ProductModel(product.ProId, product.Name, product.Description, product.Price, product.CateId, product.Category);
            // return View(delModel);
        }
        [HttpPost("/admin-manage/product-manage/delete/{id}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            var product = await _burgeloContext.products.FindAsync(id);
            _burgeloContext.products.Remove(product);
            await _burgeloContext.SaveChangesAsync();
            message = "Xóa thành công";
            return RedirectToAction(nameof(Index));
        }
    }
}