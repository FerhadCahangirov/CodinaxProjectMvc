using CodinaxProjectMvc.Areas.Auth.Controllers;
using CodinaxProjectMvc.Business.Abstract.InfrastructureServices;
using CodinaxProjectMvc.Constants;
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
            string controller = nameof(ConfirmMail);
            
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
            string controller = nameof(PasswordSetup);

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
