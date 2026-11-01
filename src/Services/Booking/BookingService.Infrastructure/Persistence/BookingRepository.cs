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

        public async Task<IReadOnlyList<string>> FindTakenSeatsAsync(Guid showingId, IEnumerable<Seat> seats, CancellationToken ct)
        {
            var labels = seats.Select(s => s.Label).ToList();

            return await db.Bookings
                .Where(b => b.ShowingId == showingId && b.Status != BookingStatus.Cancelled)
                .SelectMany(b => b.Seats)
                .Where(s => labels.Contains(s.Label))
                .Select(s => s.Label)
                .ToListAsync(ct);
        }
    }
}