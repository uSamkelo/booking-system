namespace BookingService.Application.Bookings
{
    public partial class BookingAppService
    {
        public sealed record CreateBookingRequest(Guid ResourceId, Guid CustomerId, DateTime StartUtc, DateTime EndUtc);
    }
}