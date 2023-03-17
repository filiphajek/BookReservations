using BookReservations.Api.BL.Models;
using BookReservations.Infrastructure.BL.Commands;
using Microsoft.AspNetCore.Http;

namespace BookReservations.Api.BL.Commands;

public record UpdateUserCommand(UserUpdateModel User, IFormFile? File) : CommandRequest<UpdateUserResponse>;
