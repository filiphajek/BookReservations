using BookReservations.Infrastructure.BL.Queries;
using BookReservations.Infrastructure.BL.Services;
using BookReservations.Infrastructure.DAL;
using BookReservations.Infrastructure.DAL.EFcore.Extensions;
using BookReservations.Infrastructure.DAL.Query.Interfaces;
using MapsterMapper;

namespace BookReservations.Infrastructure.BL.Handlers;

public class PaginatedQueryHandler<TRequest, TModel, TEntity, TKey> : QueryHandler<TRequest, PaginatedQueryResult<TModel>>
    where TRequest : PaginatedQuery<TModel, TEntity>
    where TModel : class
    where TEntity : Entity<TKey>
    where TKey : IEquatable<TKey>
{
    protected readonly IQuery<TEntity> query;
    protected readonly IPaginatedUrlBuilder urlBuilder;

    public PaginatedQueryHandler(IMapper mapper, IQuery<TEntity> query, IPaginatedUrlBuilder urlBuilder) : base(mapper)
    {
        this.query = query;
        this.urlBuilder = urlBuilder;
    }

    public async override Task<PaginatedQueryResult<TModel>> Handle(TRequest request, CancellationToken cancellationToken)
    {
        var queryResult = await BuildQuery(request)
            .Page(request.Page, request.PageSize, true)
            .ExecuteAsync(cancellationToken);

        var data = queryResult.Data.Select(Mapper.Map<TModel>).ToList();

        var hasNextPage = queryResult.PageSize == queryResult.ItemsCount;
        var hasPrevPage = queryResult.Page > 1;

        var result = new PaginatedQueryResult<TModel>()
        {
            NextPage = hasNextPage ? urlBuilder.GetPaginatedUrl(request.Page + 1) : null,
            PreviousPage = hasPrevPage ? urlBuilder.GetPaginatedUrl(request.Page - 1) : null
        };

        Mapper.Map(queryResult, result);
        return result;
    }

    public virtual IPageQuery<TEntity> BuildQuery(TRequest request)
    {
        return query.WhereIfNotNull(request.Predicate).OrderBy(request.SortBy, request.Ascending);
    }
}