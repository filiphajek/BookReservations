using BookReservations.Api.BL.Models;
using BookReservations.Api.DAL.Entities;
using Mapster;

namespace BookReservations.Api.BL.Relations.Mappings;

public class RelationModelProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<UserBookRelations, RelationInfoModel>()
            .Map(i => i.BookName, j => j.Book.Name);
    }
}
