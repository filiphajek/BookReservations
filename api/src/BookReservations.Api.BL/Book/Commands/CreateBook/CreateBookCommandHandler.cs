using BookReservations.Api.DAL.Entities;
using BookReservations.Infrastructure.BL.Common;
using BookReservations.Infrastructure.BL.Handlers;
using BookReservations.Infrastructure.DAL;
using BookReservations.Infrastructure.DAL.Query.Interfaces;
using MapsterMapper;

namespace BookReservations.Api.BL.Commands;

public class CreateBookCommandHandler : CommandHandler<CreateBookCommand, ErrorPropertyResponse>
{
    private readonly IRepository<Book> bookRepository;
    private readonly IRepository<BookAuthor> bookAuthorRepository;

    private readonly IQuery<Book> query;
    private readonly IUnitOfWorkProvider unitOfWorkProvider;

    public CreateBookCommandHandler(IMapper mapper, IRepository<Book> bookRepository, IQuery<Book> query, IUnitOfWorkProvider unitOfWorkProvider, IRepository<BookAuthor> bookAuthorRepository) : base(mapper)
    {
        this.bookRepository = bookRepository;
        this.query = query;
        this.unitOfWorkProvider = unitOfWorkProvider;
        this.bookAuthorRepository = bookAuthorRepository;
    }

    public override async Task<ErrorPropertyResponse> Handle(CreateBookCommand request, CancellationToken cancellationToken)
    {
        var existingIsbnBooks = (await query.Where(i => i.Isbn == request.Book.Isbn).ExecuteAsync(cancellationToken)).Data;
        if (existingIsbnBooks.Any())
        {
            return new ErrorPropertyResponse(nameof(request.Book.Isbn), $"Book with ISBN '{request.Book.Isbn}' exists");
        }
        var book = Mapper.Map<Book>(request.Book);
        var bookEntity = await bookRepository.InsertAsync(book, cancellationToken);
        await unitOfWorkProvider.UnitOfWork.CommitAsync(cancellationToken);

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