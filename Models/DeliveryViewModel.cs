using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebBurgelo.Models;

public class DeliveryViewModel
{
    public int DeliveryId { set; get; }

    public string DeliveryCode { set; get; }

    public int ShipperId { set; get; } = 0;
    // 0 - Waiting, 1 - Delivering, 2 - Success, 3 - Fail

    public int DeliveryStatus { set; get; } = 0;

    // 0 - Waiting, 1 - Receive,
    public int CustomerConfirm { set; get; }
    public UserModel shipper { set; get; }
}