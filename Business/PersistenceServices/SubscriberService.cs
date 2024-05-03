using CodinaxProjectMvc.Business.Abstract.PersistenceServices;
using CodinaxProjectMvc.DataAccess.Abstract.Repositories;
using CodinaxProjectMvc.DataAccess.Models;
using CodinaxProjectMvc.Managers.Abstract;
using CodinaxProjectMvc.ViewModel;
using CodinaxProjectMvc.ViewModel.SubscribeVm;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using MlkPwgen;

namespace CodinaxProjectMvc.Business.PersistenceServices
{
    public class SubscriberService : ISubscriberService
    {
        private readonly IReadRepository<Subscriber> _subscribeReadRepository;
        private readonly IWriteRepository<Subscriber> _subscribeWriteRepository;
        private readonly IMailManager _mailManager;
        private readonly IActionContextAccessor _actionContextAccessor;

        public SubscriberService(
            IReadRepository<Subscriber> subscribeReadRepository,
            IWriteRepository<Subscriber> subscribeWriteRepository,
            IMailManager mailManager,
            IActionContextAccessor actionContextAccessor)
        {
            _subscribeReadRepository = subscribeReadRepository;
            _subscribeWriteRepository = subscribeWriteRepository;
            _mailManager = mailManager;
            _actionContextAccessor = actionContextAccessor;
        }

        public Task<PaginationVm<IEnumerable<Subscriber>>> ListSubscribersAsync(string? searchFilter = null , string? statusFilter = null)
        {

            IQueryable<Subscriber> subscribers = _subscribeReadRepository
                .GetWhere(x => !x.IsDeleted && !x.IsArchived)
                .OrderBy(OrderFilters<Subscriber>.ByCreatedDate).AsQueryable();

            if (!string.IsNullOrEmpty(searchFilter))
            {
                searchFilter = searchFilter.ToLower();
                subscribers = subscribers.Where(x => x.Email.ToLower().Contains(searchFilter));
            }

            if (!string.IsNullOrEmpty(statusFilter))
            {
                statusFilter = statusFilter.ToLower();
                if(statusFilter == "confirmed")
                    subscribers = subscribers.Where(x => x.IsEmailConfirmed);
                else if(statusFilter == "not confirmed")
                    subscribers = subscribers.Where(x => !x.IsEmailConfirmed);
            }
            else
            {
                subscribers = subscribers.Where(x => x.IsEmailConfirmed);
            }

            List<Subscriber> data = subscribers.ToList();

            return Task.FromResult(new PaginationVm<IEnumerable<Subscriber>>(data));
        }

        public async Task<bool> SubscribeAsync(string email)
        {
            Subscriber? check_sub = await _subscribeReadRepository.GetSingleAsync(x => x.Email == email);

            if (check_sub != null)
            {
                if (!check_sub.IsEmailConfirmed)
                {
                    check_sub.TokenValidationKey = PasswordGenerator.Generate(length: 10, allowed: Sets.Alphanumerics);
                    _subscribeWriteRepository.Update(check_sub);
                    await _subscribeWriteRepository.SaveAsync();

                    await _mailManager.SendSubscribeConfirmationMailAsync(check_sub.TokenValidationKey, check_sub.Email);
                }
                return true;
            }

            Subscriber? subscriber = new Subscriber(email, PasswordGenerator.Generate(length: 10, allowed: Sets.Alphanumerics));

            await _subscribeWriteRepository.AddAsync(subscriber);
            await _subscribeWriteRepository.SaveAsync();

            await _mailManager.SendSubscribeConfirmationMailAsync(subscriber.TokenValidationKey, subscriber.Email);

            return true;
        }

        public async Task<bool> ConfirmAsync(string email, string token)
        {
            Subscriber? subscriber = await _subscribeReadRepository.GetSingleAsync(x => x.Email == email);

            if (subscriber == null)
                return false;

            string validation_key = subscriber.TokenValidationKey;

            bool isValidated = subscriber.TokenValidationKey == validation_key;

            if (!isValidated)
                return false;

            subscriber.IsEmailConfirmed = true; 

            _subscribeWriteRepository.Update(subscriber);
            await _subscribeWriteRepository.SaveAsync();

            return true;
        }

        public async Task<bool> SendAsync(SubscribeSendVm subscribeSendVm)
        {
            if (!_actionContextAccessor.ActionContext.ModelState.IsValid)
            {
                return false;
            }

            List<Subscriber> subscribers = await _subscribeReadRepository
                .GetWhere(x => x.IsEmailConfirmed && !x.IsDeleted && !x.IsArchived).ToListAsync();

            await _mailManager.SendMailToAllSubscribersAsync(subscribeSendVm.Subject, subscribeSendVm.Content, subscribers);

            return true;
        }
    }
}
