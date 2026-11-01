using BookingService.Application.Abstractions;
using BookingService.Domain.Bookings;
using MediatR;

namespace BookingService.Application.Bookings.Commands
{
    public sealed record CancelBookingCommand(Guid BookingId) : ICommand;
    public sealed class CancelBookingHandler(IBookingRepository bookings, IUnitOfWork uow)
    : IRequestHandler<CancelBookingCommand>
    {
        public async Task Handle(CancelBookingCommand command, CancellationToken ct)
        {
            var booking = await bookings.GetByIdAsync(command.BookingId, ct)
                ?? throw new NotFoundException($"Booking {command.BookingId} not found.");

            booking.Cancel();
            await uow.SaveChangesAsync(ct);
        }
    }
}