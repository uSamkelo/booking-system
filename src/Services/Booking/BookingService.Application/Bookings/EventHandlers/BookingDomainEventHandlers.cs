using BookingService.Domain.Bookings;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BookingService.Application.Bookings.EventHandlers
{
    public sealed class BookingCreateHandler(ILogger<BookingCreateHandler> logger)
        : INotificationHandler<BookingCreatedDomainEvent>
    {
        public  Task Handle(BookingCreatedDomainEvent e, CancellationToken cancellationToken)
        {
            logger.LogInformation("Booking {BookingId} created for showing {ShowingId}, seats: {Seats}",
                e.BookingId, e.ShowingId, string.Join(",", e.Seats));
            return Task.CompletedTask;
        }
    }

    public sealed class BookingConfirmedHandler(ILogger<BookingConfirmedHandler> logger)
    : INotificationHandler<BookingConfirmedDomainEvent>
    {
        public Task Handle(BookingConfirmedDomainEvent e, CancellationToken ct)
        {
            logger.LogInformation("Booking {BookingId} confirmed", e.BookingId);
            return Task.CompletedTask;
        }
    }

    public sealed class BookingCancelledHandler(ILogger<BookingCancelledHandler> logger)
    : INotificationHandler<BookingCancelledDomainEvent>
    {
        public Task Handle(BookingCancelledDomainEvent e, CancellationToken ct)
        {
            logger.LogInformation("Booking {BookingId} cancelled, seats released: {Seats}",
                e.BookingId, string.Join(",", e.Seats));
            return Task.CompletedTask;
        }
    }
}