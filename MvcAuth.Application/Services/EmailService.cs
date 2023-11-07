using MvcAuth.Application.Constants;
using MvcAuth.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace MvcAuth.Application.Services;
public class EmailService : IEmailService
{
    public void ConfirmacaoCadastro(string email, int codigoConfirmacao)
    {
        var message = new MailMessage(EmailConstants.Email, email);

        message.Subject = "Confirmação de Cadastro";
        message.Body = $"Seu código de confirmação é: {codigoConfirmacao}";
        message.IsBodyHtml = false;

        EnviarEmail(message);
    }

    public void EnviarEmail(MailMessage message)
    {
        SmtpClient smtpClient = new SmtpClient("smtp.office365.com", 587);
        smtpClient.EnableSsl = true;
        smtpClient.Timeout = 50000;
        smtpClient.UseDefaultCredentials = false;
        smtpClient.Credentials = new NetworkCredential(EmailConstants.Email, EmailConstants.Password);
        smtpClient.Send(message);
        smtpClient.Dispose();
    }
}
