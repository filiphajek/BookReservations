using BookReservations.Api.BL.Models;
using BookReservations.Api.DAL.Entities;
using BookReservations.Infrastructure.BL.Handlers;
using BookReservations.Infrastructure.BL.Services;
using BookReservations.Infrastructure.DAL.Query.Interfaces;
using MapsterMapper;

namespace BookReservations.Api.BL.Queries;

public class UserPaginatedQueryHandler : PaginatedQueryHandler<UserPaginatedQuery, UserInfoModel, User, int>
{
    public UserPaginatedQueryHandler(IMapper mapper, IQuery<User> query, IPaginatedUrlBuilder urlBuilder) : base(mapper, query, urlBuilder)
    {
    }

    public override IPageQuery<User> BuildQuery(UserPaginatedQuery request)
    {
        if (string.IsNullOrEmpty(request.SearchText))
        {
            return base.BuildQuery(request);
        }
        return query
            .Where(i => i.Email.ToLower().StartsWith(request.SearchText.ToLower()))
            .OrWhere(i => i.FirstName.ToLower().StartsWith(request.SearchText.ToLower()))
            .OrWhere(i => i.LastName.ToLower().StartsWith(request.SearchText.ToLower()))
            .OrWhere(i => i.UserName.ToLower().StartsWith(request.SearchText.ToLower()))
            .OrderBy(request.SortBy, request.Ascending);
    }
}
