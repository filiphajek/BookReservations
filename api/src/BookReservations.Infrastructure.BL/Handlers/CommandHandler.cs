using BookReservations.Infrastructure.BL.Commands;
using MapsterMapper;
using MediatR;

namespace BookReservations.Infrastructure.BL.Handlers;

public abstract class CommandHandler<TRequest, TResponse> : BaseRequestHandler, IRequestHandler<TRequest, TResponse> where TRequest : CommandRequest<TResponse>
{
    protected CommandHandler(IMapper mapper) : base(mapper) { }

    public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
}

public abstract class CommandHandler<TRequest> : BaseRequestHandler, IRequestHandler<TRequest, Unit> where TRequest : CommandRequest
{
    protected CommandHandler(IMapper mapper) : base(mapper) { }

    public abstract Task<Unit> Handle(TRequest request, CancellationToken cancellationToken);
}
