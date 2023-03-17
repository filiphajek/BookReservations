using BookReservations.Api.BL.Models;
using FluentValidation;

namespace BookReservations.Api.BL.Validators;

public class UserRegistrationModelValidator : AbstractValidator<UserRegistrationModel>
{
    public UserRegistrationModelValidator()
    {
        RuleFor(i => i.Email).EmailAddress();
        RuleFor(i => i.UserName).NotNull();
        RuleFor(i => i.FirstName).NotNull();
        RuleFor(i => i.LastName).NotNull();
        RuleFor(i => i.Password).MinimumLength(5);
    }
}
