namespace BookReservations.Infrastructure.DAL.Query.Interfaces;

public interface IJoinQuery<TEntity> where TEntity : class, IBaseEntity
{
    IAfterJoinQuery<TEntity> Join(params string[] tableNames);
}
