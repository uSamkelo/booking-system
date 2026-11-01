using BookingService.Domain.Bookings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingService.Infrastructure.Persistence;

public sealed class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> b)
    {
        b.ToTable("Bookings");
        b.HasKey(x => x.Id);
        b.Ignore(x => x.DomainEvents);

        // Seats become rows in their own table: booking_seats(BookingId, seat)
        b.OwnsMany(x => x.Seats, s =>
        {
            s.ToTable("Booking_Seats");
            s.WithOwner().HasForeignKey("BookingId");
            s.Property(x => x.Label).HasColumnName("seat").HasMaxLength(4);
            s.HasKey("BookingId", nameof(Seat.Label));
        });
        b.Navigation(x => x.Seats).UsePropertyAccessMode(PropertyAccessMode.Field);

        b.Property(x => x.Status).HasConversion<string>().HasMaxLength(20);
        b.HasIndex(x => x.ShowingId);
    }
}