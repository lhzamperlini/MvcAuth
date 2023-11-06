using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace MvcAuth.Domain.Models;
public class Email
{
    public string Provedor { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public Email(string provedor, string username, string password)
    {
        Provedor = provedor;
        Username = username;
        Password = password;
    }

    public void SendMail(string mailTo, string subject, string body)
    {
        var mail = PreparateMessage(mailTo, subject, body);

    }

    private MailMessage PreparateMessage(string mailTo, string subject, string body)
    {
        //add mail validator

        var mail = new MailMessage();
        mail.From = new MailAddress(Username);
        mail.To.Add(new MailAddress(mailTo));
        mail.Subject = subject;
        mail.Body = body;

        return mail;
        //mail.IsBodyHtml = true;
    }

    private async void SendMailBySMTP(MailMessage message)
    {
        SmtpClient smtpClient = new SmtpClient("smtp.office365.com");
        smtpClient.Port = 587;
        smtpClient.EnableSsl = true;
        smtpClient.Timeout = 5000;
        smtpClient.UseDefaultCredentials = false;
        smtpClient.Credentials = new NetworkCredential(Username, Password);

        smtpClient.Send(message);
        smtpClient.Dispose();
    }

}
