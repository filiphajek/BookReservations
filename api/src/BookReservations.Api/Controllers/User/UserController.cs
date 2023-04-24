using BookReservations.Api.BL.Commands;
using BookReservations.Api.BL.Models;
using BookReservations.Infrastructure;
using BookReservations.Infrastructure.Extensions;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookReservations.Api.Controllers;

[Route("api/user")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IValidator<UserUpdateModel> validator;
    private readonly IMediator mediator;

    public UserController(IValidator<UserUpdateModel> validator, IMediator mediator)
    {
        this.validator = validator;
        this.mediator = mediator;
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult> UpdateUser([FromForm] UserUpdateModel user, IFormFile? file, CancellationToken cancellationToken)
    {
        if (!User.IsInRole(BookReservationsRoles.Admin))
        {
            var id = User.GetUserId();
            if (id is null)
            {
                return Unauthorized();
            }
            user.Id = id.Value;
        }
        var validationResult = await validator.ValidateAsync(user, cancellationToken);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.ToDictionary());
        }
        var response = await mediator.Send(new UpdateUserCommand(user, file), cancellationToken);
        return StatusCode(response.Success ? 200 : 400, response);
    }
}
