using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebBurgelo.Models;

[Table("Product")]
public class ProductModel
{
    [Key]
    [Column("ProductId")]
    public int ProductId { set; get; }
    [Column("ProductName")]
    [Display(Name = "Name")]
    [Required(ErrorMessage = "Phải nhập {0}")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "{0} phải có từ {2} đến {1} kí tự")]
    public string? ProductName { set; get; }
    [StringLength(1000)]
    [Column("Description")]
    [Display(Name = "Description")]
    public string? Description { set; get; }
    [Column("Price")]
    [Display(Name = "Price")]
    public int Price { set; get; }
    [Column("Image")]
    [Display(Name = "Image")]
    public string? Image { set; get; }
    [Column("CategoryId")]
    [Display(Name = "CategoryId")]
    public int CategoryId { set; get; }
    //Foreign key
    [ForeignKey("CategoryId")]
    //[Required]
    public virtual CategoryModel? category { set; get; }
}