using MediatR;

namespace BookReservations.Infrastructure.BL.Commands;

public abstract record CommandRequest : IRequest<Unit>;

public abstract record CommandRequest<TResponse> : IRequest<TResponse>;
