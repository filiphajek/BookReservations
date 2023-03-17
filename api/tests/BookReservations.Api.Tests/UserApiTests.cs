using BookReservations.Api.BL.Models;
using System.Net.Http.Json;
using Xunit;

namespace BookReservations.Api.Tests;

public class UserApiTests : IClassFixture<AuthorizedApiFixture>
{
    public HttpClient HttpClient { get; }

    public UserApiTests(AuthorizedApiFixture fixture)
    {
        HttpClient = fixture.HttpClient;
    }

    [Fact]
    public async Task GetUserShouldReturnOk()
    {
        var response = await HttpClient.GetFromJsonAsync<UserInfoModel>("api/user/1");

        Assert.NotNull(response);
        Assert.True(response.Id == 1);
        Assert.True(response.FirstName == "Simon");
    }
}
