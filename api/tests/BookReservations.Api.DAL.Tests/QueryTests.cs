using BookReservations.Api.DAL.Tests.Fixtures;
using Xunit;

namespace BookReservations.Api.DAL.Tests;

[CollectionDefinition(nameof(QueryTests))]
public class QueryTests : ICollectionFixture<BookReservationsQueryFixture>
{
}
