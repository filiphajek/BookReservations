using BookReservations.Api.BL.Models;
using FluentValidation;

namespace BookReservations.Api.BL.Validators;

public class AuthorModelValidator : AbstractValidator<AuthorModel>
{
    public AuthorModelValidator()
    {
        RuleFor(i => i.FirstName).NotNull().NotEmpty();
        RuleFor(i => i.LastName).NotNull().NotEmpty();
        RuleFor(i => i.Nationality).NotNull().NotEmpty();
        RuleFor(i => i.Birthdate).NotNull().NotEmpty();
    }
}
