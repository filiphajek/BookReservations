using BookReservations.Api.BL.Models;
using BookReservations.Api.DAL.Entities;
using Mapster;

namespace BookReservations.Api.BL.Mappings;

public class BookDetailModelProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Book, BookDetailModel>()
            .MaxDepth(2);
    }
}
