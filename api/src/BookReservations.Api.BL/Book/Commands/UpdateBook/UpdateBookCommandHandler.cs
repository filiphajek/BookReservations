using BookReservations.Api.DAL.Entities;
using BookReservations.Infrastructure.BL.Common;
using BookReservations.Infrastructure.BL.Handlers;
using BookReservations.Infrastructure.DAL;
using BookReservations.Infrastructure.DAL.Query.Interfaces;
using MapsterMapper;

namespace BookReservations.Api.BL.Commands;

public class UpdateBookCommandHandler : CommandHandler<UpdateBookCommand, ErrorPropertyResponse>
{
    private readonly IRepository<Book> bookRepository;
    private readonly IRepository<BookAuthor> bookAuthorRepository;
    private readonly IQuery<BookAuthor> bookAuthorQuery;
    private readonly IQuery<Book> query;
    private readonly IUnitOfWorkProvider unitOfWorkProvider;

    public UpdateBookCommandHandler(IMapper mapper, IRepository<Book> bookRepository, IQuery<Book> query, IUnitOfWorkProvider unitOfWorkProvider, IRepository<BookAuthor> bookAuthorRepository, IQuery<BookAuthor> bookAuthorQuery)
        : base(mapper)
    {
        this.bookRepository = bookRepository;
        this.query = query;
        this.unitOfWorkProvider = unitOfWorkProvider;
        this.bookAuthorRepository = bookAuthorRepository;
        this.bookAuthorQuery = bookAuthorQuery;
    }

    public override async Task<ErrorPropertyResponse> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
    {
        var book = await bookRepository.SingleByIdAsync(request.Book.Id, cancellationToken);
        if (book is null)
        {
            return new ErrorPropertyResponse(nameof(request.Book), "Book does not exists");
        }

        Mapper.Map(request.Book, book);
        var bookEntity = await bookRepository.UpdateAsync(book, cancellationToken);

        var entitiesToDelete = await bookAuthorQuery.Where(i => i.BookId == request.Book.Id).ExecuteAsync(cancellationToken);
        foreach (var entity in entitiesToDelete.Data)
        {
            await bookAuthorRepository.RemoveAsync(entity, cancellationToken);
        }

        var bookAuthors = request.AuthorIds.Select(i => new BookAuthor
        {
            AuthorId = i,
            BookId = bookEntity.Id
        }).ToList();
        foreach (var item in bookAuthors)
        {
            await bookAuthorRepository.InsertAsync(item, cancellationToken);
        }
        await unitOfWorkProvider.UnitOfWork.CommitAsync(cancellationToken);
        return new ErrorPropertyResponse(bookEntity.Id);
    }
}