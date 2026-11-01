using BookingService.Application.Abstractions;
using BookingService.Domain.Bookings;
using FluentValidation;
using MediatR;

namespace BookingService.Application.Bookings.Commands
{
    public sealed record CreateBookingCommand(Guid ShowingId, Guid CustomerId, IReadOnlyList<string> Seats) : ICommand<Guid>;

    public sealed class CreateBookingValidator : AbstractValidator<CreateBookingCommand>
    {
        public CreateBookingValidator()
        {
            RuleFor(c => c.ShowingId).NotEmpty();
            RuleFor(c => c.CustomerId).NotEmpty();
            RuleFor(c => c.Seats).NotEmpty();
            RuleForEach(c => c.Seats).NotEmpty();
        }
    }

    public sealed class CreateBookingHandler(IBookingRepository bookings, IUnitOfWork uow, TimeProvider clock)
    : IRequestHandler<CreateBookingCommand, Guid>
    {
        public async Task<Guid> Handle(CreateBookingCommand command, CancellationToken ct)
        {
            var seats = command.Seats.Select(Seat.Create).ToList();

            var taken = await bookings.FindTakenSeatsAsync(command.ShowingId, seats, ct);
            if (taken.Count > 0)
                throw new ConflictException($"Seats already taken: {string.Join(",", taken)}.");

            var booking = Booking.Create(command.ShowingId, command.CustomerId, seats, clock.GetUtcNow().UtcDateTime);
            bookings.Add(booking);
            await uow.SaveChangesAsync(ct);

            return booking.Id; // deliberately returns an id, not the full object
        }
    }

}