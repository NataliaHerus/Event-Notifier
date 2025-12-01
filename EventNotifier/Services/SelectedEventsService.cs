using EventNotifier.Data;
using EventNotifier.Data.Entities;
using EventNotifier.DTOs;
using EventNotifier.Repositories.Interfaces;
using EventNotifier.Services.Interfaces;
using AutoMapper;

namespace EventNotifier.Services
{
    public class SelectedEventsService : ISelectedEventsService
    {
        protected readonly IEventRepository _eventRepository;
        protected readonly ISelectedEventsRepository _selectedEventsRepository;
        protected readonly IMapper _mapper;
        protected readonly IHttpContextAccessor _httpContextAccessor;
        protected readonly IAzureService _azureService;

        public SelectedEventsService(IHttpContextAccessor httpContextAccessor,
            IMapper mapper, IEventRepository eventRepository, ISelectedEventsRepository selectedEventsRepository, IAzureService azureService)
        {
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _eventRepository = eventRepository;
            _selectedEventsRepository = selectedEventsRepository;
            _azureService = azureService;
        }
        public async Task<IEnumerable<EventDto>> GetEventsByUser()
        {
            string userId = _httpContextAccessor.HttpContext.User.Identity.Name;
            var selectedEvents = await _selectedEventsRepository.GetEventsByUserId(userId);
            List<EventDto> events = new List<EventDto>();
            foreach(var selectedEvent in selectedEvents)
            {
                var selectedEventDto = _mapper.Map<EventDto>(await _eventRepository.GetEventById(selectedEvent.EventId));
                selectedEventDto.Photo = await _azureService.DownloadBlobAsync($"{selectedEventDto.Id}.jpg");
                events.Add(selectedEventDto);
            }
            return events;
        }

        public async Task<SelectedEventsDto> CreateSelectedEventAsync(SelectedEventsDto dto)
        {
            dto.UserId = _httpContextAccessor.HttpContext.User.Identity.Name;
            var selectedEvents = await _selectedEventsRepository.GetEventsByUserId(dto.UserId);
            foreach (var selectedEvent in selectedEvents)
            {
                if (selectedEvent.EventId == dto.EventId && selectedEvent.UserId == dto.UserId)
                {
                    throw new InvalidOperationException("Ця подія вже вибрана вами");
                }
            }
            var Event = _mapper.Map<SelectedEvents>(dto);
            await _selectedEventsRepository.AddSelectedEventsAsync(Event);
            return _mapper.Map<SelectedEventsDto>(Event);
        }

        public async Task<SelectedEventsDto> DeleteEventAsync(int id)
        {
            var Event = await _selectedEventsRepository.GetSelectedEventById(id);
            var deletedEvent = await _selectedEventsRepository.DeleteAsync(Event);
            await _selectedEventsRepository.SaveChangesAsync();

            return _mapper.Map<SelectedEventsDto>(deletedEvent);
        }

        public async Task<SelectedEventsDto> GetEventAsync(int eventId)
        {
            string userId = _httpContextAccessor.HttpContext.User.Identity.Name;
            SelectedEvents Event = await _selectedEventsRepository.GetEvent(userId, eventId);
            return _mapper.Map<SelectedEventsDto>(Event);
        }
    }
}
