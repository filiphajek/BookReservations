using BookReservations.Api.BL.Models;
using FluentValidation;

namespace BookReservations.Api.BL.Validators;

public class MakeReservationModelValidator : AbstractValidator<MakeReservationModel>
{
    public MakeReservationModelValidator()
    {
        RuleFor(i => i.BookId).NotNull();
        RuleFor(i => i.From).GreaterThanOrEqualTo(DateTime.Now.Date.AddDays(1));
        RuleFor(i => i.To).GreaterThanOrEqualTo(DateTime.Now.Date.AddDays(2));
        RuleFor(i => i.To).LessThan(DateTime.Now.Date.AddDays(60));
        RuleFor(i => i.From).Must((i, to) => i.From > to).WithMessage("'From' must be before 'To'");
    }
}
