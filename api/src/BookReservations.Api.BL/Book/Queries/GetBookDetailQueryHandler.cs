using BookReservations.Api.BL.Models;
using BookReservations.Api.DAL.Entities;
using BookReservations.Infrastructure.BL.Handlers;
using BookReservations.Infrastructure.BL.Queries;
using BookReservations.Infrastructure.DAL.EFcore.Extensions;
using BookReservations.Infrastructure.DAL.Query.Interfaces;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace BookReservations.Api.BL.Queries;

public class GetBookDetailQueryHandler : SimpleQueryHandler<BookDetailModel, Book, int>
{
    public GetBookDetailQueryHandler(IMapper mapper, IQuery<Book> query) : base(mapper, query)
    {
    }

    public override async Task<ICollection<BookDetailModel>> Handle(SimpleQuery<BookDetailModel, Book> request, CancellationToken cancellationToken)
    {
        if (request.Predicate is null)
        {
            return new List<BookDetailModel>();
        }

        var result = await query
            .Join(i => i.Include(j => j.Authors).Include(j => j.Reviews).ThenInclude(i => i.User))
            .Where(request.Predicate!)
            .ExecuteAsync(cancellationToken);
        return result.Data.Select(Mapper.Map<BookDetailModel>).ToList();
    }
}
