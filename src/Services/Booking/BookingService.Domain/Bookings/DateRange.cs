namespace BookingService.Domain.Bookings
{
    public sealed record DateRange
    {
        public DateTime Start { get; init; }
        public DateTime End { get; init; }

        private DateRange() //this is for EF
        {
        }

        private DateRange(DateTime start, DateTime end)
        {
            Start = start;
            End = end;
        }

        public static DateRange Create(DateTime start, DateTime end)
        {
            if (start.Kind != DateTimeKind.Utc || end.Kind != DateTimeKind.Utc)
                throw new DomainException("Dates must be UTC.");
            if (start >= end)
                throw new DomainException("Start must be before end.");
            return new DateRange(start, end);
        }

        public bool Overlaps(DateRange other) => Start < other.End && other.Start < End;
    }
}