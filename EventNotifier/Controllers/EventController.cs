using EventNotifier.DTOs;
using EventNotifier.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EventNotifier.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventController : ControllerBase
    {

        protected readonly IEventService _eventService;
        protected readonly IAzureService _azureService;
        public EventController(IEventService eventService, IAzureService azureService)
        {
            _eventService = eventService;
            _azureService = azureService;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] EventDto Event)
        {
            try
            {
                return Ok(await _eventService.CreateEventAsync(Event));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("collection")]
        public async Task<IActionResult> GetEvents()
        {
            return Ok(await _eventService.GetEvents());
        }

        [HttpGet]
        [Route("collection/closest")]
        public async Task<IActionResult> GetClosest()
        {
            return Ok(await _eventService.GetClosest());
        }

        [HttpGet]
        [Route("get/{id}")]
        public async Task<IActionResult> GetEvent([FromRoute] int id)
        {
            return Ok(await _eventService.GetEventById(id));
        }

        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> UpdateEvent([FromBody] EventDto dto)
        {
            return Ok(await _eventService.UpdateEventAsync(dto));
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<IActionResult> DeleteEvent([FromRoute] int id)
        {
            try
            {
                var deletedEvent = await _eventService.DeleteEventAsync(id);

                return Ok(deletedEvent);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("save/file")]
        public async Task<IActionResult> OnFileUploadAsync([FromForm] EventPhotoDto eventPhoto)
        {

            if (eventPhoto.File.Length > 0)
            {
                await _azureService.UploadBlobAsync($"{eventPhoto.EventId}.jpg", eventPhoto.File);

                return Ok();
            }

            return BadRequest("No file or invalid file received");
        }
    }
}
