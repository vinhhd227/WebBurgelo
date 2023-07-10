using Microsoft.AspNetCore.Mvc;
using WebBurgelo.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Drawing;

namespace WebBurgelo.Areas_Admin_Controllers
{

    [Area("Admin")]
    [Route("/admin/product-manage/[action]")]
    public class ProductManageController : Controller
    {
        private readonly ILogger<ProductManageController> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly BurgeloContext _burgeloContext;
        private readonly AccountService _accountService;
        public ProductManageController(ILogger<ProductManageController> logger, IWebHostEnvironment env, BurgeloContext burgeloContext, AccountService accountService)
        {
            _logger = logger;
            _env = env;
            _burgeloContext = burgeloContext;
            _accountService = accountService;
        }

        [BindProperty]
        public InputProductModel inputProductModel { get; set; }
        [BindProperty]
        public ProductModel productModel { set; get; }
        [TempData]
        public string message { set; get; }
        [HttpGet]
        public async Task<IActionResult> Index(int categoryid = 0)
        {

            var user = await (from u in _burgeloContext.users where u.UserId == _accountService.GetAccountInfo().UserId select u).FirstOrDefaultAsync();
            if (user is not null)
            {
                if (user.RoleId >= 3)
                {
                    {
                        var p = await _burgeloContext.products.ToListAsync();
                        foreach (var item in p)
                        {
                            if (item.Image == null)
                            {
                                StringBuilder sb = new StringBuilder();
                                sb.Append(item.ProductName.ToLower().Replace(" ", "_")).Append(".png");
                                string imgName = sb.ToString();
                                var qr = (from c in _burgeloContext.categories where c.CategoryId == item.CategoryId select c).FirstOrDefault();
                                string ImageUrl = $"content/images/menu/{qr.CategoryName.ToLower()}/{imgName}";
                                byte[] imageArray = Encoding.ASCII.GetBytes(ImageUrl);
                                string base64Image = Convert.ToBase64String(imageArray);
                                item.Image = base64Image;
                                _burgeloContext.products.Update(item);
                            }
                        }
                        await _burgeloContext.SaveChangesAsync();
                    }
                    List<ProductModel> model = new List<ProductModel>();
                    if (categoryid == 0)
                    {
                        var products = await _burgeloContext.products.ToListAsync();
                        model = products;
                    }
                    else
                    {
                        var products = await (from p in _burgeloContext.products where p.CategoryId == categoryid select p).ToListAsync();
                        model = products;
                    }
                    switch (categoryid)
                    {
                        case 0:
                            {
                                ViewData["nav"] = "All";
                                break;
                            }
                        case 1:
                            {
                                ViewData["nav"] = "Burgers";
                                break;
                            }
                        case 2:
                            {
                                ViewData["nav"] = "Pizza";
                                break;
                            }
                        case 3:
                            {
                                ViewData["nav"] = "Drinks";
                                break;
                            }
                        case 4:
                            {
                                ViewData["nav"] = "Pasta";
                                break;
                            }
                        case 5:
                            {
                                ViewData["nav"] = "Soup";
                                break;
                            }
                        case 6:
                            {
                                ViewData["nav"] = "Sushi";
                                break;
                            }
                    }
                    return View(model);
                }
            }
            return RedirectToAction(nameof(AccessDenied));
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
                    string base64Image = "";
                    try
                    {
                        // Create Image name
                        StringBuilder sb = new StringBuilder();
                        var random = new Random();
                        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                        string code = new string(Enumerable.Repeat(chars, 9).Select(s => s[random.Next(s.Length)]).ToArray());
                        sb.Append(model.ProductName.ToLower().Replace(" ", "_")).Append(code).Append(".png");
                        string imgName = sb.ToString();
                        // Get Category name
                        var qr = (from c in _burgeloContext.categories where c.CategoryId == model.CategoryId select c).FirstOrDefault();
                        // Create filePath and coppy file to filePath
                        var filePath = Path.Combine(_env.WebRootPath, "content", "images", "menu", qr.CategoryName, imgName);
                        using var fileStream = new FileStream(filePath, FileMode.Create);
                        model.FileUpload.CopyTo(fileStream);
                        // Create Image Url
                        string ImageUrl = $"content/images/menu/{qr.CategoryName.ToLower()}/{imgName}";
                        byte[] imageArray = Encoding.ASCII.GetBytes(ImageUrl);
                        base64Image = Convert.ToBase64String(imageArray);
                        // using (var ms = new MemoryStream())
                        // {
                        //     model.FileUpload.CopyTo(ms);
                        //     var fileBytes = ms.ToArray();
                        //     base64Image = Convert.ToBase64String(fileBytes);
                        //     // act on the Base64 data
                        // }
                        // Console.WriteLine(base64Image);
                        // byte[] imageArray = model.FileUpload.FileBytes()
                        // System.IO.File.ReadAllBytes(System.IO.Path.GetDirectoryName(model.FileUpload.FileName));
                        // base64Image = Convert.ToBase64String(imageArray);
                        //System.IO.File.WriteAllBytes(filePath, Convert.FromBase64String(base64Image));
                    }
                    catch (Exception e)
                    {
                        return Content(e.Message);
                    }
                    ProductModel product = new ProductModel()
                    {
                        ProductName = model.ProductName,
                        Description = model.Description,
                        Price = model.Price,
                        Image = base64Image,
                        CategoryId = model.CategoryId
                    };
                    _burgeloContext.products.Add(product);
                    await _burgeloContext.SaveChangesAsync();
                    message = "Successful";
                }

                return RedirectToAction(nameof(Index));
            }
            else
            {
                message = "Failed Add New Product";
            }
            return View();
        }

        [HttpGet("/admin-manage/product-manage/update/{id}"), ActionName("Update")]
        public IActionResult Update(int id)
        {
            var product = (from p in _burgeloContext.products where p.ProductId == id select p).FirstOrDefault();
            if (product != null)
            {
                return View(product);
            }
            else
            {
                return (Content("SomeThing Wrong :("));
            }
        }
        [HttpPost("/admin-manage/product-manage/update/{id}"), ActionName("Update")]
        public async Task<IActionResult> Update(InputProductModel model)
        {
            message = "";
            Console.WriteLine(ModelState.IsValid);
            if (ModelState.IsValid)
            {
                string base64Image = "";
                if (model.FileUpload != null)
                {
                    try
                    {
                        // Create Image name
                        StringBuilder sb = new StringBuilder();
                        var random = new Random();
                        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                        string code = new string(Enumerable.Repeat(chars, 9).Select(s => s[random.Next(s.Length)]).ToArray());
                        sb.Append(model.ProductName.ToLower().Replace(" ", "_")).Append(code).Append(".png");
                        string imgName = sb.ToString();
                        // Get Category name
                        var qr = (from c in _burgeloContext.categories where c.CategoryId == model.CategoryId select c).FirstOrDefault();
                        // Create filePath 
                        var filePath = Path.Combine(_env.WebRootPath, "content", "images", "menu", qr.CategoryName, imgName);
                        using var fileStream = new FileStream(filePath, FileMode.Create);
                        model.FileUpload.CopyTo(fileStream);
                        // Create Image Url
                        string ImageUrl = $"content/images/menu/{qr.CategoryName.ToLower()}/{imgName}";
                        byte[] imageArray = Encoding.ASCII.GetBytes(ImageUrl);
                        base64Image = Convert.ToBase64String(imageArray);
                    }
                    catch (Exception e)
                    {
                        return Content(e.Message);
                    }
                }
                else
                {

                }
                try
                {
                    var product = _burgeloContext.products.Find(model.ProductId);
                    product.ProductName = model.ProductName;
                    product.Description = model.Description;
                    if (base64Image != "")
                    {
                        product.Image = base64Image;
                    }
                    product.Price = model.Price;
                    product.CategoryId = model.CategoryId;
                    _burgeloContext.products.Update(product);
                    await _burgeloContext.SaveChangesAsync();
                    message = "Update Successful";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception e)
                {
                    return Content(e.Message);
                }
            }
            else
            {
                message = "Update Failed";
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
            message = "Deleted";
            return RedirectToAction(nameof(Index));
        }
        public IActionResult AccessDenied()
        {
            ViewData["Reffer"] = Request.Headers["Referer"].ToString();
            return View();
        }
    }
}