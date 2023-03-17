namespace BookReservations.Api.BL.Models;

public class BookDetailModel : BookModel
{
    public ICollection<AuthorModel> Authors { get; set; } = new List<AuthorModel>();
    public ICollection<ReviewModel> Reviews { get; set; } = new List<ReviewModel>();
}
