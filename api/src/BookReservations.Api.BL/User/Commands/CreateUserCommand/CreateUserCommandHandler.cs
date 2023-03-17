using BookReservations.Api.DAL.Entities;
using BookReservations.Infrastructure;
using BookReservations.Infrastructure.BL.Handlers;
using BookReservations.Infrastructure.BL.Services;
using BookReservations.Infrastructure.DAL;
using BookReservations.Infrastructure.DAL.Query.Interfaces;
using MapsterMapper;
using Microsoft.Extensions.Options;

namespace BookReservations.Api.BL.Commands;

public class CreateUserCommandHandler : CommandHandler<CreateUserCommand, CreateUserResponse>
{
    private readonly IRepository<User> repository;
    private readonly IQuery<User> query;
    private readonly BookReservationsDefaults options;
    private readonly IUnitOfWorkProvider unitOfWorkProvider;
    private readonly IHashService hashService;


    public CreateUserCommandHandler(IMapper mapper,
        IRepository<User> repository,
        IUnitOfWorkProvider unitOfWorkProvider,
        IQuery<User> query,
        IOptions<BookReservationsDefaults> options,
        IHashService hashService) : base(mapper)
    {
        this.repository = repository;
        this.unitOfWorkProvider = unitOfWorkProvider;
        this.query = query;
        this.options = options.Value;
        this.hashService = hashService;
    }

    public override async Task<CreateUserResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var queryResult = await query
            .Where(i => i.UserName == request.User.UserName)
            .OrWhere(i => i.Email == request.User.Email)
            .Page(1, 10)
            .ExecuteAsync(cancellationToken);

        if (queryResult.ItemsCount != 0)
        {
            var users = queryResult.Data;
            bool userNameExists = users.Any(i => i.UserName == request.User.UserName);
            bool emailNameExists = users.Any(i => i.Email == request.User.Email);
            return new CreateUserResponse(false, null, emailNameExists, userNameExists);
        }

        request.User.Image = options.Image;
        request.User.Password = hashService.Hash(request.User.Password);
        var entity = await repository.InsertAsync(Mapper.Map<User>(request.User), cancellationToken);
        await unitOfWorkProvider.UnitOfWork.CommitAsync(cancellationToken);
        return new CreateUserResponse(true, entity.Id);
    }
}