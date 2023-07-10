using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using WebBurgelo.Models;


var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://localhost:5038/");

var services = builder.Services;
// Add services to the container.
// ==========================================
services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;               // Url viết chữ thường
    // options.LowercaseQueryStrings = true;    // Query trong Url viết chữ thường
});
services.AddOptions();
services.AddHttpContextAccessor();
services.Configure<MailSetting>(builder.Configuration.GetSection("MailSetting"));   // Đọc connfig gửi Mail 
services.AddTransient<SendMailService>();                                           // Đăng kí dịch vụ gửi Mail
services.AddTransient<CartService>();                                               // Đăng kí dịch vụ giỏ hàng
services.AddTransient<AccountService>();                                            // Đăng kí dịch vụ user
services.AddTransient<OrderService>();                                            // Đăng kí dịch vụ order
services.AddDbContext<BurgeloContext>(options =>
{
    string connectString = builder.Configuration.GetConnectionString("BurgeloContext");
    options.UseSqlServer(connectString);
    options.UseLazyLoadingProxies();
});
//services.AddSingleton<ProductService>();
services.AddRazorPages();
services.AddControllersWithViews();
services.AddDistributedMemoryCache();               // Đăng ký dịch vụ lưu cache trong bộ nhớ (Session sẽ sử dụng nó)
services.AddSession(options =>
{                                                   // Đăng ký dịch vụ Session
    options.Cookie.Name = "WebBurgelo.Session";                    // Đặt tên Session - tên này sử dụng ở Browser (Cookie)
    options.IdleTimeout = TimeSpan.FromHours(1);       // Thời gian tồn tại của Session
    options.Cookie.IsEssential = true;
});
services.AddHttpContextAccessor();

// ============================================

var contentRootPath = builder.Environment.ContentRootPath; // Thiết lập root

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();

// app.UseAuthentication();
// app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    endpoints.MapRazorPages();
});


app.Run();
