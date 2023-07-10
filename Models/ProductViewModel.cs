using System.ComponentModel.DataAnnotations;

namespace WebBurgelo.Models;
public class InputProductModel
{
    public int ProductId { set; get; }
    [Display(Name = "Name")]
    [Required(ErrorMessage = "Must enter {0}")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "{0} must have {2} to {1} character")]
    public string? ProductName { set; get; }
    [StringLength(1000)]
    [DataType(DataType.MultilineText)]
    [Display(Name = "Description")]
    [Required(ErrorMessage = "Must enter {0}")]
    public string? Description { set; get; }
    [Display(Name = "Price")]
    [Required(ErrorMessage = "Must enter {0}")]
    [Range(0, 9999, ErrorMessage = "{0} must a positive number")]
    public int Price { get; set; }
    [DataType(DataType.Upload)]
    [Display(Name = "File Upload")]
    public IFormFile? FileUpload { set; get; }
    [Display(Name = "Category")]
    public int CategoryId { get; set; }
}