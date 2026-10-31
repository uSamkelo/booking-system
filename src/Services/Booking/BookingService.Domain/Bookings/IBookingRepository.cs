using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingService.Domain.Bookings
{
    public interface IBookingRepository
    {
        void Add(Booking booking);
        Task<Booking?> GetByIdAsync(Guid id, CancellationToken ct);
        Task<bool> HasOverlapAsync(Guid resourceId, DateRange period, CancellationToken ct);
    }
}