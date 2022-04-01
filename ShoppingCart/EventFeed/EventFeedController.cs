using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ShoppingCart.EventFeed
{
    [Route("/[controller]")]
    [ApiController]
    public class EventFeedController : ControllerBase
    {
        private readonly IEventStore _eventStore;

        public EventFeedController(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        [HttpGet("")]
        // Read the start and end values from a query string parameter
        public Event[] Get([FromQuery] long start, [FromQuery] long end = long.MaxValue)
        {
            // Return the raw list of events. dotNET takes care
            // of serializing the events into the response body
            return _eventStore.GetEvents(start, end).ToArray();
        }
    }
}
