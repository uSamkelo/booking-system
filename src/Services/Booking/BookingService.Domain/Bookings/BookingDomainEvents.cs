using BookingService.Domain.Interfaces;

namespace BookingService.Domain.Bookings
{
    public sealed record BookingCreatedDomainEvent(
    Guid BookingId, Guid ShowingId, Guid CustomerId, IReadOnlyList<string> Seats) : IDomainEvent;

    public sealed record BookingConfirmedDomainEvent(Guid BookingId) : IDomainEvent;

    public sealed record BookingCancelledDomainEvent(
        Guid BookingId, Guid ShowingId, IReadOnlyList<string> Seats) : IDomainEvent;
}