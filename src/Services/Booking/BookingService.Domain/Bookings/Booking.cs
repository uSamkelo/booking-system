namespace BookingService.Domain.Bookings;

public sealed class Booking : AggregateRoot
{
    public const int MaxSeatsPerBooking = 6;

    private readonly List<Seat> _seats = [];

    public Guid Id { get; private set; }
    public Guid ShowingId { get; private set; }
    public Guid CustomerId { get; private set; }
    public IReadOnlyCollection<Seat> Seats => _seats.AsReadOnly();
    public BookingStatus Status { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }

    private Booking() {} // for EF

    public static Booking Create(Guid showingId, Guid customerId, IEnumerable<Seat> seats, DateTime nowUtc)
    {
        if (showingId == Guid.Empty) throw new DomainException("ShowingId is required.");
        if (customerId == Guid.Empty) throw new DomainException("CustomerId is required.");

        var list = seats.ToList();
        if (list.Count == 0)
            throw new DomainException("Select at least one seat.");
        if (list.Count > MaxSeatsPerBooking)
            throw new DomainException($"You can book at most {MaxSeatsPerBooking} seats at once.");
        if (list.Distinct().Count() != list.Count) // records compare by value, so "A5" == "A5"
            throw new DomainException("The same seat was selected more than once.");

        var booking = new Booking
        {
            Id = Guid.NewGuid(),
            ShowingId = showingId,
            CustomerId = customerId,
            Status = BookingStatus.Pending,
            CreatedAtUtc = nowUtc
        };
        booking._seats.AddRange(list);

        booking.Raise(new BookingCreatedDomainEvent(
            booking.Id, showingId, customerId, list.Select(s => s.Label).ToList()));

        return booking;
    }

    public void Confirm()
    {
        if (Status != BookingStatus.Pending)
            throw new DomainException($"Cannot confirm a {Status} booking.");

        Status = BookingStatus.Confirmed;
        Raise(new BookingConfirmedDomainEvent(Id));
    }

    public void Cancel()
    {
        if (Status == BookingStatus.Cancelled)
            throw new DomainException("Booking is already cancelled.");

        Status = BookingStatus.Cancelled;
        Raise(new BookingCancelledDomainEvent(Id, ShowingId, _seats.Select(s => s.Label).ToList()));
    }
}