using BookReservations.Api.BL.Models;
using FluentValidation;

namespace BookReservations.Api.BL.Validators;

public class BookModelValidator : AbstractValidator<BookModel>
{
    public BookModelValidator()
    {
        RuleFor(i => i.Name).NotEmpty().NotNull();
        RuleFor(i => i.Language).NotEmpty().NotNull();
        RuleFor(i => i.Description).NotEmpty().NotNull();
        RuleFor(i => i.TotalAmount).GreaterThan(-1);
        RuleFor(i => i.Isbn).NotEmpty().NotNull();
    }
}
