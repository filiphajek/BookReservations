using BookReservations.Api.BL.Models;
using BookReservations.Api.DAL.Entities;
using Mapster;

namespace BookReservations.Api.BL.Mappings;

public class ReviewModelProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Review, ReviewModel>()
            .Map(i => i.UserName, j => j.User == null ? "Anonymous" : j.User.UserName);
    }
}