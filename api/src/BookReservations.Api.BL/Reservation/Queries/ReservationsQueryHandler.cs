using BookReservations.Api.BL.Models;
using BookReservations.Api.DAL.Entities;
using BookReservations.Infrastructure.BL.Handlers;
using BookReservations.Infrastructure.BL.Queries;
using BookReservations.Infrastructure.BL.Services;
using BookReservations.Infrastructure.DAL.EFcore.Extensions;
using BookReservations.Infrastructure.DAL.Query.Interfaces;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace BookReservations.Api.BL.Queries;

public class ReservationsQueryHandler : PaginatedQueryHandler<PaginatedQuery<ReservationModel, Reservation>, ReservationModel, Reservation, int>
{
    public ReservationsQueryHandler(IMapper mapper, IQuery<Reservation> query, IPaginatedUrlBuilder urlBuilder) : base(mapper, query, urlBuilder)
    {
    }

    public override IPageQuery<Reservation> BuildQuery(PaginatedQuery<ReservationModel, Reservation> request)
    {
        return query
            .Join(i => i.Include(j => j.Book).Include(i => i.User))
            .WhereIfNotNull(request.Predicate)
            .OrderBy(request.SortBy, request.Ascending);
    }
}
