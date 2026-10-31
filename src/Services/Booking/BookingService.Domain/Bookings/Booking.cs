namespace BookingService.Domain.Bookings;

public sealed class Booking
{
    public Guid Id { get; private set; }
    public Guid ResourceId { get; private set; }
    public Guid CustomerId { get; private set; }
    public DateRange Period { get; private set; } = null!;
    public BookingStatus Status { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }

    private Booking() {} // for EF

    public static Booking Create(Guid resourceId, Guid customerId, DateRange period, DateTime nowUtc)
    {
        if (resourceId == Guid.Empty) throw new DomainException("ResourceId is required.");
        if (customerId == Guid.Empty) throw new DomainException("CustomerId is required.");
        if (period.Start <= nowUtc) throw new DomainException("Booking must start in the future.");

        return new Booking
        {
            Id = Guid.NewGuid(),
            ResourceId = resourceId,
            CustomerId = customerId,
            Period = period,
            Status = BookingStatus.Pending,
            CreatedAtUtc = nowUtc
        };
    }

    public void Confirm()
    {
        if (Status != BookingStatus.Pending)
            throw new DomainException($"Cannot confirm a {Status} booking.");
        Status = BookingStatus.Confirmed;
    }

    public void Cancel()
    {
        if (Status == BookingStatus.Cancelled)
            throw new DomainException("Booking is already cancelled.");
        Status = BookingStatus.Cancelled;
    }
}
