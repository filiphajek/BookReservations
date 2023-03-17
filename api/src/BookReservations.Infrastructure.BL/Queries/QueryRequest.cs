using MediatR;

namespace BookReservations.Infrastructure.BL.Queries;

public abstract record QueryRequest<TResponse> : IRequest<TResponse>;