using BookingService.Application.Abstractions;
using MediatR;

namespace BookingService.Application.Bookings.Queries
{
    public sealed record GetTakenSeatsQuery(Guid ShowingId) : IQuery<IReadOnlyList<string>>;
    
    public class GetTakenSeatsHandler(IBookingQueries queries)
    : IRequestHandler<GetTakenSeatsQuery, IReadOnlyList<string>>
    {
        public Task<IReadOnlyList<string>> Handle(GetTakenSeatsQuery q, CancellationToken ct)
            => queries.GetTakenSeatsAsync(q.ShowingId, ct);
    }
}