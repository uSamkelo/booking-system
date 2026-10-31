using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingService.Domain
{
    public sealed class DomainException(string message) : Exception(message)
    {
        
    }
}