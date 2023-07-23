using WebBurgelo.Models;
using Newtonsoft.Json;

public class AccountService
{
    // Key lưu chuỗi JSON cart
    public const string ACCOUNT = "account";

    private readonly IHttpContextAccessor _context;
    private readonly HttpContext _httpContext;
    public AccountService(IHttpContextAccessor context)
    {
        _context = context;
        _httpContext = context.HttpContext;
    }

    // Lấy account từ Session (danh sách CartItem)
    public AccountModel GetAccountInfo()
    {
        var session = _httpContext.Session;
        string jsonAccount = session.GetString(ACCOUNT);
        if (jsonAccount != null)
        {
            return JsonConvert.DeserializeObject<AccountModel>(jsonAccount);
        }
        return new AccountModel();
    }

    // Xóa cart khỏi session
    public void Logout()
    {
        var session = _httpContext.Session;
        session.Remove(ACCOUNT);
    }

    // Lưu account vào session
    public void SaveAccountSession(AccountModel account)
    {
        var session = _httpContext.Session;
        string jsonAccount = JsonConvert.SerializeObject(account);
        session.SetString(ACCOUNT, jsonAccount);
    }
}