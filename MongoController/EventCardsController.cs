using Microsoft.AspNetCore.Mvc;
using MobileBasedCashFlowAPI.IMongoServices;
using MobileBasedCashFlowAPI.MongoDTO;
using MobileBasedCashFlowAPI.MongoModels;

namespace MobileBasedCashFlowAPI.MongoController
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventCardsController : ControllerBase
    {
        private readonly IEventCardService _eventCardService;

        public EventCardsController(IEventCardService eventCardService)
        {
            _eventCardService = eventCardService;
        }

        [HttpGet("event-card")]
        public async Task<ActionResult<List<EventCard>>> GetAll()
        {
            var eventCard = await _eventCardService.GetAsync();
            if (eventCard != null)
            {
                return Ok(eventCard);
            }
            return NotFound("list is empty");
        }

        [HttpGet("event-card/{id:length(24)}")]
        public async Task<ActionResult<EventCard>> GetById(string id)
        {
            var eventCard = await _eventCardService.GetAsync(id);
            if (eventCard != null)
            {
                return Ok(eventCard);
            }
            return NotFound("can not find this event card");
        }

        [HttpPost("event-card")]
        public async Task<IActionResult> PostEvent(EventCardRequest request)
        {
            try
            {
                var result = await _eventCardService.CreateAsync(request);
                if (result == "success")
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpPut("event-card/{id:length(24)}")]
        public async Task<IActionResult> UpdateEvent(string id, EventCardRequest request)
        {
            try
            {
                var eventCard = await _eventCardService.GetAsync(id);
                if (eventCard is null)
                {
                    return NotFound("can not find this event card");
                }
                var result = await _eventCardService.UpdateAsync(id, request);
                if (result == "success")
                {
                    return Ok(result);
                }
                return BadRequest(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
    }
}
