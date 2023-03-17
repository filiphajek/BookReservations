using BookReservations.Api.BL.Models;
using BookReservations.Api.DAL.Entities;
using BookReservations.Api.DAL.Enums;
using BookReservations.Infrastructure.BL.Commands;
using BookReservations.Infrastructure.BL.Handlers;
using BookReservations.Infrastructure.BL.Services;
using BookReservations.Infrastructure.DAL;
using BookReservations.Infrastructure.DAL.Query.Interfaces;
using MapsterMapper;

namespace BookReservations.Api.BL.Commands;

public class SetReviewCommandHandler : SetCommandHandler<ReviewModel, Review, int>
{
    private readonly IQuery<Reservation> reservationQuery;
    private readonly IQuery<Review> reviewQuery;
    private readonly IUserIdProvider userIdProvider;

    public SetReviewCommandHandler(IMapper mapper,
        IQuery<Reservation> reservationQuery,
        IQuery<Review> reviewQuery,
        IRepository<Review, int> repository,
        IUserIdProvider userIdProvider,
        IUnitOfWorkProvider unitOfWorkProvider)
        : base(mapper, repository, unitOfWorkProvider)
    {
        this.reservationQuery = reservationQuery;
        this.reviewQuery = reviewQuery;
        this.userIdProvider = userIdProvider;
    }

    public override async Task<ICollection<ReviewModel>> Handle(SetCommand<ReviewModel> request, CancellationToken cancellationToken)
    {
        var list = new List<Review>();
        var userId = userIdProvider.GetUserId();
        var validItems = request.Items.Where(i => i.UserId == userId).ToList();
        var reviewIds = validItems.Select(x => x.Id).Where(i => i != 0).Distinct().ToArray();
        var bookIds = validItems.Select(x => x.BookId).Distinct().ToArray();

        ICollection<Review> existingReviews = new List<Review>();
        if (reviewIds.Any())
        {
            existingReviews = (await reviewQuery.Where(i => i.UserId == userId).AndWhere(i => reviewIds.Contains(i.Id)).ExecuteAsync(cancellationToken)).Data;
        }

        var reservations = (await reservationQuery
            .Where(i => i.UserId == userId)
            .AndWhere(i => bookIds.Contains(i.BookId))
            .AndWhere(i => i.Status != ReservationStatus.Cancelled)
            .AndWhere(i => i.Status != ReservationStatus.CanRetrieve)
            .AndWhere(i => i.Status != ReservationStatus.Created)
            .ExecuteAsync(cancellationToken)).Data;

        foreach (var review in validItems)
        {
            var existingReview = existingReviews.SingleOrDefault(i => i.Id == review.Id);
            if (existingReview is not null)
            {
                existingReview.Rating = review.Rating;
                existingReview.Text = review.Text;
                await repository.UpdateAsync(existingReview, cancellationToken);
                list.Add(existingReview);
                continue;
            }

            if (reservations.Any(i => i.BookId == review.BookId))
            {
                var createdReview = await repository.InsertAsync(Mapper.Map<Review>(review), cancellationToken);
                list.Add(createdReview);
            }
        }
        if (list.Any())
        {
            await unitOfWorkProvider.UnitOfWork.CommitAsync(cancellationToken);
        }
        return list.Select(Mapper.Map<ReviewModel>).ToList();
    }
}
