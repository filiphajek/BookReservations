using BookReservations.Api.BL.Models;
using BookReservations.Api.DAL.Entities;
using BookReservations.Api.DAL.Enums;
using BookReservations.Api.DAL.Extensions;
using BookReservations.Infrastructure.BL.Handlers;
using BookReservations.Infrastructure.BL.Queries;
using BookReservations.Infrastructure.DAL.Query.Interfaces;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookReservations.Api.BL.Queries;

public class GetUserBookInfoQueryHandler : QueryHandler<GetUserBookInfoQuery, BookUserInfoModel>
{
    private readonly IQuery<Book> bookQuery;
    private readonly IQuery<UserBookRelations> relationQuery;
    private readonly IQuery<Reservation> reservationQuery;

    public GetUserBookInfoQueryHandler(IMapper mapper, IQuery<Book> bookQuery, IQuery<UserBookRelations> relationQuery, IQuery<Reservation> reservationQuery) : base(mapper)
    {
        this.bookQuery = bookQuery;
        this.relationQuery = relationQuery;
        this.reservationQuery = reservationQuery;
    }

    public override async Task<BookUserInfoModel> Handle(GetUserBookInfoQuery request, CancellationToken cancellationToken)
    {
        var book = (await bookQuery.Where(i => i.Id == request.BookId).ExecuteAsync(cancellationToken)).Data;
        if (!book.Any())
        {
            return new();
        }
        var isInWishlisht = (await relationQuery
            .Where(i => i.UserId == request.UserId)
            .AndWhere(i => i.BookId == request.BookId)
            .AndWhere(i => i.RelationType == UserBookRelationType.Wishlist)
            .ExecuteAsync(cancellationToken)).Data.Any();
        var reservations = (await reservationQuery
            .Where(i => i.UserId == request.UserId)
            .AndWhere(i => i.BookId == request.BookId)
            .ExecuteAsync(cancellationToken)).Data;
        var isInReservations = reservations.Any(i => !i.Status.CanCreateNewReservation());

        var result = Mapper.Map<BookUserInfoModel>(book.First());
        result.IsInWishlist = isInWishlisht;
        result.IsInReservations = isInReservations;
        return result;
    }
}

public class GetBooksWithUserInfoQueryHandler : QueryHandler<GetBooksWithUserInfoQuery, PaginatedQueryResult<BookUserInfoModel>>
{
    private readonly IMediator mediator;
    private readonly IQuery<Reservation> reservationQuery;
    private readonly IQuery<UserBookRelations> relationQuery;

    public GetBooksWithUserInfoQueryHandler(IMediator mediator, IMapper mapper, IQuery<Reservation> reservationQuery, IQuery<UserBookRelations> relationQuery) : base(mapper)
    {
        this.mediator = mediator;
        this.reservationQuery = reservationQuery;
        this.relationQuery = relationQuery;
    }

    public override async Task<PaginatedQueryResult<BookUserInfoModel>> Handle(GetBooksWithUserInfoQuery request, CancellationToken cancellationToken)
    {
        var bookPaginationQueryResult = await mediator.Send(new GetBooksQuery(
            request.Page, request.AuthorIds, request.OnlyAvailable, request.PageSize, request.SortBy, request.Ascending, request.SearchText), cancellationToken);

        var bookIds = bookPaginationQueryResult.Data.Select(i => i.Id).Distinct().ToArray();
        var relations = (await relationQuery
            .Where(i => i.UserId == request.UserId)
            .AndWhere(i => bookIds.Contains(i.BookId))
            .ExecuteAsync(cancellationToken)).Data;
        var reservations = (await reservationQuery
            .Where(i => i.UserId == request.UserId)
            .AndWhere(i => bookIds.Contains(i.BookId))
            .ExecuteAsync(cancellationToken)).Data;

        var bookResult = Mapper.Map<PaginatedQueryResult<BookUserInfoModel>>(bookPaginationQueryResult);
        foreach (var item in bookResult.Data)
        {
            var relation = relations.FirstOrDefault(i => i.BookId == item.Id && i.RelationType == UserBookRelationType.Wishlist);
            if (relation is not null)
            {
                item.IsInWishlist = true;
            }

            var reservation = reservations.FirstOrDefault(i => i.BookId == item.Id && !i.Status.CanCreateNewReservation());
            if (reservation is not null)
            {
                item.IsInReservations = true;
            }
        }

        return bookResult;
    }
}
