using CodinaxProjectMvc.DataAccess.Models;

namespace CodinaxProjectMvc.Managers.Abstract
{
    public interface INotificationManager
    {
        Task SendStudentApplymentNotificationMailAsync(Student student);
        Task SendInstructorApplymentNotificationMailAsync(Instructor instructor);
        Task SendSubscriptionNotificationMailAsync(Subscriber subscriber);
    }
}
