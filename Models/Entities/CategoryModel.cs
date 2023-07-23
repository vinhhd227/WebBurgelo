using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebBurgelo.Models;

[Table("Category")]
public class CategoryModel
{
    [Key]
    [Column("CategoryId")]
    [Display(Name = "Category Id")]
    public int CategoryId { set; get; }
    [Column("CategoryName")]
    [Display(Name = "Category Name")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "{0} must have {2} to {1} characters")]
    public string? CategoryName { set; get; }
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
}