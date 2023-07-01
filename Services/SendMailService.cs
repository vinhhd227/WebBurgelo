using MimeKit;
using MailKit.Security;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
// public class MailUltis
// {
//     // Gửi bằng local (Linux)
//     public static async Task<string> SendMail(string _form, string _to, string _subject, string _body)
//     {
//         MailMessage message = new MailMessage(_form, _to, _subject, _body);
//         message.BodyEncoding = System.Text.Encoding.UTF8;
//         message.SubjectEncoding = System.Text.Encoding.UTF8;
//         message.IsBodyHtml = true;

//         message.ReplyToList.Add(new MailAddress(_form));
//         message.Sender = new MailAddress(_form);

//         using var smtpClient = new SmtpClient("localhost");

//         try
//         {
//             await smtpClient.SendMailAsync(message);
//             return "Email sent";
//         }
//         catch (Exception e)
//         {
//             Console.WriteLine(e.Message);
//             return "Gửi Email thất bại T.T";
//         }
//     }
//     // Gửi bằng Gmail
//     public static async Task<string> SendGmail(string _form, string _to, string _subject, string _body, string _gmail, string _password)
//     {
//         MailMessage message = new MailMessage(_form, _to, _subject, _body);
//         message.BodyEncoding = System.Text.Encoding.UTF8;
//         message.SubjectEncoding = System.Text.Encoding.UTF8;
//         message.IsBodyHtml = true;

//         message.ReplyToList.Add(new MailAddress(_form));
//         message.Sender = new MailAddress(_form);

//         using var smtpClient = new SmtpClient("smtp.gmail.com");
//         smtpClient.Port = 587;
//         smtpClient.EnableSsl = true;
//         smtpClient.Credentials = new NetworkCredential(_gmail, _password);
//         try
//         {
//             await smtpClient.SendMailAsync(message);
//             return "Email sent";
//         }
//         catch (Exception e)
//         {
//             Console.WriteLine(e.Message);
//             return "Gửi Email thất bại T.T";
//         }
//     }
// }

public class SendMailService
{
    MailSetting _mailSetting { set; get; }
    public SendMailService(IOptions<MailSetting> mailSetting)
    {
        _mailSetting = mailSetting.Value;
    }
    public async Task<string> SendMail(MailContent mailContent)
    {
        var email = new MimeMessage();

        email.Sender = new MailboxAddress(_mailSetting.DisplayName, _mailSetting.Mail);
        email.From.Add(new MailboxAddress(_mailSetting.DisplayName, _mailSetting.Mail));
        email.To.Add(new MailboxAddress(mailContent.To, mailContent.To));
        email.Subject = mailContent.Subject;

        var builder = new BodyBuilder();

        builder.HtmlBody = mailContent.Body;
        email.Body = builder.ToMessageBody();

        using var smtp = new MailKit.Net.Smtp.SmtpClient();

        try
        {
            await smtp.ConnectAsync(_mailSetting.Host, _mailSetting.Port, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_mailSetting.Mail, _mailSetting.Password);
            await smtp.SendAsync(email);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return "Error:" + e.Message;
        }

        smtp.Disconnect(true);
        return "Email Sent. Check your mail";
    }
}

public class MailSetting
{
    public string Mail { set; get; }
    public string DisplayName { set; get; }
    public string Password { set; get; }
    public string Host { set; get; }
    public int Port { set; get; }

}
public class MailContent
{
    public string To { set; get; }
    public string Subject { set; get; }
    public string Body { set; get; }

}