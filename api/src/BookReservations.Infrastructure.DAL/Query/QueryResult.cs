namespace BookReservations.Infrastructure.DAL.Query;

public class QueryResult<TEntity> where TEntity : class
{
    public int? Page { get; set; }
    public int? PageSize { get; set; }
    public int ItemsCount { get; set; }
    public int? TotalCount { get; set; }
    public ICollection<TEntity> Data { get; set; } = new List<TEntity>();
}
