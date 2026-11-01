using BookingService.Application.Abstractions;
using BookingService.Domain.Bookings;
using Microsoft.EntityFrameworkCore;
using static BookingService.Application.Bookings.BookingAppService;

namespace BookingService.Infrastructure.Persistence
{
    public sealed class BookingQueries(BookingDbContext db) : IBookingQueries
    {
        public async Task<BookingDto?> GetBookingAsync(Guid id, CancellationToken ct)
        {
            var row = await db.Bookings.AsNoTracking()
                .Where(b => b.Id == id)
                .Select(b => new
                {
                    b.Id, b.ShowingId, b.CustomerId, b.Status, b.CreatedAtUtc,
                    Seats = b.Seats.Select(s => s.Label).ToList()
                })
                .FirstOrDefaultAsync(ct);

            return row is null ? null : new BookingDto(row.Id, row.ShowingId, row.CustomerId, row.Seats.Order().ToList(), row.Status.ToString(), row.CreatedAtUtc);
        }

        public async Task<IReadOnlyList<string>> GetTakenSeatsAsync(Guid showingId, CancellationToken ct)
            => await db.Bookings.AsNoTracking()
                .Where(b => b.ShowingId == showingId && b.Status != BookingStatus.Cancelled)
                .SelectMany(b => b.Seats)
                .Select(s => s.Label)
                .ToListAsync(ct);
    }
}