using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Event.Api.Models
{
    public class RegisterEvent
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }

    }
}
