using BookingService.Application.Abstractions;
using BookingService.Domain;
using BookingService.Domain.Bookings;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookingService.Infrastructure.Persistence
{
    public sealed class BookingDbContext(DbContextOptions<BookingDbContext> options, IPublisher publisher) : DbContext(options), IUnitOfWork
    {
        public DbSet<Booking> Bookings => Set<Booking>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
           => modelBuilder.ApplyConfigurationsFromAssembly(typeof(BookingDbContext).Assembly);

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            //1. Collect events from every tracked aggregate, then clear them
            var aggregates = ChangeTracker.Entries<AggregateRoot>()
                .Select(e => e.Entity)
                .Where(a => a.DomainEvents.Count > 0)
                .ToList();

            var events = aggregates.SelectMany(a => a.DomainEvents).ToList();
            aggregates.ForEach(a => a.ClearDomainEvents());

            // 2. Commit
            var result = await base.SaveChangesAsync(cancellationToken);

            //3. Dispatch in-process, after the commit
            foreach (var domainEvent in events)
                await publisher.Publish(domainEvent, cancellationToken);

            return result;
        }
    }
}