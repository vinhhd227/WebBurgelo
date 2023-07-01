using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebBurgelo.Models;

[Table("Contact")]
public class ContactModel
{
    [Key]
    [Column("ContactId")]
    public int ContactId { set; get; }
    [Column("CustomerName")]
    [Display(Name = "Tên khách hàng")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Chiều dài không chính xác")]
    [Required(ErrorMessage="Phải có tên")]
    public string? CustomerName { set; get; }
    [Column("Email")]
    [EmailAddress]
    [StringLength(100)]
    [Required(ErrorMessage="Phải có địa chỉ Email")]
    public string? Email { set; get; }
    public ContactModel()
    {
        // ConId = conId;
        // CustomerName = customerName;
        // Email = email;
    }
    public ContactModel(int contactId, string customerName, string email)
    {
        ContactId = contactId;
        CustomerName = customerName;
        Email = email;
    }
}