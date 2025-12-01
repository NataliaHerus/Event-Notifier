using EventNotifier.DTOs;
using EventNotifier.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EventNotifier.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SelectedEventsController : ControllerBase
    {
        protected readonly ISelectedEventsService _selectedEventsService;
        public SelectedEventsController(ISelectedEventsService selectedEventsService)
        {
            _selectedEventsService = selectedEventsService;
        }

        [HttpGet]
        [Route("collection/byUser")]
        public async Task<IActionResult> GetEventsByUser()
        {
            return Ok(await _selectedEventsService.GetEventsByUser());
        }

        [HttpGet]
        [Route("get/{id}")]
        public async Task<IActionResult> GetEvent([FromRoute] int id)
        {
            return Ok(await _selectedEventsService.GetEventAsync(id));
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] SelectedEventsDto Event)
        {
            try
            {
                await _selectedEventsService.CreateSelectedEventAsync(Event);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<IActionResult> DeleteSelectedEvent([FromRoute] int id)
        {
            try
            {
                var deletedEvent = await _selectedEventsService.DeleteEventAsync(id);

                return Ok(deletedEvent);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
