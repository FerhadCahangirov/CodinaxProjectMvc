using System.Net.Mail;
using System.Net;
using CodinaxProjectMvc.Business.Abstract.InfrastructureServices;

namespace CodinaxProjectMvc.Business.InfrastructureServices
{
    public class MailSender : IMailSender
    {
        public Task SendEmailAsync(string from, string to, string pwd, string subject, string message, bool isHtml = false)
        {
            var client = new SmtpClient("codinax.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(from, pwd)
            };

            return client.SendMailAsync(
                new MailMessage(from: from,
                                to: to,
                                subject,
                                message
                                )
                {
                    IsBodyHtml = isHtml
                });
        }
    }
}
