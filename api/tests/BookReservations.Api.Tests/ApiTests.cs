using BookReservations.Api.BL.Models;
using System.Net.Http.Json;
using Xunit;

namespace BookReservations.Api.Tests;

public class ApiTests : IClassFixture<AuthorizedApiFixture>
{
    public HttpClient HttpClient { get; }

    public ApiTests(AuthorizedApiFixture fixture)
    {
        HttpClient = fixture.HttpClient;
    }

    [Fact]
    public async Task GetAuthorsShouldReturnNonEmptyList()
    {
        var response = await HttpClient.GetFromJsonAsync<List<AuthorModel>>("api/author");
        Assert.NotEmpty(response!);
    }

    [Fact]
    public async Task GetAuthorsShouldReturnNonEmptyList2()
    {
        var response = await HttpClient.GetFromJsonAsync<List<AuthorModel>>("api/author");
        Assert.NotEmpty(response!);
    }
}