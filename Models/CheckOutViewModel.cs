using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace WebBurgelo.Models;
public class CheckOutModel
{
    [Display(Name = "Name")]
    [Required(ErrorMessage = "Must enter {0}")]
    public string CustomerName { set; get; }

    [Display(Name = "Address")]
    [Required(ErrorMessage = "Must enter {0}")]
    public string Address { set; get; }
    [Display(Name = "Phone Number")]
    [Required(ErrorMessage = "Must enter {0}")]
    public string PhoneNumber { set; get; }

    public decimal SubTotal { set; get; }

    [Display(Name = "Payment Method")]
    public int PaymentMethod { set; get; }
    [NotMapped]
    public List<CartItem>? listCartItems { set; get; }
}