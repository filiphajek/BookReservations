using BookReservations.Api;
using BookReservations.Api.BL;
using BookReservations.Api.Caching;
using BookReservations.Api.DAL;
using BookReservations.Api.Extensions;
using BookReservations.Api.Middlewares;
using BookReservations.Api.Services;
using BookReservations.Infrastructure;
using Microsoft.AspNetCore.Http.Json;
using System.Text.Json.Serialization;
using MvcJsonOptions = Microsoft.AspNetCore.Mvc.JsonOptions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDatabaseLayer(builder.Configuration);
builder.Services.AddBussinessLayer(builder.Configuration);
builder.Services.AddControllers();

builder.Services.Configure<JsonOptions>(o => o.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.Configure<MvcJsonOptions>(o => o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpContextAccessor();

builder.Services.Configure<BookReservationsDefaults>(builder.Configuration.GetSection(nameof(BookReservationsDefaults)));
builder.Services.Configure<AuthServiceOptions>(builder.Configuration.GetSection(nameof(AuthServiceOptions)));
builder.Services.AddAuthentication("default").AddCookie("default", c =>
{
    c.Cookie.Name = "book-reservation-cookie";
    c.Cookie.SameSite = SameSiteMode.Lax;
    c.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});
builder.Services.AddAuthorization(i =>
{
    i.AddPolicy(BookReservationsPolicies.AdminPolicy, p =>
    {
        p.RequireRole(BookReservationsRoles.Admin)
        .RequireAuthenticatedUser();
    });
    i.AddPolicy(BookReservationsPolicies.LibrarianPolicy, p =>
    {
        p.RequireRole(BookReservationsRoles.Librarian)
        .RequireAuthenticatedUser();
    });
    i.AddPolicy(BookReservationsPolicies.UserPolicy, p =>
    {
        p.RequireRole(BookReservationsRoles.User)
        .RequireAuthenticatedUser();
    });
    i.AddPolicy(BookReservationsPolicies.CanUpdateBookReservationPolicy, p =>
    {
        p.RequireRole(BookReservationsRoles.Admin, BookReservationsRoles.Librarian)
        .RequireAuthenticatedUser();
    });
    i.AddPolicy(BookReservationsPolicies.ViewAllUsers, p =>
    {
        p.RequireRole(BookReservationsRoles.Admin, BookReservationsRoles.Librarian)
        .RequireAuthenticatedUser();
    });
    i.AddPolicy(BookReservationsPolicies.ProfilePolicy, p =>
    {
        p.RequireRole(BookReservationsRoles.Librarian, BookReservationsRoles.User)
        .RequireAuthenticatedUser();
    });
});

builder.Services.AddOutputCache(options =>
{
    options.AddPolicy(nameof(OutputCachePolicy), OutputCachePolicy.Instance);
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ExceptionMiddleware>();

app.UseSpaDevelopmentServer(builder.Configuration);

app.UseOutputCache();

app.MapControllers("api");

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<BookReservationsDbContext>();
    await dbContext.Database.EnsureCreatedAsync();
}

app.Run();

public partial class Program { }