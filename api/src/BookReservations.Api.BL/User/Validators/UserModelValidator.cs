using BookReservations.Api.BL.Models;
using BookReservations.Infrastructure;
using FluentValidation;

namespace BookReservations.Api.BL.Validators;

public class UserModelValidator : AbstractValidator<UserModel>
{
    public UserModelValidator()
    {
        RuleFor(i => i.Email).EmailAddress();
        RuleFor(i => i.UserName).NotNull();
        RuleFor(i => i.LastName).NotNull();
        RuleFor(i => i.FirstName).NotNull();
        RuleFor(i => i.Password).NotNull().MinimumLength(4);
        RuleFor(i => i.Role).Custom((i, j) =>
        {
            if (!BookReservationsRoles.AllRoles.Contains(i.ToLower()))
            {
                j.AddFailure(nameof(UserModel.Role), "Unknown role");
            }
        });
    }
}
