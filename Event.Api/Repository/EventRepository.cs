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

        public async Task<List<Models.Event>> GetEventsByVolunteer(string username)
        {
            var filter = Builders<Models.Event>.Filter.Eq("RegisterUser.UserName", username);
            var result = await _context.Events.Find(filter).ToListAsync();
            return result;
        }

        public async Task CreateEvent(Models.Event @event)
        {
            await _context.Events.InsertOneAsync(@event);
        }
             

        public async Task<bool> UpdateEvent(RegisterEvent registerEvent)
        {
            var filter = Builders<Models.Event>.Filter.Eq("Id", registerEvent.Id);
            var currentEvent = _context.Events.Find(filter).FirstOrDefaultAsync();
            if (currentEvent.Result == null)
                return false;

            List<Models.User> updateUser = currentEvent.Result.RegisterUser;
            if (updateUser == null)
               updateUser = new List<Models.User>();

            User user = new User();
            user.UserId = registerEvent.UserId;
            user.UserName = registerEvent.UserName;
            updateUser.Add(user);

            var update = Builders<Models.Event>.Update.Set(x => x.RegisterUser, updateUser);                             

            UpdateResult result = await _context.Events.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0 && result.IsAcknowledged;
            
        }
    }
}