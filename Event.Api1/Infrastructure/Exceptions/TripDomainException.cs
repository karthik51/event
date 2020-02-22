using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Event.Api.Infrastructure.Exceptions
{
    public class EventDomainException : Exception
    {
        public EventDomainException()
        {

        }

        public EventDomainException(string message)
            : base(message)
        { }

        public EventDomainException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
