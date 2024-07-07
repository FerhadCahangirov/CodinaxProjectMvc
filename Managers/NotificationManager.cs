using CodinaxProjectMvc.Business.Abstract.InfrastructureServices;
using CodinaxProjectMvc.Constants;
using CodinaxProjectMvc.DataAccess.Models;
using CodinaxProjectMvc.Managers.Abstract;

namespace CodinaxProjectMvc.Managers
{
    public class NotificationManager : INotificationManager
    {
        private readonly IMailSender _mailSender;
        private readonly IConfiguration _configuration;

        public NotificationManager(IMailSender mailSender, IConfiguration configuration)
        {
            _mailSender = mailSender;
            _configuration = configuration;
        }

        public async Task SendInstructorApplymentNotificationMailAsync(Instructor instructor)
        {
            string subject = $"{instructor.FirstName} {instructor.LastName} has applied to join our Team!";

            string message = $@"
                We're thrilled to announce that {instructor.FirstName} {instructor.LastName} has applied to join our team of instructors at Codianx! With their expertise in {instructor.Profession}, we're excited about the knowledge and experience they'll bring to our community.
            ";

            string info_mail_addr = _configuration[ConfigurationStrings.InfoMailAddr];
            string info_mail_pwd = _configuration[ConfigurationStrings.InfoMailPwd];

            await _mailSender.SendEmailAsync(info_mail_addr, info_mail_addr, info_mail_pwd, subject, message);
        }

        public async Task SendStudentApplymentNotificationMailAsync(Student student)
        {
            string subject = $"{student.FirstName} {student.LastName} has applied to join our Team!";

            string message = $@"
                We're thrilled to announce that {student.FirstName} {student.LastName} has applied to join our team of students at Codianx!
            ";

            string apply_mail_addr = _configuration[ConfigurationStrings.AccountMailAddr];
            string apply_mail_pwd = _configuration[ConfigurationStrings.AccountMailPwd];

            await _mailSender.SendEmailAsync(apply_mail_addr, apply_mail_addr, apply_mail_pwd, subject, message);
        }

        public async Task SendSubscriptionNotificationMailAsync(Subscriber subscriber)
        {
            string subject = $"{subscriber.Email} has subscribed to our Newsletter!";

            string message = $@"
                Stay tuned for the latest updates and exclusive offers - {subscriber.Email} is now part of our Newsletter community! Get ready to receive exciting content straight to your inbox, tailored just for you.
            ";

            string apply_mail_addr = _configuration[ConfigurationStrings.AccountMailAddr];
            string apply_mail_pwd = _configuration[ConfigurationStrings.AccountMailPwd];

            await _mailSender.SendEmailAsync(apply_mail_addr, apply_mail_addr, apply_mail_pwd, subject, message);
        }
    }
}
