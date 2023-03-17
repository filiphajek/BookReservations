using BookReservations.Infrastructure.BL.Services;
using Xunit;

namespace BookReservations.Api.BL.Tests;

public class HashServiceTests
{
    private readonly HashService hashService = new();

    [Fact]
    public void SameTextShouldMatch()
    {
        var hash = hashService.Hash("text");
        Assert.True(hashService.Verify("text", hash));
    }

    [Fact]
    public void DifferentTextShouldNotMatch()
    {
        var hash = hashService.Hash("text");
        Assert.False(hashService.Verify("text1", hash));
    }
}
