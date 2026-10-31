using BookingService.Domain.Bookings;
using Microsoft.EntityFrameworkCore;

namespace BookingService.Infrastructure.Persistence
{
    public class BookingRepository(BookingDbContext db) : IBookingRepository
    {
        public void Add(Booking booking)
        {
            db.Bookings.Add(booking);
        }

        public Task<Booking?> GetByIdAsync(Guid id, CancellationToken ct)
        {
            return db.Bookings.FirstOrDefaultAsync(b => b.Id == id, ct);
        }

        public Task<bool> HasOverlapAsync(Guid resourceId, DateRange period, CancellationToken ct)
        {
            var start = period.Start;
            var end = period.End;
            return db.Bookings.AnyAsync(b =>
            b.ResourceId == resourceId &&
            b.Status != BookingStatus.Cancelled &&
            b.Period.Start < end &&
            start < b.Period.End, ct);
        }
    }
}