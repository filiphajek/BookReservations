using BookReservations.Api.DAL;
using BookReservations.Api.DAL.Tests.Factories;
using BookReservations.Infrastructure;
using BookReservations.Infrastructure.Tests.Factories;
using BookReservations.Infrastructure.Tests.Fixtures;
using BookReservations.Infrastructure.Tests.Fixtures.MockedAuth;
using System.Security.Claims;

namespace BookReservations.Api.Tests;

public class AuthorizedApiFixture : ApiTestsFixture<Program, BookReservationsDbContext, TestSeeder>
{
    public AuthorizedApiFixture(DbContextFactoryOptions dbOptions) : base(dbOptions)
    {
    }

    public override MockedClaims Claims => new() { new Claim(BookReservationsClaimTypes.UserId, "1"), new Claim(ClaimTypes.Role, BookReservationsRoles.User) };
}
