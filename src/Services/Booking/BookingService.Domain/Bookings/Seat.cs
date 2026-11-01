using System.Globalization;

namespace BookingService.Domain.Bookings
{
    public sealed record Seat
    {
        public string Label { get; init; } = null!;

        private Seat() { }
        private Seat(string label) => Label = label;

        public static Seat Create(string? label)
        {
            var text = label?.Trim().ToUpperInvariant() ?? "";

            if (text.Length < 2
            || !char.IsAsciiLetterUpper(text[0])
            || !int.TryParse(text.AsSpan(1), NumberStyles.None, CultureInfo.InvariantCulture, out var number)
            || number is < 1 or > 50)
            {
                throw new DomainException($"'{label}' is not a valid seat. Use a row letter and a number, e.g. A5");
            }

            return new Seat($"{text[0]}{number}"); // "A05" becomes "A5"
        }

        public override string ToString() => Label;
    }
}