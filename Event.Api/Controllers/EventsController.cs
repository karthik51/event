using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Event.Api.Repository;
using static Event.Api.Helpers.Constants;
using Event.Api.Models;

namespace Event.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/v1/events")]
    public class EventsController : ControllerBase
    {
        private readonly IEventRepository _eventRepository;

        public EventsController(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        // GET: api/v1/events/GetAllEvents
        [HttpGet("GetAllEvents")]
        [Authorize(Roles = RoleNames.ADMIN + "," + RoleNames.USER)]
        public async Task<IActionResult> Get()
        {
            return new ObjectResult(await _eventRepository.GetAllEvents());
        }

        // GET: api/v1/events/GetEventsByVolunteer
        [HttpGet("GetEventsByVolunteer")]
        [Authorize(Roles = RoleNames.ADMIN + "," + RoleNames.USER)]
        public async Task<IActionResult> GetEventsByVolunteer(string name)
        {
            var @event = await _eventRepository.GetEventsByVolunteer(name);

            if (@event == null)
                return new NotFoundResult();

            return new ObjectResult(@event);
        }

        // POST: api/v1/CreateEvent
        [HttpPost("CreateEvent")]
        [Authorize(Roles = RoleNames.ADMIN)]
        public async Task<IActionResult> Post([FromBody] Models.Event @event)
        {
            await _eventRepository.CreateEvent(@event);
            return new OkObjectResult(@event);
        }

        // PUT: api/v1/UpdateEvent
        [HttpPut("UpdateEvent")]
        [Authorize(Roles = RoleNames.USER)]
        public async Task<IActionResult> Upate(Models.Event @event)
        {
            await _eventRepository.UpdateEvent(@event);
            return new OkObjectResult(@event);
        }       
    }
}
