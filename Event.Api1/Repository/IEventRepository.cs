using System.Collections.Generic;
using System.Threading.Tasks;
using Event.Api.Data;
using Event.Api.Helpers;
using Event.Api.Models;

namespace Event.Api.Repository
{
    public interface IEventRepository
    {
        Task<IEnumerable<Models.Event>> GetAllEvents();
        Task<List<Models.Event>> GetEventsByVolunteer(string name);
        Task CreateEvent(Models.Event @event);
        Task<bool> UpdateEvent(Models.RegisterEvent @event);
    }
}