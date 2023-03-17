namespace BookReservations.Infrastructure.DAL;

public class Entity<TKey> : IEntity<TKey> where TKey : IEquatable<TKey>
{
    public TKey Id { get; set; } = default!;
}

public class Entity : Entity<int>
{
}
