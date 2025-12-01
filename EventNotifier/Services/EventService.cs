using EventNotifier.Data.Entities;
using EventNotifier.Data.Entities.CategoryEntity;
using EventNotifier.Data.Entities.FormatEntity;
using EventNotifier.DTOs;
using EventNotifier.Repositories.Interfaces;
using EventNotifier.Services.Interfaces;
using AutoMapper;
using System.Text.Json;
using Azure.Storage.Queues;

namespace EventNotifier.Services
{
    public class EventService : IEventService
    {
        protected readonly IEventRepository _eventRepository;
        protected readonly ISelectedEventsRepository _selectedEventsRepository;
        protected readonly ICategoryRepository _categoryRepository;
        protected readonly IFormatRepository _formatRepository;
        protected readonly IMapper _mapper;
        protected readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        protected readonly IAzureService _azureService;

        public EventService(IHttpContextAccessor httpContextAccessor,
            IMapper mapper, IEventRepository eventRepository, ISelectedEventsRepository selectedEventsRepository,
            ICategoryRepository categoryRepository, IFormatRepository formatRepository, IConfiguration configuration, IAzureService azureService)
        {
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _eventRepository = eventRepository;
            _selectedEventsRepository = selectedEventsRepository;
            _categoryRepository = categoryRepository;
            _formatRepository = formatRepository;
            _configuration = configuration;
            _azureService = azureService;
        }

        public async Task<EventDto> CreateEventAsync(EventDto dto)
        {
            dto.Year = dto.StartDate.Year;
            var Event = _mapper.Map<Event>(dto);
            Format format = await _formatRepository.GetFormatByName(Event.Format.Name);
            Event.Format = format;
            Category category = await _categoryRepository.GetCategoryByName(Event.Category.Name);
            Event.Category = category;
            await _eventRepository.AddEventAsync(Event);
            await _eventRepository.SaveChangesAsync();
            return _mapper.Map<EventDto>(Event);
        }

        public async Task<EventDto> UpdateEventAsync(EventDto dto)
        {
            var updatedEvent = _mapper.Map<Event>(dto);
            Format format = await _formatRepository.GetFormatByName(dto.Format.Name);
            updatedEvent.Format = format;
            Category category = await _categoryRepository.GetCategoryByName(dto.Category.Name);
            updatedEvent.Category = category;
            await _eventRepository.UpdateEventAsync(updatedEvent);
            await _eventRepository.SaveChangesAsync();

            var evtMsg = new EventUpdatedMessage
            {
                EventId = updatedEvent.Id.Value,
                NewStartDate = updatedEvent.StartDate.ToString(),
                NewEndDate = updatedEvent.EndDate.ToString()
            };

            // Надсилаємо в Azure Queue
            var queueClient = new QueueClient(
                _configuration["AzureWebJobsStorage"],
                "event-updated");

            await queueClient.CreateIfNotExistsAsync();
            await queueClient.SendMessageAsync(JsonSerializer.Serialize(evtMsg));

            return _mapper.Map<EventDto>(updatedEvent);
        }

        public async Task<EventDto> DeleteEventAsync(int id)
        {
            var Event = await _eventRepository.GetEventById(id);
            if (Event == null)
            {
                throw new ArgumentException("Event not found");
            }
            var deletedEvent = await _eventRepository.DeleteAsync(Event);
            await _eventRepository.SaveChangesAsync();

            return _mapper.Map<EventDto>(deletedEvent);
        }
        public async Task<EventDto> GetEventById(int id)
        {
            var eventEntity = await _eventRepository.GetEventById(id);
            var eventDto = _mapper.Map<EventDto>(eventEntity);
            eventDto.Photo = await _azureService.DownloadBlobAsync($"{eventDto.Id}.jpg");

            return eventDto;
        }

        public async Task<List<EventDto>> GetClosest()
        {
            var events =  _mapper.Map<List<EventDto>>(await _eventRepository.GetAllAsync());
            await Task.WhenAll(events
               .Select(async techEvent =>
               {
                   techEvent.Photo = await _azureService.DownloadBlobAsync($"{techEvent.Id}.jpg");
               }));

            return events;
        }
        public async Task<List<EventDto>> GetEvents()
        {
            var events = _mapper.Map<List<EventDto>>(await _eventRepository.GetAllAsync());
            await Task.WhenAll(events
               .Select(async techEvent =>
               {
                   techEvent.Photo = await _azureService.DownloadBlobAsync($"{techEvent.Id}.jpg");
               }));

            return events;
        }
    }
}
