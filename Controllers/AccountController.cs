using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBurgelo.Models;

namespace WebBurgelo.Controllers;
[Route("/[action]")]
public class AccountController : Controller
{
    // private readonly ILogger<AccountController> _logger;
    private readonly IWebHostEnvironment _env;
    private readonly BurgeloContext _burgeloContext;
    private readonly AccountService _accountService;
    private readonly CartService _cartService;
    private readonly OrderService _orderService;
    private SendMailService _sendMailService;

    public AccountController(IWebHostEnvironment env, BurgeloContext burgeloContext, AccountService accountService, CartService cartService, SendMailService sendMailService, OrderService orderService)
    {
        // _logger = logger;
        _env = env;
        _burgeloContext = burgeloContext;
        _accountService = accountService;
        _cartService = cartService;
        _sendMailService = sendMailService;
        _orderService = orderService;
    }
    [TempData]
    public string message { set; get; }
    public AccountModel loggedAccount { set; get; }
    [HttpGet]
    [Route("/login")]
    public IActionResult Login()
    {
        return View();
    }
    [HttpGet]
    [Route("/logincheck")]
    public async Task<IActionResult> LoginCheck([Bind("AccountName", "Password")] LoginModel model)
    {
        if (ModelState.IsValid)
        {
            var account = await (from u in _burgeloContext.accounts where u.AccountName == model.AccountName && u.Password == model.Password select u).FirstOrDefaultAsync();
            if (account != null)
            {
                if (loggedAccount == null)
                {
                    loggedAccount = _accountService.GetAccountInfo();
                    if (loggedAccount.AccountId == 0)
                    {
                        {
                            // Login
                            loggedAccount = account;
                            _accountService.SaveAccountSession(loggedAccount);
                            Console.WriteLine("Login successfully");
                        }
                        {
                            //Get Order Info
                            var orders = await (from o in _burgeloContext.orders where o.UserId == account.UserId && o.DeliveryStatus < 2 select o).ToListAsync();
                            _orderService.SaveOrderSession(orders);
                        }
                        return RedirectToAction(nameof(LoginSuccess));
                    }
                    else RedirectToAction(nameof(UserError));
                }
                else
                {
                    return RedirectToAction(nameof(UserError));
                }
            }
            else
            {
                message = "Wrong User Name or Password. Try again";
                Console.WriteLine(message);
                return RedirectToAction("Login", "Account");
            }
        }
        else
        {
            var m = string.Join(" | ", ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage));
            Console.WriteLine(m);
            return View("Login");
        }
        return RedirectToAction(nameof(UserError));
    }
    public IActionResult LoginSuccess()
    {
        return View();
    }
    [HttpGet]
    [Route("/forgotpassword")]
    public IActionResult ForgotPassword()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> ForgotPasswordSendMail([FromForm][Bind("Email")] ForgotPasswordModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await (from u in _burgeloContext.users where u.Email == model.Email select u).FirstOrDefaultAsync();
            if (user is not null)
            {
                var verifyE = await (from v in _burgeloContext.verifyEmails where v.UserId == user.UserId select v).FirstOrDefaultAsync();
                if (verifyE is null)
                {
                    verifyE = new VerifyEmailModel();
                }
                var random = new Random();
                const string chars = "0123456789";
                verifyE.UserId = user.UserId;
                verifyE.ExpirationTime = DateTime.Now.AddMinutes(5);
                verifyE.Code = new string(Enumerable.Repeat(chars, 6).Select(s => s[random.Next(s.Length)]).ToArray());
                verifyE.user = user;
                Console.WriteLine(verifyE.Code);
                Console.WriteLine(verifyE.UserId);
                _burgeloContext.verifyEmails.Update(verifyE);
                await _burgeloContext.SaveChangesAsync();
                MailContent content = new MailContent
                {
                    To = model.Email,
                    Subject = "Your Verify Code: " + verifyE.Code,
                    Body = "Your Verify code: <br>" + verifyE.Code
                };
                var success = await _sendMailService.SendMail(content);
                return RedirectToAction(nameof(ForgotPasswordCode), verifyE);
            }
            else
            {
                message = "Email doesn't exit";
                Console.WriteLine(message);
                return RedirectToAction("ForgotPassword", "Account");
            }
        }
        else
        {
            var m = string.Join(" | ", ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage));
            Console.WriteLine(m);
            return View(nameof(ForgotPassword));
        }
    }
    [HttpGet]
    public async Task<IActionResult> ForgotPasswordCode(VerifyEmailModel verifyE)
    {
        // Console.WriteLine(verifyE.UserId);
        return View();

    }
    [HttpPost]
    public async Task<IActionResult> ForgotPasswordCodeConfirm([FromForm][Bind("VerifyEmailId", "Code")] VerifyEmailModel model)
    {
        if (ModelState.IsValid)
        {
            Console.WriteLine("Model:True");
            var verifyE = await (from v in _burgeloContext.verifyEmails where v.VerifyEmailId == model.VerifyEmailId select v).FirstOrDefaultAsync();
            if (verifyE is not null)
            {
                if (verifyE.ExpirationTime < DateTime.Now)
                {
                    message = "Your code has been expired";
                    TempData["verifyE"] = verifyE;
                    return RedirectToAction(nameof(ForgotPasswordCode), verifyE);
                }
                if (verifyE.Code != model.Code)
                {
                    message = "Wrong code";
                    return RedirectToAction(nameof(ForgotPasswordCode), verifyE);
                    // return View("ForgotPasswordCode", verifyE);
                }
                ResetPasswordModel resetPassword = new ResetPasswordModel();
                resetPassword.UserID = verifyE.UserId;
                return View("ResetPassword", resetPassword);
            }
            else
            {
                Console.WriteLine("Some thing wrong");
                return RedirectToAction(nameof(UserError));
            }
        }
        else
        {
            Console.WriteLine("Model:False");
            var m = string.Join(" | ", ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage));
            Console.WriteLine(m);
            return View(nameof(ForgotPasswordCode));
        }
    }
    [HttpGet]
    public async Task<IActionResult> ResetPassword(ResetPasswordModel resetPassword)
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> ResetPasswordConfirm(ResetPasswordModel model)
    {
        if (ModelState.IsValid)
        {
            if (model.NewPassword == model.ConfirmNewPassword)
            {
                var account = await (from a in _burgeloContext.accounts where a.UserId == model.UserID select a).FirstOrDefaultAsync();
                if (account is not null)
                {
                    account.Password = model.NewPassword;
                    _burgeloContext.Update(account);
                    await _burgeloContext.SaveChangesAsync();
                    return RedirectToAction(nameof(ResetPasswordSuccess));
                }
                else
                {
                    return RedirectToAction(nameof(UserError));
                }
            }
            else
            {
                message = "Confirm password must match";
                return View("ResetPassword", model);
            }
        }
        else
        {
            Console.WriteLine("Model Fail");
            var m = string.Join(" | ", ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage));
            Console.WriteLine(m);
            return View("ResetPassword", model);
        }
    }
    public IActionResult ResetPasswordSuccess()
    {
        return View();
    }
    public IActionResult ToRegister()
    {
        RegisterModel registerModel = new RegisterModel()
        {
            AccountName = "",
            Email = "",
            Password = "",
            PasswordConfirm = ""
        };
        return RedirectToAction(nameof(Register), registerModel);
    }
    [HttpGet]
    [Route("/register")]
    public IActionResult Register(RegisterModel registerModel)
    {
        return View();
    }
    [HttpPost]
    [Route("/registerconfirm")]
    public async Task<IActionResult> RegisterConfirm(RegisterModel registerModel)
    {
        Console.WriteLine(registerModel.AccountName);
        Console.WriteLine(registerModel.Email);
        Console.WriteLine(registerModel.Password);
        Console.WriteLine(registerModel.PasswordConfirm);
        if (ModelState.IsValid)
        {
            Console.WriteLine("Model Oke");
            var checkAccountName = await (from a in _burgeloContext.accounts where a.AccountName == registerModel.AccountName select a).FirstOrDefaultAsync();
            if (checkAccountName != null)
            {
                message = "That username is taken. Try another";
                Console.WriteLine(message);
                return RedirectToAction(nameof(Register), registerModel);
            }
            var checkEmail = await (from a in _burgeloContext.accounts where a.user.Email == registerModel.Email select a).FirstOrDefaultAsync();
            if (checkEmail != null)
            {
                message = "That email is taken. Try another";
                Console.WriteLine(message);
                return RedirectToAction(nameof(Register), registerModel);
            }
            if (registerModel.Password != registerModel.PasswordConfirm)
            {
                message = "Confirm password must match";
                Console.WriteLine(message);
                return RedirectToAction(nameof(Register), registerModel);
            }
            Console.WriteLine("Pass Check");
            // Tạo ra user mới
            UserModel user = new UserModel();
            user.UserName = registerModel.AccountName;
            user.Gender = "Unknow";
            user.DateOfBirth = DateTime.Now;
            user.Email = registerModel.Email;
            user.RoleId = 1;
            user.Address = "";
            user.PhoneNumber = "";
            try
            {
                _burgeloContext.users.Add(user);
                await _burgeloContext.SaveChangesAsync();
                Console.WriteLine("User created");
            }
            catch (Exception e)
            {
                return Content(e.Message);
            }
            user = (from c in _burgeloContext.users where c.Email == registerModel.Email select c).FirstOrDefault();
            // Tạo ra account mới
            if (user != null)
            {
                AccountModel account = new AccountModel();
                account.AccountName = registerModel.AccountName;
                account.Password = registerModel.Password;
                account.UserId = user.UserId;
                try
                {
                    _burgeloContext.accounts.Add(account);
                    await _burgeloContext.SaveChangesAsync();
                    Console.WriteLine("Account created");

                    // {
                    //     // Login
                    //     loggedAccount = account;
                    //     _accountService.SaveAccountSession(loggedAccount);
                    // }

                    return RedirectToAction(nameof(RegisterSuccess));
                }
                catch (Exception e)
                {
                    return Content(e.Message);
                }
            }
            else
            {
                return RedirectToAction(nameof(UserError));
            }

        }
        else
        {
            //Đọc lỗi Model
            Console.WriteLine("Model Fail");
            var m = string.Join(" | ", ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage));
            Console.WriteLine(m);
            return View("Register", registerModel);
        }
        return RedirectToAction(nameof(Register));
    }

    [Route("/user/userinfo")]
    public IActionResult UserInfo()
    {
        var account = _accountService.GetAccountInfo();
        //Console.WriteLine(account.AccountId);
        if (account.AccountId != 0)
        {
            var user = (from c in _burgeloContext.users where c.UserId == account.UserId select c).FirstOrDefault();
            account.user = user;
            return View(account);
        }
        return RedirectToAction("Login", "Account");
    }
    public IActionResult Logout()
    {
        _accountService.Logout();
        _cartService.ClearCart();
        _orderService.ClearOrder();
        return View();
    }
    [Route("/user/changeuserinfo/")]
    [HttpGet]
    public async Task<IActionResult> ChangeUserInfo()
    {
        var account = _accountService.GetAccountInfo();
        // Console.WriteLine("account iD:"+account.AccountId);
        if (account.AccountId != 0)
        {
            UserModel user = await _burgeloContext.users.FindAsync(account.UserId);
            return View(user);
        }
        return RedirectToAction(nameof(UserError));
    }
    [HttpPost]
    public async Task<IActionResult> ChangeUserInfoConfirm(ChangeUserInfoModel model)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var user = _burgeloContext.users.Find(model.UserId);
                user.UserName = model.UserName;
                user.Gender = model.Gender;
                user.DateOfBirth = model.DateOfBirth;
                _burgeloContext.users.Update(user);
                await _burgeloContext.SaveChangesAsync();
                message = "Successful";
                return RedirectToAction(nameof(UserInfo));
            }
            catch (Exception e)
            {
                message = "Some thing wrong. Please try again later" + e;
                return RedirectToAction(nameof(UserInfo));
            }
        }
        else
        {
            var m = string.Join(" | ", ModelState.Values
                            .SelectMany(v => v.Errors)
                            .Select(e => e.ErrorMessage));
            Console.WriteLine(m);
            return RedirectToAction(nameof(ChangeUserInfo));
        }
    }
    [HttpGet]
    [Route("/user/changepassword")]
    public async Task<IActionResult> ChangePassword()
    {
        return View();
    }
    [Route("/user/changepasswordconfirm")]
    [HttpPost]
    public IActionResult ChangePasswordConfirm(ChangePasswordModel model)
    {
        if (ModelState.IsValid)
        {
            Console.WriteLine("Model:True");
            var account = _burgeloContext.accounts.Find(_accountService.GetAccountInfo().AccountId);
            if (account.Password != model.OldPassword)
            {
                message = "Wrong old password";
                return RedirectToAction("ChangePassword", "Account");
            }
            if (model.NewPassword != model.ConfirmNewPassword)
            {
                message = "Wrong confirm password";
                return RedirectToAction(nameof(ChangePassword));
            }
            Console.WriteLine(model.OldPassword);
            Console.WriteLine(model.NewPassword);
            Console.WriteLine(model.ConfirmNewPassword);
            try
            {
                account.Password = model.NewPassword;
                _burgeloContext.accounts.Update(account);
                _burgeloContext.SaveChanges();
                message = "Successful";
                return RedirectToAction(nameof(UserInfo));
            }
            catch (Exception e)
            {
                message = "Some thing wrong";
                return RedirectToAction(nameof(UserInfo));
            }
        }
        else
        {
            //Đọc lỗi Model
            Console.WriteLine("Model:False");
            var m = string.Join(" | ", ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage));
            Console.WriteLine(m);
            return View("ChangePassword");
        }
    }
    [HttpGet]
    [Route("/user/changeemail")]
    public async Task<IActionResult> ChangeEmail()
    {
        var account = _accountService.GetAccountInfo();
        // Console.WriteLine("account iD:"+account.AccountId);
        if (account.AccountId != 0)
        {
            UserModel user = await _burgeloContext.users.FindAsync(account.UserId);
            return View(user);
        }
        return RedirectToAction(nameof(UserError));
    }
    [HttpPost]
    public async Task<IActionResult> ChangeEmailConfirm([Bind("UserId", "Email")] UserModel model)
    {
        var emailCheck = (from e in _burgeloContext.users where model.Email == e.Email select e).FirstOrDefault();
        if (emailCheck != null)
        {
            message = "That email is taken. Try another";
            return RedirectToAction(nameof(ChangeEmail));
        }
        else
        {
            Console.WriteLine(model.UserId);
            Console.WriteLine(model.Email);
            var user = _burgeloContext.users.Find(model.UserId);
            try
            {
                user.Email = model.Email;
                if (user.RoleId == 2)
                {
                    user.RoleId = 1;
                }
                _burgeloContext.users.Update(user);
                await _burgeloContext.SaveChangesAsync();
                message = "Successful";
                return RedirectToAction(nameof(UserInfo));
            }
            catch (Exception e)
            {
                message = "Some thing wrong";
                return RedirectToAction(nameof(UserInfo));
            }
        }

    }
    [HttpGet]
    [Route("/user/changedeliveryinfo")]
    public async Task<IActionResult> ChangeDeliveryInfo()
    {
        var account = _accountService.GetAccountInfo();
        if (account.AccountId != 0)
        {
            UserModel user = await _burgeloContext.users.FindAsync(account.UserId);
            return View(user);
        }
        else
        {
            return RedirectToAction(nameof(UserError));
        }
    }
    [HttpPost]
    public async Task<IActionResult> ChangeDeliveryInfoConfirm([Bind("UserId", "Address", "PhoneNumber")] UserModel model)
    {
        try
        {
            var user = _burgeloContext.users.Find(model.UserId);
            user.Address = model.Address;
            user.PhoneNumber = model.PhoneNumber;
            _burgeloContext.users.Update(user);
            await _burgeloContext.SaveChangesAsync();
            message = "Successful";
            return RedirectToAction(nameof(UserInfo));
        }
        catch (Exception e)
        {
            message = "Some thing wrong";
            return RedirectToAction(nameof(UserInfo));
        }
    }
    [HttpGet]
    public async Task<IActionResult> VerifyEmailSendMail()
    {
        UserModel user = await _burgeloContext.users.FindAsync(_accountService.GetAccountInfo().UserId);
        //Console.WriteLine(user.UserId);
        return View(user);
    }
    [HttpPost]
    public async Task<IActionResult> VerifyEmailSendMailConfirm([Bind("UserId", "Email")] UserModel model)
    {
        VerifyEmailModel verifyE = new VerifyEmailModel();
        int id = 0;
        Console.WriteLine(model.UserId);
        Console.WriteLine(model.Email);
        var checkVerifyExit = await (from c in _burgeloContext.verifyEmails where c.UserId == model.UserId select c).FirstOrDefaultAsync();
        if (checkVerifyExit != null)
        {
            Console.WriteLine(checkVerifyExit.UserId);
            Console.WriteLine(checkVerifyExit.ExpirationTime);
            Console.WriteLine(checkVerifyExit.Code);
            verifyE = checkVerifyExit;
        }
        else
        {
            Console.WriteLine("New");
            verifyE = new VerifyEmailModel();
        }
        try
        {
            var random = new Random();
            const string chars = "0123456789";
            if (id != 0)
            {
                verifyE.VerifyEmailId = id;
            }
            verifyE.UserId = model.UserId;
            verifyE.ExpirationTime = DateTime.Now.AddMinutes(5);
            verifyE.Code = new string(Enumerable.Repeat(chars, 6).Select(s => s[random.Next(s.Length)]).ToArray());
            Console.WriteLine(verifyE.Code);
            _burgeloContext.verifyEmails.Update(verifyE);
            await _burgeloContext.SaveChangesAsync();
            MailContent content = new MailContent
            {
                To = _burgeloContext.users.Find(model.UserId).Email,
                Subject = "Your Verify Code: " + verifyE.Code,
                Body = "Your Verify code: <br>" + verifyE.Code
            };
            var success = await _sendMailService.SendMail(content);
            message = success;
            return RedirectToAction(nameof(VerifyEmail));
        }
        catch (Exception e)
        {
            return RedirectToAction(nameof(VerifyEmail));
        }
    }
    [HttpGet]
    public async Task<IActionResult> VerifyEmail()
    {
        var userid = await (from uid in _burgeloContext.users where uid.UserId == _accountService.GetAccountInfo().UserId select uid.UserId).FirstOrDefaultAsync();
        VerifyEmailModel verifyE = await (from v in _burgeloContext.verifyEmails where v.UserId == userid select v).FirstOrDefaultAsync();
        return View(verifyE);
    }
    public async Task<IActionResult> VerifyEmailConfirm([Bind("VerifyEmailId", "Code")] VerifyEmailModel model)
    {
        Console.WriteLine(model.VerifyEmailId);
        Console.WriteLine(model.Code);
        var verifyE = await (from v in _burgeloContext.verifyEmails where v.VerifyEmailId == model.VerifyEmailId select v).FirstOrDefaultAsync();
        Console.WriteLine(verifyE.Code);
        if (verifyE.Code != model.Code)
        {
            message = "Wrong Code";
            return RedirectToAction(nameof(VerifyEmail));
        }
        if (verifyE.ExpirationTime < DateTime.Now)
        {
            message = "Code Expirated";
            return RedirectToAction(nameof(VerifyEmail));
        }
        try
        {
            var user = _burgeloContext.users.Find(verifyE.UserId);
            user.RoleId = 2;
            _burgeloContext.users.Update(user);
            await _burgeloContext.SaveChangesAsync();
            message = "Email Verified";
            return RedirectToAction(nameof(UserInfo));
        }
        catch (Exception e)
        {
            message = "Something wrong";
            return RedirectToAction(nameof(UserInfo));
        }
    }
    public IActionResult RegisterSuccess()
    {
        return View();
    }
    public IActionResult UserError()
    {
        return View();
    }
}

