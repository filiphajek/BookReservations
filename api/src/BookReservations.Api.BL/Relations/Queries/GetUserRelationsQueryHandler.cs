using BookReservations.Api.BL.Models;
using BookReservations.Api.DAL.Entities;
using BookReservations.Infrastructure.BL.Handlers;
using BookReservations.Infrastructure.BL.Queries;
using BookReservations.Infrastructure.DAL.EFcore.Extensions;
using BookReservations.Infrastructure.DAL.Query.Interfaces;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace BookReservations.Api.BL.Queries;

public class GetUserRelationsQueryHandler : SimpleQueryHandler<RelationInfoModel, UserBookRelations, int>
{
    public GetUserRelationsQueryHandler(IMapper mapper, IQuery<UserBookRelations> query) : base(mapper, query)
    {
    }

    public override async Task<ICollection<RelationInfoModel>> Handle(SimpleQuery<RelationInfoModel, UserBookRelations> request, CancellationToken cancellationToken)
    {
        if (request.Predicate is null)
        {
            return new List<RelationInfoModel>();
        }

        var result = await query
            .Join(i => i.Include(j => j.Book))
            .Where(request.Predicate!)
            .ExecuteAsync(cancellationToken);
        return result.Data.Select(Mapper.Map<RelationInfoModel>).ToList();
    }
}
