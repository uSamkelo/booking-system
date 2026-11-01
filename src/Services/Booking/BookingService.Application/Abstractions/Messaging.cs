using MediatR;

namespace BookingService.Application.Abstractions
{
    public interface ICommand : IRequest { }
    public interface ICommand<TResponse> : IRequest<TResponse> { }
    public interface IQuery<TResponse> : IRequest<TResponse> { }
}