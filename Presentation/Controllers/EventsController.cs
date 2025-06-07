using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Services;

// Controller for getting the data from the services, copilot assisted
namespace Presentation.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase {
        private readonly EventService _eventService;

        public EventsController(EventService eventService) {
            _eventService = eventService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() {
            var result = await _eventService.GetAllAsync();
            return Ok(result);
        }
        // Get one event with ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id) {
            var result = await _eventService.GetAsync(id);
            return result != null ? Ok(result) : NotFound();
        }
    }
}
