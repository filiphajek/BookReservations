namespace BookReservations.Infrastructure.BL.Common;

public record ErrorPropertyResponse(bool Success, Dictionary<string, string[]> Errors)
{
    public int EntityId { get; }

    public ErrorPropertyResponse(string property, string error)
        : this(false, new() { { property, new[] { error } } })
    {
    }

    public ErrorPropertyResponse(int entityId) : this(true, new())
    {
        EntityId = entityId;
    }
}