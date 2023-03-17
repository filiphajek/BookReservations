using BookReservations.Api.BL.Models;
using FluentValidation;

namespace BookReservations.Api.BL.Validators;

public class UserLoginModelValidator : AbstractValidator<UserLoginModel>
{
    public UserLoginModelValidator()
    {
        RuleFor(i => i.Login).NotEmpty();
        RuleFor(i => i.Password).MinimumLength(5);
    }
}