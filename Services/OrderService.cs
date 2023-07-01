using WebBurgelo.Models;
using Newtonsoft.Json;

public class OrderService
{
    // Key lưu chuỗi JSON cuae cart
    public const string ORDERKEY = "order";

    private readonly IHttpContextAccessor _context;
    private readonly HttpContext _httpContext;
    public OrderService(IHttpContextAccessor context)
    {
        _context = context;
        _httpContext = context.HttpContext;
    }

    // Lấy cart từ Session (danh sách OrderModel)
    public List<OrderModel> GetOrderItems()
    {

        var session = _httpContext.Session;
        string jsonorder = session.GetString(ORDERKEY);
        if (jsonorder != null)
        {
            return JsonConvert.DeserializeObject<List<OrderModel>>(jsonorder);
        }
        return new List<OrderModel>();
    }

    // Xóa cart khỏi session
    public void ClearOrder()
    {
        var session = _httpContext.Session;
        session.Remove(ORDERKEY);
    }

    // Lưu Cart (Danh sách OrderModel) vào session
    public void SaveOrderSession(List<OrderModel> ls)
    {
        var session = _httpContext.Session;
        string jsonorder = JsonConvert.SerializeObject(ls);
        session.SetString(ORDERKEY, jsonorder);
    }
}