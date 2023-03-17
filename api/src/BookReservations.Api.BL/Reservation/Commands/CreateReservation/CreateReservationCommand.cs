using BookReservations.Api.BL.Models;
using BookReservations.Infrastructure.BL.Commands;

namespace BookReservations.Api.BL.Commands;

public record CreateReservationCommand(MakeReservationModel Reservation) : CommandRequest<CreateReservationResponse>;
