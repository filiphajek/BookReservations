namespace BookReservations.Api.Middlewares;

public record ExceptionResponse(string Type, string ExceptionMessage);
public record KnownExceptionResponse(string Type, string ExceptionMessage, string Message);

public class ExceptionMiddleware
{
    private readonly RequestDelegate request;

    public ExceptionMiddleware(RequestDelegate request)
    {
        this.request = request;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await request(context);
        }
        catch (Exception)
        {
            context.Response.StatusCode = 500;
            await context.Response.WriteAsJsonAsync(new ExceptionResponse(nameof(Exception), "Not authorized or not found"));
        }
    }
}