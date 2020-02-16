using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Event.Api.Data;
using Event.Api.Helpers;
using Event.Api.Models;

namespace Event.Api.Repository
{
    public class EventRepository : IEventRepository
    {
        private readonly IEventContext _context;

        public EventRepository(IEventContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Models.Event>> GetAllEvents()
        {
            return await _context
                            .Events
                            .Find(_ => true)
                            .ToListAsync();
        }

        public Task<List<Models.Event>> GetEventsByVolunteer(string username)
        {
            //FilterDefinition<Models.Event> filter = Builders<Models.Event>.Filter.ElemMatch(m => m.EventRegisterUsers, username);
            //var filter = Builders<Models.Event>.Filter.AnyEq(m => m.EventRegisterUsers, username);
            //return _context
            //        .Events
            //        .Find(filter)
            //        .ToListAsync();

           // var filter = Builders<Models.Event>.Filter.In("registerUser.userName", username);
            var result = _context.Events.Find(_ => true).ToListAsync();
            return result;
        }

        public async Task CreateEvent(Models.Event @event)
        {
            await _context.Events.InsertOneAsync(@event);
        }

        public async Task UpdateEvent(Models.Event @event)
        {
            FilterDefinition<Models.Event> filter = Builders<Models.Event>.Filter.Eq(m => m.Id, @event.Id);
            await _context.Events.ReplaceOneAsync(filter, @event);
        }
    }
}