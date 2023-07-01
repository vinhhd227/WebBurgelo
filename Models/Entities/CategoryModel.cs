using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebBurgelo.Models;

[Table("Category")]
public class CategoryModel
{
    [Key]
    [Column("CategoryId")]
    [Display(Name = "Mã danh mục")]
    public int CategoryId { set; get; }
    [Column("CategoryName")]
    [Display(Name = "Tên danh mục")]
    public string? CategoryName { set; get; }
}