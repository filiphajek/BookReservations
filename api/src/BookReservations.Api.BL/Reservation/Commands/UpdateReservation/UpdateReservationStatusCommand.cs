using BookReservations.Api.BL.Models;
using BookReservations.Infrastructure.BL.Commands;
using BookReservations.Infrastructure.BL.Common;

namespace BookReservations.Api.BL.Commands;

public record UpdateReservationStatusCommand(ICollection<UpdateReservationModel> Reservations) : CommandRequest<ErrorPropertyResponse>;
