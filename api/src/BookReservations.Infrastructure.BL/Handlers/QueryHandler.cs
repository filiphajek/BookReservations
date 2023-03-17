using BookReservations.Infrastructure.BL.Queries;
using MapsterMapper;
using MediatR;

namespace BookReservations.Infrastructure.BL.Handlers;

public abstract class QueryHandler<TRequest, TResponse> : BaseRequestHandler, IRequestHandler<TRequest, TResponse> where TRequest : QueryRequest<TResponse>
{
    protected QueryHandler(IMapper mapper) : base(mapper) { }

    public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
}