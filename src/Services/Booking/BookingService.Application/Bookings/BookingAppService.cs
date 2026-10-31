using BookingService.Application.Abstractions;
using BookingService.Domain.Bookings;

namespace BookingService.Application.Bookings
{
    public sealed partial class BookingAppService(IBookingRepository bookings, IUnitOfWork uow, TimeProvider clock)
    {
        public async Task<BookingDto> CreateAsync(CreateBookingRequest r, CancellationToken ct)
        {
            var period = DateRange.Create(r.StartUtc, r.EndUtc);

            if (await bookings.HasOverlapAsync(r.ResourceId, period, ct))
                throw new ConflictException("Resource is already booked for that period.");

            var booking = Booking.Create(r.ResourceId, r.CustomerId, period, clock.GetUtcNow().UtcDateTime);
            bookings.Add(booking);
            await uow.SaveChangesAsync(ct);
            return ToDto(booking);
        }

        public async Task<BookingDto> GetAsync(Guid id, CancellationToken ct)
            => ToDto(await Load(id, ct));

        public async Task<BookingDto> ConfirmAsync(Guid id, CancellationToken ct)
        {
            var booking = await Load(id, ct);
            booking.Confirm();
            await uow.SaveChangesAsync(ct);
            return ToDto(booking);
        }

        public async Task<BookingDto> CancelAsync(Guid id, CancellationToken ct)
        {
            var booking = await Load(id, ct);
            booking.Cancel();
            await uow.SaveChangesAsync(ct);
            return ToDto(booking);
        }

        private async Task<Booking> Load(Guid id, CancellationToken ct)
            => await bookings.GetByIdAsync(id, ct) ?? throw new NotFoundException($"Booking {id} not found");

        private static BookingDto ToDto(Booking b) =>
            new(b.Id, b.ResourceId, b.CustomerId, b.Period.Start, b.Period.End, b.Status.ToString());
    }
}
