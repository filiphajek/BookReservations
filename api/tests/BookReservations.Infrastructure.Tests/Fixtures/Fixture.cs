using Xunit;

namespace BookReservations.Infrastructure.Tests.Fixtures;

public abstract class Fixture : IAsyncLifetime
{
    public virtual async Task InitializeAsync() => await SeedAsync();
    public virtual Task DisposeAsync() => Task.CompletedTask;
    protected abstract Task SeedAsync();
}
