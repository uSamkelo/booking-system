using BookingService.Application;
using BookingService.Application.Bookings.Commands;
using BookingService.Application.Bookings.Queries;
using BookingService.Domain;
using BookingService.Infrastructure;
using FluentValidation;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration.GetConnectionString("BookingDb")!);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// Map exceptions to HTTP status codes
app.Use(async (ctx, next) =>
{
    try { await next(); }
    catch (ValidationException ex)
    {
        var errors = ex.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());
        await Results.ValidationProblem(errors).ExecuteAsync(ctx);
    }
    catch (DomainException ex) { await Results.Problem(ex.Message, statusCode: 400).ExecuteAsync(ctx); }
    catch (NotFoundException ex) { await Results.Problem(ex.Message, statusCode: 404).ExecuteAsync(ctx); }
    catch (ConflictException ex) { await Results.Problem(ex.Message, statusCode: 409).ExecuteAsync(ctx); }
});

// Commands
app.MapPost("/bookings", async (CreateBookingCommand cmd, ISender sender, CancellationToken ct) =>
{
    var id = await sender.Send(cmd, ct);
    return Results.Created($"/bookings/{id}", new { id });
});

app.MapPost("/bookings/{id:guid}/confirm", async (Guid id, ISender sender, CancellationToken ct) =>
{
    await sender.Send(new ConfirmBookingCommand(id), ct); // temporary: Payment does this in Phase 4
    return Results.NoContent();
});

app.MapPost("/bookings/{id:guid}/cancel", async (Guid id, ISender sender, CancellationToken ct) =>
{
    await sender.Send(new CancelBookingCommand(id), ct);
    return Results.NoContent();
});

// Queries
app.MapGet("/bookings/{id:guid}", async (Guid id, ISender sender, CancellationToken ct) =>
    Results.Ok(await sender.Send(new GetBookingQuery(id), ct)));

app.MapGet("/showings/{showingId:guid}/taken-seats", async (Guid showingId, ISender sender, CancellationToken ct) =>
    Results.Ok(await sender.Send(new GetTakenSeatsQuery(showingId), ct)));

app.Run();