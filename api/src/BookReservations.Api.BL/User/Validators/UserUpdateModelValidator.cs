using BookReservations.Api.BL.Models;
using FluentValidation;

namespace BookReservations.Api.BL.Validators;

public class UserUpdateModelValidator : AbstractValidator<UserUpdateModel>
{
    public UserUpdateModelValidator()
    {
        RuleFor(i => i.Email).EmailAddress();
        RuleFor(i => i.UserName).NotEmpty();
        RuleFor(i => i.FirstName).NotEmpty();
        RuleFor(i => i.LastName).NotEmpty();
    }
}