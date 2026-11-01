namespace BookingService.Domain.Bookings
{
    public interface IBookingRepository
    {
        void Add(Booking booking);
        Task<Booking?> GetByIdAsync(Guid id, CancellationToken ct);
        Task<IReadOnlyList<string>> FindTakenSeatsAsync(Guid showingId, IEnumerable<Seat> seats, CancellationToken ct);
    }
}