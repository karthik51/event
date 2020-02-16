using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Event.Api.Helpers;
using Event.Api.Models;

namespace Event.Api.Data
{
    public class EventContext : IEventContext
    {
        private readonly IMongoDatabase _db;

        public EventContext(IOptions<MongoSettings> options, IMongoClient client)
        {
            _db = client.GetDatabase(options.Value.Database);
        }

        public IMongoCollection<Models.Event> Events => _db.GetCollection<Models.Event>("Events");
      }
}