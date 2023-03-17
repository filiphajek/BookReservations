using BookReservations.Api.BL.Models;
using BookReservations.Api.DAL.Entities;
using BookReservations.Infrastructure.BL.Commands;
using BookReservations.Infrastructure.BL.Handlers;
using BookReservations.Infrastructure.DAL;
using BookReservations.Infrastructure.DAL.Query.Interfaces;
using MapsterMapper;
using MediatR;

namespace BookReservations.Api.BL.Commands;

public record UpdateBooksCommand(ICollection<BookModel> Books) : CommandRequest<Unit>;

public class UpdateBooksCommandHandler : CommandHandler<UpdateBooksCommand, Unit>
{
    private readonly IRepository<Book> repository;
    private readonly IQuery<Book> query;
    private readonly IUnitOfWorkProvider unitOfWorkProvider;

    public UpdateBooksCommandHandler(IMapper mapper, IRepository<Book> repository, IUnitOfWorkProvider unitOfWorkProvider, IQuery<Book> query) : base(mapper)
    {
        this.repository = repository;
        this.unitOfWorkProvider = unitOfWorkProvider;
        this.query = query;
    }

    public override async Task<Unit> Handle(UpdateBooksCommand request, CancellationToken cancellationToken)
    {
        var bookIds = request.Books.Select(j => j.Id).Distinct().ToArray();
        var books = (await query.Where(i => bookIds.Contains(i.Id)).ExecuteAsync(cancellationToken)).Data;

        foreach (var book in request.Books)
        {
            if (book.AvailableAmount < 0 || book.TotalAmount < 0)
            {
                continue;
            }
            if (book.AvailableAmount > book.TotalAmount)
            {
                continue;
            }
            var bookEntity = books.First(i => i.Id == book.Id);
            var isbn = bookEntity.Isbn;
            Mapper.Map(book, bookEntity);
            bookEntity.Isbn = isbn;
            await repository.UpdateAsync(bookEntity, cancellationToken);
        }
        await unitOfWorkProvider.UnitOfWork.CommitAsync(cancellationToken);
        return Unit.Value;
    }
}