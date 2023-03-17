using FluentValidation;
using System.Net;

namespace BookReservations.Api.Filters;

public class ValidationFilter<T> : IEndpointFilter
{
    private readonly IValidator<T> validator;

    public ValidationFilter(IValidator<T> validator)
    {
        this.validator = validator;
    }

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var model = context.Arguments.OfType<T>().FirstOrDefault();

        if (validator is not null)
        {
            var validationResult = await validator.ValidateAsync(model!);
            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.ToDictionary(),
                    statusCode: (int)HttpStatusCode.UnprocessableEntity);
            }
        }

        return await next.Invoke(context);
    }
}