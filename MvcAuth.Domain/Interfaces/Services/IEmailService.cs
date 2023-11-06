using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace MvcAuth.Domain.Interfaces.Services;
public interface IEmailService
{
    void ConfirmacaoCadastro(string email);
    void EnviarEmail(MailMessage email);
}
