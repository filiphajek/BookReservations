using BookReservations.Api.BL.Models;
using BookReservations.Infrastructure.BL.Commands;
using BookReservations.Infrastructure.BL.Common;
using Microsoft.AspNetCore.Http;

namespace BookReservations.Api.BL.Commands;

public record CreateBookCommand(BookModel Book, ICollection<int> AuthorIds, IFormFile? File) : CommandRequest<ErrorPropertyResponse>;
