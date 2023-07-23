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
    [Required(ErrorMessage = "Must enter {0}")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "{0} must have {2} to {1} characters")]
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
    [Column("CreateBy")]
    [StringLength(100)]
    public string? CreateBy { set; get; }
    [Column("CreateDate")]
    public DateTime? CreateDate { set; get; }
    [Column("UpdateBy")]
    [StringLength(100)]
    public string? UpdateBy { set; get; }
    [Column("UpdateDate")]
    public DateTime? UpdateDate { set; get; }
    //Foreign key
    [ForeignKey("CategoryId")]
    //[Required]
    public virtual CategoryModel? category { set; get; }
}