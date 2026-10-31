using BookingService.Application.Abstractions;
using BookingService.Domain.Bookings;
using Microsoft.EntityFrameworkCore;

namespace BookingService.Infrastructure.Persistence
{
    public sealed class BookingDbContext(DbContextOptions<BookingDbContext> options) : DbContext(options), IUnitOfWork
    {
        public DbSet<Booking> Bookings => Set<Booking>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BookingDbContext).Assembly);
        }
    }
}