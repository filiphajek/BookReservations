using BookReservations.Api;
using BookReservations.Api.BL;
using BookReservations.Api.Caching;
using BookReservations.Api.DAL;
using BookReservations.Api.Extensions;
using BookReservations.Api.Middlewares;
using BookReservations.Api.Services;
using BookReservations.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;
using MvcJsonOptions = Microsoft.AspNetCore.Mvc.JsonOptions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDatabaseLayer(builder.Configuration);
builder.Services.AddBussinessLayer(builder.Configuration);
builder.Services.AddControllers();

builder.Services.Configure<JsonOptions>(o => o.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.Configure<MvcJsonOptions>(o => o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "BookReservations API", Version = "v1" });
    var securitySchema = new OpenApiSecurityScheme
    {
        Description = "Using the Authorization header with the Bearer scheme.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = JwtBearerDefaults.AuthenticationScheme
        }
    };
    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, securitySchema);
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { securitySchema, new[] { JwtBearerDefaults.AuthenticationScheme } }
    });
});

builder.Services.AddHttpContextAccessor();

builder.Services.Configure<BookReservationsDefaults>(builder.Configuration.GetSection(nameof(BookReservationsDefaults)));
builder.Services.Configure<AuthServiceOptions>(builder.Configuration.GetSection(nameof(AuthServiceOptions)));
builder.Services.AddAuthentication("default")
    .AddCookie("default", c =>
    {
        c.Cookie.Name = "book-reservation-cookie";
        c.Cookie.SameSite = SameSiteMode.Lax;
        c.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    })
    .AddJwtBearer(i => i.TokenValidationParameters = new()
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration[$"{nameof(AuthServiceOptions)}:{nameof(AuthServiceOptions.Issuer)}"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration[$"{nameof(AuthServiceOptions)}:{nameof(AuthServiceOptions.Audience)}"],
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration[$"{nameof(AuthServiceOptions)}:{nameof(AuthServiceOptions.SecretKey)}"]!))
    })
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"), "BearerMsal")
    .EnableTokenAcquisitionToCallDownstreamApi()
    .AddMicrosoftGraph(builder.Configuration.GetSection("MicrosoftGraph"))
    .AddInMemoryTokenCaches()
    .AddDownstreamWebApi("DownstreamApi", builder.Configuration.GetSection("DownstreamApi"))
    .AddInMemoryTokenCaches();

builder.Services.AddAuthorization(i =>
{
    i.AddPolicy(BookReservationsPolicies.AdminPolicy, p =>
    {
        p.AuthenticationSchemes = new List<string> { JwtBearerDefaults.AuthenticationScheme, "default", "BearerMsal" };
        p.RequireRole(BookReservationsRoles.Admin)
        .RequireAuthenticatedUser();
    });
    i.AddPolicy(BookReservationsPolicies.LibrarianPolicy, p =>
    {
        p.AuthenticationSchemes = new List<string> { JwtBearerDefaults.AuthenticationScheme, "default", "BearerMsal" };
        p.RequireRole(BookReservationsRoles.Librarian)
        .RequireAuthenticatedUser();
    });
    i.AddPolicy(BookReservationsPolicies.UserPolicy, p =>
    {
        p.AuthenticationSchemes = new List<string> { JwtBearerDefaults.AuthenticationScheme, "default", "BearerMsal" };
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
        p.AuthenticationSchemes = new List<string> { JwtBearerDefaults.AuthenticationScheme, "default", "BearerMsal" };
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