namespace BookReservations.Infrastructure.BL.Commands;

public record BulkUpdateCommand<TModel>(ICollection<TModel> Items) : CommandRequest<int>;

