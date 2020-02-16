using MongoDB.Driver;
using Event.Api.Helpers;
using Event.Api.Models;

namespace Event.Api.Data
{
    public interface IEventContext
    {
        IMongoCollection<Models.Event> Events { get; }
    }
}