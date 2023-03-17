using BookReservations.Api.BL.Models;
using BookReservations.Api.DAL.Entities;
using Mapster;

namespace BookReservations.Api.BL.Mappings;

public class AuthorSimpleModelProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Author, AuthorSimpleModel>()
            .Map(i => i.FullName, j => j.FirstName + " " + j.LastName);
    }
}