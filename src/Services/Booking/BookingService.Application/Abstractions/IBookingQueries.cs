using static BookingService.Application.Bookings.BookingAppService;

namespace BookingService.Application.Abstractions
{
    public interface IBookingQueries
    {
        Task<BookingDto?> GetBookingAsync(Guid id, CancellationToken ct);
        Task<IReadOnlyList<string>> GetTakenSeatsAsync(Guid showingId, CancellationToken ct);
    }
}