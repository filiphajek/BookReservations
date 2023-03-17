using BookReservations.Api.BL.Models;
using BookReservations.Api.DAL.Entities;
using Mapster;

namespace BookReservations.Api.BL.Mappings;

public class BookModelProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Book, BookModel>()
            .AfterMapping((i, model) =>
            {
                model.IsAvailable = i.AvailableAmount > 0;
            });
    }
}