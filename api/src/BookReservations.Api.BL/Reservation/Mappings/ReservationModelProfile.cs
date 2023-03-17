using BookReservations.Api.BL.Models;
using BookReservations.Api.DAL.Entities;
using Mapster;

namespace BookReservations.Api.BL.Mappings;

public class ReservationModelProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Reservation, ReservationModel>()
            .Map(i => i.BookName, j => j.Book.Name)
            .Map(i => i.TotalAmount, j => j.Book.TotalAmount)
            .Map(i => i.AvailableAmount, j => j.Book.AvailableAmount)
            .Map(i => i.UserFullName, j => j.User.FirstName + " " + j.User.LastName);
    }
}