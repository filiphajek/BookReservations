using BookReservations.Api.BL.Commands;
using BookReservations.Api.BL.Models;
using BookReservations.Infrastructure;
using BookReservations.Infrastructure.BL.Common;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookReservations.Api.Controllers;

[Route("api/book")]
[ApiController]
public class BookController : ControllerBase
{
    private readonly IValidator<BookModel> validator;
    private readonly IMediator mediator;

    public BookController(IValidator<BookModel> validator, IMediator mediator)
    {
        this.validator = validator;
        this.mediator = mediator;
    }

    [Authorize(Policy = BookReservationsPolicies.CanUpdateBookReservationPolicy)]
    [HttpPost]
    [ProducesResponseType(typeof(IDictionary<string, string[]>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorPropertyResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorPropertyResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult> CreateBook([FromForm] BookModel book, [FromForm] ICollection<int> authorIds, IFormFile? file, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(book, cancellationToken);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.ToDictionary());
        }
        ErrorPropertyResponse? result;
        if (book.Id == 0)
        {
            result = await mediator.Send(new CreateBookCommand(book, authorIds, file), cancellationToken);
        }
        else
        {
            result = await mediator.Send(new UpdateBookCommand(book, authorIds, file), cancellationToken);
        }
        if (result.Success)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }
}
