using BookingService.Application.Abstractions;
using BookingService.Domain.Bookings;
using BookingService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BookingService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<BookingDbContext>(o => o.UseNpgsql(connectionString));
            services.AddScoped<IBookingRepository, BookingRepository>();
            services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<BookingDbContext>());
            return services;
        }
    }
}