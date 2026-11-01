using BookingService.Application.Abstractions;
using MediatR;
using static BookingService.Application.Bookings.BookingAppService;

namespace BookingService.Application.Bookings.Queries
{
    public sealed record GetBookingQuery(Guid BookingId) : IQuery<BookingDto>;
    public sealed class GetBookingHandler(IBookingQueries queries) : IRequestHandler<GetBookingQuery, BookingDto>
    {
        public async Task<BookingDto> Handle(GetBookingQuery q, CancellationToken ct)
            => await queries.GetBookingAsync(q.BookingId, ct)
                ?? throw new NotFoundException($"Booking {q.BookingId} not found.");
    }
}