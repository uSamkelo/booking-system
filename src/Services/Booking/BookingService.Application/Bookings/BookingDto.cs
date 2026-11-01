namespace BookingService.Application.Bookings
{
    public partial class BookingAppService
    {
        public sealed record BookingDto(
                            Guid Id, Guid ShowingId, Guid CustomerId,
                            IReadOnlyList<string> Seats, string Status, DateTime CreatedAtUtc);
    }
}