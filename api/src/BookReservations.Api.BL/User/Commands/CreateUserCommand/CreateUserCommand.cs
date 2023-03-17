using BookReservations.Api.BL.Models;
using BookReservations.Infrastructure.BL.Commands;

namespace BookReservations.Api.BL.Commands;

public record CreateUserCommand(UserModel User) : CommandRequest<CreateUserResponse>;
