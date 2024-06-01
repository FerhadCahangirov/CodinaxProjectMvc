using CodinaxProjectMvc.ViewModel.EventVm;

namespace CodinaxProjectMvc.Business.Abstract.PersistenceServices
{
    public interface IEventService
    {
        Task<bool> EventAddOrUpdateAsync(EventActionVm eventActionVm);

        Task<IEnumerable<EventListItemVm>> ListEventsAsync();

        Task<bool> DeleteEventAsync(Guid id);

        Task<IEnumerable<EventListItemVm>> ListStudentEventsAsync();
    }
}
