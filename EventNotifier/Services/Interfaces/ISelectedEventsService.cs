using EventNotifier.DTOs;

namespace EventNotifier.Services.Interfaces
{
    public interface ISelectedEventsService
    {
        Task<IEnumerable<EventDto>> GetEventsByUser();
        Task<SelectedEventsDto> DeleteEventAsync(int id);
        Task<SelectedEventsDto> GetEventAsync(int eventId);
        Task<SelectedEventsDto> CreateSelectedEventAsync(SelectedEventsDto dto);
    }
}
