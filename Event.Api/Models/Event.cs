using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Event.Api.Models
{
    [BsonIgnoreExtraElements]
    public class Event
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string EventTitle { get; set; }

        public string EventDescription { get; set; }

        public DateTime EventDate { get; set; }

        public DateTime EventStartTime { get; set; }

        public DateTime EventEndTime { get; set; }

        public string EventLocation { get; set; } 
        
        public bool IsTransportation { get; set; }
               
        public string POC { get; set; }
               
        public User[] RegisterUser { get; set; }

    }   

    public class User
    {
        public string UserId { get; set; }
        public string UserName { get; set; }

    }
}
