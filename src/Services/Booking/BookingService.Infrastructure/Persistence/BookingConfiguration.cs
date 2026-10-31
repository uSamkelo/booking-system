using BookingService.Domain.Bookings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingService.Infrastructure.Persistence
{
    public class BookingConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> b)
        {
            b.ToTable("Bookings");
            b.HasKey(x => x.Id);

            b.ComplexProperty(x => x.Period, p =>
            {
               p.Property(r => r.Start).HasColumnName("start_utc");
               p.Property(r => r.End).HasColumnName("end_utc"); 
            });

            b.Property(x => x.Status).HasConversion<string>().HasMaxLength(20);
            b.HasIndex(x => x.ResourceId);
        }
    }
}