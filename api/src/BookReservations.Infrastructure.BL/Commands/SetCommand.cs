namespace BookReservations.Infrastructure.BL.Commands;

public record SetCommand<TModel>(ICollection<TModel> Items) : CommandRequest<ICollection<TModel>>;
