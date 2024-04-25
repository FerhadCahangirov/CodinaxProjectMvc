namespace CodinaxProjectMvc.Business.Abstract.InfrastructureServices
{
    public interface IMailSender
    {
        Task SendEmailAsync(string from, string to, string pwd, string subject, string message,bool isHtml = false);
    }
}
