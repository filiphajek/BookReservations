using BookReservations.Api.BL.Models;
using BookReservations.Api.DAL.Entities;
using BookReservations.Infrastructure.BL.Commands;
using BookReservations.Infrastructure.BL.Handlers;
using BookReservations.Infrastructure.DAL;
using BookReservations.Infrastructure.DAL.Query;
using BookReservations.Infrastructure.DAL.Query.Interfaces;
using MapsterMapper;
using NSubstitute;
using Xunit;

namespace BookReservations.Api.BL.Tests;

public class CommandTests
{
    private readonly IRepository<Author> repository = Substitute.For<IRepository<Author>>();
    private readonly IMapper mapper = Substitute.For<IMapper>();
    private readonly IUnitOfWorkProvider unitOfWorkProvider = Substitute.For<IUnitOfWorkProvider>();
    private readonly IQuery<Author> query = Substitute.For<IQuery<Author>>();
    private readonly Query<Author> whereQuery = Substitute.For<Query<Author>>();

    [Fact]
    public async Task SetCommandShouldReturnNewRecordWithId()
    {
        //setup
        int id = 3;
        var authorModel = new AuthorModel { FirstName = "test" };
        var authorEntity = new Author { FirstName = "test" };

        var newAuthorModel = new AuthorModel { Id = id, FirstName = "test" };
        var newAuthorEntity = new Author { Id = id, FirstName = "test" };

        mapper.Map<Author>(Arg.Any<AuthorModel>()).Returns(authorEntity);
        mapper.Map<AuthorModel>(Arg.Any<Author>()).Returns(newAuthorModel);
        repository.InsertAsync(Arg.Any<Author>()).ReturnsForAnyArgs(newAuthorEntity);

        //action
        var commandHandler = new SetCommandHandler<AuthorModel, Author, int>(mapper, repository, unitOfWorkProvider);

        //asserts
        var result = await commandHandler.Handle(new SetCommand<AuthorModel>(new[] { new AuthorModel() }), default);

        mapper.ReceivedWithAnyArgs(1).Map<Author>(authorModel);
        mapper.ReceivedWithAnyArgs(1).Map<AuthorModel>(newAuthorEntity);
        await repository.ReceivedWithAnyArgs(1).InsertAsync(authorEntity);
        await unitOfWorkProvider.UnitOfWork.Received(1).CommitAsync();

        Assert.Equal(1, result.Count);
        Assert.Equal(id, result.First().Id);
    }

    [Fact]
    public async Task DeleteCommandRemoveShouldComplete()
    {
        //setup
        var authorFail = new Author { FirstName = "test", LastName = "fail" };

        var queryResult = new QueryResult<Author>();
        queryResult.Data.Add(authorFail);

        query.Where(author => author.LastName.Equals("fail")).ReturnsForAnyArgs(whereQuery);
        whereQuery.ExecuteAsync().ReturnsForAnyArgs(queryResult);
        repository.RemoveAsync(Arg.Any<Author>()).Returns(Task.CompletedTask);

        //action
        var commandHandler =
            new DeleteCommandHandler<AuthorModel, Author, int>(mapper, repository, unitOfWorkProvider, query);

        await commandHandler.Handle(new DeleteCommand<Author>(author => author.LastName.Equals("fail")),
            default);

        //asserts
        await whereQuery.ReceivedWithAnyArgs(1).ExecuteAsync(default);
        await repository.ReceivedWithAnyArgs(1).RemoveAsync(authorFail);
        await unitOfWorkProvider.UnitOfWork.Received(1).CommitAsync();
        Assert.True(repository.RemoveAsync(Arg.Is<Author>(author => author.LastName.Equals("fail"))).IsCompleted);
    }
}