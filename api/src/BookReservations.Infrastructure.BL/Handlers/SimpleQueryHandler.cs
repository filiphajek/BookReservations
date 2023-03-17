using BookReservations.Infrastructure.BL.Queries;
using BookReservations.Infrastructure.DAL;
using BookReservations.Infrastructure.DAL.EFcore.Extensions;
using BookReservations.Infrastructure.DAL.Query.Interfaces;
using MapsterMapper;

namespace BookReservations.Infrastructure.BL.Handlers;

public class SimpleQueryHandler<TModel, TEntity, TKey> : QueryHandler<SimpleQuery<TModel, TEntity>, ICollection<TModel>>
    where TModel : class
    where TEntity : Entity<TKey>
    where TKey : IEquatable<TKey>
{
    protected readonly IQuery<TEntity> query;

    public SimpleQueryHandler(IMapper mapper, IQuery<TEntity> query) : base(mapper)
    {
        this.query = query;
    }

    public override async Task<ICollection<TModel>> Handle(SimpleQuery<TModel, TEntity> request, CancellationToken cancellationToken)
    {
        var result = await query.WhereIfNotNull(request.Predicate).ExecuteAsync(cancellationToken);
        var tmp = result.Data.Select(Mapper.Map<TModel>).ToList();
        return tmp;
    }
}
