using BookingService.Application;
using BookingService.Application.Bookings;
using BookingService.Domain;
using BookingService.Infrastructure;
using static BookingService.Application.Bookings.BookingAppService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddScoped<BookingAppService>();
builder.Services.AddInfrastructure(builder.Configuration.GetConnectionString("BookingDb")!);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// Map exceptions to HTTP status codes
app.Use(async (ctx, next) =>
{
    try { await next(); }
    catch (DomainException ex) { await Results.Problem(ex.Message, statusCode: 400).ExecuteAsync(ctx); }
    catch (NotFoundException ex) { await Results.Problem(ex.Message, statusCode: 404).ExecuteAsync(ctx); }
    catch (ConflictException ex) { await Results.Problem(ex.Message, statusCode: 409).ExecuteAsync(ctx); }
});

var group = app.MapGroup("/bookings");

group.MapPost("/", async (CreateBookingRequest req, BookingAppService svc, CancellationToken ct) =>
{
    var dto = await svc.CreateAsync(req, ct);
    return Results.Created($"/bookings/{dto.Id}", dto);
});
group.MapGet("/{id:guid}", async (Guid id, BookingAppService svc, CancellationToken ct) =>
    Results.Ok(await svc.GetAsync(id, ct)));
group.MapPost("/{id:guid}/confirm", async (Guid id, BookingAppService svc, CancellationToken ct) =>
    Results.Ok(await svc.ConfirmAsync(id, ct)));   // temporary: Payment will do this in Phase 4
group.MapPost("/{id:guid}/cancel", async (Guid id, BookingAppService svc, CancellationToken ct) =>
    Results.Ok(await svc.CancelAsync(id, ct)));

app.Run();