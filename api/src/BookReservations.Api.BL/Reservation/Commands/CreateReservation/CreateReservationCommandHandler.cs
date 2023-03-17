using BookReservations.Api.DAL.Entities;
using BookReservations.Api.DAL.Enums;
using BookReservations.Infrastructure.BL.Handlers;
using BookReservations.Infrastructure.DAL;
using BookReservations.Infrastructure.Extensions;
using MapsterMapper;
using Microsoft.AspNetCore.Http;

namespace BookReservations.Api.BL.Commands;

public class CreateReservationCommandHandler : CommandHandler<CreateReservationCommand, CreateReservationResponse>
{
    private readonly IRepository<Reservation> reservationRepository;
    private readonly IRepository<Book> bookRepository;
    private readonly HttpContext httpContext;
    private readonly IUnitOfWorkProvider unitOfWorkProvider;

    public CreateReservationCommandHandler(
        IRepository<Reservation> reservationRepository,
        IRepository<Book> bookRepository,
        IMapper mapper,
        IUnitOfWorkProvider unitOfWorkProvider,
        IHttpContextAccessor httpContextAccessor)
        : base(mapper)
    {
        this.reservationRepository = reservationRepository;
        this.bookRepository = bookRepository;
        this.unitOfWorkProvider = unitOfWorkProvider;
        httpContext = httpContextAccessor.HttpContext;
    }

    public override async Task<CreateReservationResponse> Handle(CreateReservationCommand request, CancellationToken cancellationToken)
    {
        var userId = httpContext.User.GetUserId();
        var book = await bookRepository.SingleByIdAsync(request.Reservation.BookId, cancellationToken);
        if (book is null)
        {
            return new CreateReservationResponse(false);
        }
        book.AvailableAmount--;
        if (book.AvailableAmount < 0)
        {
            return new CreateReservationResponse(false);
        }
        await bookRepository.UpdateAsync(book, cancellationToken);
        var reservationEntity = Mapper.Map<Reservation>(request.Reservation);
        reservationEntity.UserId = userId!.Value;
        reservationEntity.Status = ReservationStatus.Created;
        var reservation = await reservationRepository.InsertAsync(reservationEntity, cancellationToken);
        try
        {
            //todo: this could fail because sql column constrait caused by data race
            await unitOfWorkProvider.UnitOfWork.CommitAsync(cancellationToken);
        }
        catch { }
        if (reservation is null)
        {
            return new CreateReservationResponse(false);
        }

        return new CreateReservationResponse(true, reservation.Id);
    }
}
