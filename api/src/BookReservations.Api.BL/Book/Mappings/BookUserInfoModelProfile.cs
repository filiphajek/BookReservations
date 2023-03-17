using BookReservations.Api.BL.Models;
using BookReservations.Api.DAL.Entities;
using Mapster;

namespace BookReservations.Api.BL.Mappings;

public class BookUserInfoModelProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Book, BookUserInfoModel>()
            .AfterMapping((i, model) =>
            {
                model.IsAvailable = i.AvailableAmount > 0;
            });
    }
}
