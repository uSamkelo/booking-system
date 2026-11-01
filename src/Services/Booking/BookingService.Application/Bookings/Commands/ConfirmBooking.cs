using BookingService.Application.Abstractions;
using BookingService.Domain.Bookings;
using MediatR;

namespace BookingService.Application.Bookings.Commands
{
    public sealed record ConfirmBookingCommand(Guid BookingId) : ICommand;
    public sealed class ConfirmBookingHandler(IBookingRepository bookings, IUnitOfWork uow)
    : IRequestHandler<ConfirmBookingCommand>
    {
        public async Task Handle(ConfirmBookingCommand command, CancellationToken ct)
        {
            var booking = await bookings.GetByIdAsync(command.BookingId, ct)
                ?? throw new NotFoundException($"Booking {command.BookingId} not found.");

            booking.Confirm();
            await uow.SaveChangesAsync(ct);
        }
    }
}