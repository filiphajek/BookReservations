using BookReservations.Api.BL.Models;
using BookReservations.Api.DAL.Entities;
using BookReservations.Infrastructure.BL.Handlers;
using BookReservations.Infrastructure.BL.Queries;
using BookReservations.Infrastructure.DAL.EFcore.Extensions;
using BookReservations.Infrastructure.DAL.Query.Interfaces;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace BookReservations.Api.BL.Queries;

public class UserDetailQueryHandler : SimpleQueryHandler<UserDetailModel, User, int>
{
    public UserDetailQueryHandler(IMapper mapper, IQuery<User> query) : base(mapper, query)
    {
    }

    public override async Task<ICollection<UserDetailModel>> Handle(SimpleQuery<UserDetailModel, User> request, CancellationToken cancellationToken)
    {
        if (request.Predicate is null)
        {
            return Array.Empty<UserDetailModel>();
        }

        var result = await query
            .Join(i => i
                .Include(j => j.Relations)
                .ThenInclude(j => j.Book)
                .Include(j => j.Reservations)
                .ThenInclude(j => j.Book))
            .Where(request.Predicate)
            .ExecuteAsync(cancellationToken);

        return Mapper.Map<ICollection<UserDetailModel>>(result.Data);
    }
}
