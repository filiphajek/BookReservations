namespace BookReservations.Infrastructure.DAL;

public interface IEntity<TKey> : IBaseEntity where TKey : IEquatable<TKey>
{
    TKey Id { get; }
}
