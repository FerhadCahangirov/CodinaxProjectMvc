using CodinaxProjectMvc.Areas.Auth.Controllers;
using CodinaxProjectMvc.Business.Abstract.InfrastructureServices;
using CodinaxProjectMvc.Constants;
using CodinaxProjectMvc.DataAccess.Models;
using CodinaxProjectMvc.DataAccess.Models.Common;
using CodinaxProjectMvc.DataAccess.Models.Identity;
using CodinaxProjectMvc.Managers.Abstract;
using CodinaxProjectMvc.ViewModel.LayoutVm;

namespace CodinaxProjectMvc.Managers
{
    public class MailManager : IMailManager
    {
        private readonly IMailSender _mailSender;
        private readonly string _domain;
        private const string _area = "Auth";
        private readonly IConfiguration _configuration;

        public MailManager(IMailSender mailSender, IConfiguration configuration)
        {
            _mailSender = mailSender;
            _domain = configuration[ConfigurationStrings.DomainUrl];
            _configuration = configuration;
        }

        public async Task SendConfirmationMailAsync<TEntity>(string token, TEntity user) where TEntity : AppUser
        {
            string controller = "ConfirmMail";
            
            string? confirmationLink = $"{_domain}/{_area}/{controller}?token={Uri.EscapeDataString(token)}&email={user.Email}";

            string message = @$"
                
                Hi {user.FirstName} {user.LastName},
    
                We just need to verify your email address before data send to the administraion.

                Verify your email address {confirmationLink}

                Thanks! – The Codinax team
            ";

            await _mailSender.SendEmailAsync(_configuration[ConfigurationStrings.AccountMailAddr], user.Email, _configuration[ConfigurationStrings.AccountMailPwd], "Codinax Applyment Mail Confirmation", message);
        }

        public async Task SendPasswordSetupMailAsync<TEntity>(string token, TEntity user) where TEntity : AppUser
        {
            string controller = "PasswordSetup";

            string? confirmationLink = $"{_domain}/{_area}/{controller}?token={Uri.EscapeDataString(token)}&email={user.Email}";

            string message = @$"
                
                Hi {user.FirstName} {user.LastName},
   
                Please click this link to redirect to password setup page.

                Setup your password {confirmationLink}

                Thanks! – The Codinax team
            ";

            await _mailSender.SendEmailAsync(_configuration[ConfigurationStrings.AccountMailAddr], user.Email, _configuration[ConfigurationStrings.AccountMailPwd], "Codinax Applyment Mail Confirmation", message);
        }

        public async Task SendContactMailAsync(ContactVm contactVm)
            => await _mailSender.SendEmailAsync(_configuration[ConfigurationStrings.ContactMailAddr],
                                             _configuration[ConfigurationStrings.ContactMailAddr],
                                             _configuration[ConfigurationStrings.ContactMailPwd],
                                             contactVm.Email,
                                             PrepareContactContent(contactVm));


        public async Task SendSubscribeConfirmationMailAsync(string token, string email)
        {
            const string controller = "Subscribe";

            string? confirmationLink = $"{_domain}/{_area}/{controller}?token={token}&email={email}";

            string message = @$"

                Dear Subscriber,

                Thank you for signing up for newsletter! To complete the subscription process and start enjoying all the benefits of our service, please confirm your email address by clicking the link below:

                {confirmationLink}

                By confirming your email, you'll gain access to exclusive content, updates, and special offers tailored just for you.

                If you did not sign up for newsletter, please ignore this email. Your email address will not be subscribed until you confirm by clicking the link above.

                Thank you for choosing newsletter. We're excited to have you on board!

                Best regards,
                Codinax 
            ";

            string subject = $"Confirm Your Subscription to Newsletter";

            await _mailSender.SendEmailAsync(
                _configuration[ConfigurationStrings.InfoMailAddr],
                email,
                _configuration[ConfigurationStrings.InfoMailPwd],
                subject,
                message);
        }

            
        private string PrepareContactContent(ContactVm vm)
         =>
             @$"
                Fullname: {vm.Firstname} {vm.Lastname}
                Email: {vm.Email}
                PhoneNumber: {vm.PhoneNumber}
                Message: 
                {vm.Message}
            ";
    }
}
