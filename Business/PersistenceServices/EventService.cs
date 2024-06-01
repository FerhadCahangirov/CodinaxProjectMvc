using CodinaxProjectMvc.Business.Abstract.PersistenceServices;
using CodinaxProjectMvc.DataAccess.Abstract.Repositories;
using CodinaxProjectMvc.DataAccess.Models;
using CodinaxProjectMvc.DataAccess.Models.Identity;
using CodinaxProjectMvc.ViewModel.EventVm;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Security.Policy;

namespace CodinaxProjectMvc.Business.PersistenceServices
{
    public class EventService : IEventService
    {
        private readonly IReadRepository<Event> _eventReadRepository;
        private readonly IReadRepository<Course> _courseReadRepository;
        private readonly IReadRepository<Instructor> _instructorReadRepository;
        private readonly IWriteRepository<Event> _eventWriteRepository;
        private readonly IReadRepository<Student> _studentReadRepository;

        private readonly IActionContextAccessor _actionContextAccessor;

        public EventService(IReadRepository<Event> eventReadRepository, IWriteRepository<Event> eventWriteRepository, IActionContextAccessor actionContextAccessor, IReadRepository<Instructor> instructorReadRepository, IReadRepository<Course> courseReadRepository, IReadRepository<Student> studentReadRepository)
        {
            _eventReadRepository = eventReadRepository;
            _eventWriteRepository = eventWriteRepository;
            _actionContextAccessor = actionContextAccessor;
            _instructorReadRepository = instructorReadRepository;
            _courseReadRepository = courseReadRepository;
            _studentReadRepository = studentReadRepository;
        }

        public async Task<bool> EventAddOrUpdateAsync(EventActionVm eventActionVm)
        {
            if (eventActionVm.EventId == null)
            {
                Event _event = new Event()
                {
                    StartDate = eventActionVm.StartDate,
                    EndDate = eventActionVm.EndDate,
                    Title = eventActionVm.Title,
                    Url = eventActionVm.Url,
                    BackgroundColor = eventActionVm.BackgroundColor,
                    TextColor = eventActionVm.TextColor,
                    Course = await _courseReadRepository.GetSingleAsync(x => x.Id == eventActionVm.CourseId),
                    Instructor = ActiveInstructor
                };

                await _eventWriteRepository.AddAsync(_event);
                await _eventWriteRepository.SaveAsync();

                return true;
            }
            else
            {
                Event? _event = await _eventReadRepository.Table
                    .Include(x => x.Instructor).Include(x => x.Course)
                    .FirstOrDefaultAsync(x => x.Id == eventActionVm.EventId);

                if (_event == null) return false;

                _event.StartDate = eventActionVm.StartDate;
                _event.EndDate = eventActionVm.EndDate;
                _event.Title = eventActionVm.Title;
                _event.Url = eventActionVm.Url;
                _event.BackgroundColor = eventActionVm.BackgroundColor;
                _event.TextColor = eventActionVm.TextColor;
                _event.Course = await _courseReadRepository.GetSingleAsync(x => x.Id == eventActionVm.CourseId);


                _eventWriteRepository.Update(_event);
                await _eventWriteRepository.SaveAsync();

                return true;
            }
        }

        public async Task<IEnumerable<EventListItemVm>> ListEventsAsync()
        {
            List<EventListItemVm> vm = await _eventReadRepository.Table
                .Include(x => x.Instructor).ThenInclude(x => x.Courses)
                .Include(x => x.Course)
                .Where(x => ActiveInstructor.Courses.Contains(x.Course))
                .Select(x => new EventListItemVm()
                {
                    Id = x.Id,
                    CourseId = x.Course.Id,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    Title = x.Title,
                    Url = x.Url,
                    BackgroundColor = x.BackgroundColor,
                    TextColor = x.TextColor,
                    IsBelong = x.Instructor == ActiveInstructor,
                    InstructorId = x.Instructor.Id
                }).ToListAsync();

            return vm;
        }

        public async Task<IEnumerable<EventListItemVm>> ListStudentEventsAsync()
        {
            List<EventListItemVm> vm = await _eventReadRepository.Table
                .Include(x => x.Instructor).ThenInclude(x => x.Courses)
                .Include(x => x.Course)
                .Where(x => ActiveStudent.Courses.Contains(x.Course))
                .Select(x => new EventListItemVm()
                {
                    Id = x.Id,
                    CourseId = x.Course.Id,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    Title = x.Title,
                    Url = x.Url,
                    BackgroundColor = x.BackgroundColor,
                    TextColor = x.TextColor,
                    InstructorId = x.Instructor.Id
                }).ToListAsync();

            return vm;
        }

        public async Task<bool> DeleteEventAsync(Guid id)
        {
            Event _event = await _eventReadRepository.GetSingleAsync(x => x.Id == id);
        
            _eventWriteRepository.Remove(_event);
            await _eventWriteRepository.SaveAsync();
            return true;
        }

        private Instructor ActiveInstructor
        {
            get
            {
                return _instructorReadRepository.Table
                .Include(x => x.Courses)
                .Include(x => x.Events)
                .Single(x => x.Email == _actionContextAccessor.ActionContext.HttpContext.User.Identity.Name);
            }
        }

        private Student ActiveStudent
        {
            get
            {
                return _studentReadRepository.Table
                .Include(x => x.Courses)
                .Single(x => x.Email == _actionContextAccessor.ActionContext.HttpContext.User.Identity.Name);
            }
        }
    }
}
