using System.ComponentModel.DataAnnotations;

namespace WebBurgelo.Models;
public class LoginModel
{
    [Display(Name = "User Name")]
    [StringLength(100)]
    [Required(ErrorMessage = "Must enter {0}")]
    public string AccountName { set; get; }
    [Display(Name = "Password")]
    [StringLength(100)]
    [Required(ErrorMessage = "Must enter {0}")]
    public string Password { set; get; }
}
public class ForgotPasswordModel
{
    [Display(Name = "Email Address")]
    [StringLength(100)]
    [Required(ErrorMessage = "Enter your email")]
    [EmailAddress]
    public string Email { set; get; }
}
public class RegisterModel
{
    [Display(Name = "User Name")]
    [StringLength(100)]
    [Required(ErrorMessage = "Must enter {0}")]
    [RegularExpression(@"([a-zA-Z\d]+[\w\d]*|)[a-zA-Z]+[\w\d.]*", ErrorMessage = "Invalid username")]
    public string AccountName { set; get; }
    [Display(Name = "Email Address")]
    [StringLength(100)]
    [Required(ErrorMessage = "Must enter {0}")]
    [EmailAddress]
    public string Email { set; get; }
    [Display(Name = "Password")]
    [Required(ErrorMessage = "Must enter {0}")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$", ErrorMessage = "Password must be between 6 and 20 characters and contain one uppercase letter, one lowercase letter, one digit and one special character.")]
    public string Password { set; get; }
    [Display(Name = "Password Confirm")]
    [StringLength(100)]
    [Required(ErrorMessage = "Must enter {0}")]
    public string PasswordConfirm { set; get; }
}
public class ChangeUserInfoModel
{
    public int UserId { set; get; }
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Must have {1} to {0} character")]
    [Required(ErrorMessage = "Must enter {0}")]
    [Display(Name = "Name")]
    public string UserName { set; get; }
    [Display(Name = "Gender")]
    public string Gender { set; get; }
    [Display(Name = "Birth Day")]
    public DateTime DateOfBirth { set; get; }
}
public class ChangePasswordModel
{
    [Display(Name = "Old Password")]
    [StringLength(100)]
    [Required(ErrorMessage = "Must enter {0}")]
    public string OldPassword { set; get; }
    [Display(Name = "New Password")]
    [StringLength(100)]
    [Required(ErrorMessage = "Must enter {0}")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$", ErrorMessage = "Password must be between 6 and 20 characters and contain one uppercase letter, one lowercase letter, one digit and one special character.")]
    public string NewPassword { set; get; }
    [Display(Name = "Confirm New Password")]
    [StringLength(100)]
    [Required(ErrorMessage = "Must enter {0}")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$", ErrorMessage = "Password must be between 6 and 20 characters and contain one uppercase letter, one lowercase letter, one digit and one special character.")]
    public string ConfirmNewPassword { set; get; }
}
public class ResetPasswordModel
{
    public int UserID { set; get; }
    [Display(Name = "New Password")]
    [StringLength(100)]
    [Required(ErrorMessage = "Must enter {0}")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$", ErrorMessage = "Password must be between 6 and 20 characters and contain one uppercase letter, one lowercase letter, one digit and one special character.")]
    public string NewPassword { set; get; }
    [Display(Name = "Confirm New Password")]
    [StringLength(100)]
    [Required(ErrorMessage = "Must enter {0}")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$", ErrorMessage = "Password must be between 6 and 20 characters and contain one uppercase letter, one lowercase letter, one digit and one special character.")]
    public string ConfirmNewPassword { set; get; }
}