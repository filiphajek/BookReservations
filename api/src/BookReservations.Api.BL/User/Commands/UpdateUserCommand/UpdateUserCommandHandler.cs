using BookReservations.Api.DAL.Entities;
using BookReservations.Infrastructure.BL.Handlers;
using BookReservations.Infrastructure.BL.Services;
using BookReservations.Infrastructure.DAL;
using BookReservations.Infrastructure.DAL.Query.Interfaces;
using MapsterMapper;

namespace BookReservations.Api.BL.Commands;

public class UpdateUserCommandHandler : CommandHandler<UpdateUserCommand, UpdateUserResponse>
{
    private readonly IRepository<User> repository;
    private readonly IQuery<User> query;
    private readonly IUnitOfWorkProvider unitOfWorkProvider;
    private readonly IFileStorageService fileStorageService;

    public UpdateUserCommandHandler(IMapper mapper,
        IRepository<User> repository,
        IUnitOfWorkProvider unitOfWorkProvider,
        IFileStorageService fileStorageService,
        IQuery<User> query) : base(mapper)
    {
        this.repository = repository;
        this.unitOfWorkProvider = unitOfWorkProvider;
        this.fileStorageService = fileStorageService;
        this.query = query;
    }

    public override async Task<UpdateUserResponse> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var userId = request.User.Id;
        var user = (await repository.SingleByIdAsync(userId, cancellationToken))!;
        var errors = await ValidateAsync(request, user, cancellationToken);

        if (errors.Any())
        {
            return new UpdateUserResponse(false, errors);
        }

        var oldImage = user.Image;
        var updatedUser = Mapper.Map(request.User, user);

        if (request.File is not null)
        {
            var (success, uri) = await fileStorageService.UploadFileAsync(userId.ToString(), request.File, cancellationToken);
            if (!success)
            {
                return new UpdateUserResponse(nameof(UpdateUserCommand.File), "File can not be uploaded");
            }
            updatedUser.Image = uri;
        }
        else if (string.IsNullOrEmpty(request.User.Image) || !IsHttpUrl(request.User.Image))
        {
            updatedUser.Image = oldImage;
        }

        await repository.UpdateAsync(updatedUser, cancellationToken);
        await unitOfWorkProvider.UnitOfWork.CommitAsync(cancellationToken);

        return new UpdateUserResponse(true, new());
    }

    private async Task<Dictionary<string, string[]>> ValidateAsync(UpdateUserCommand request, User user, CancellationToken cancellationToken)
    {
        var errors = new Dictionary<string, string[]>();
        if (request.User.UserName != user.UserName)
        {
            var userNameExists = await query
                .Where(i => i.UserName == request.User.UserName)
                .Page(1, 1)
                .ExecuteAsync(cancellationToken);
            if (userNameExists.ItemsCount != 0)
            {
                errors.Add(nameof(User.UserName), new[] { $"UserName {request.User.UserName} exists" });
            }
        }

        if (request.User.Email != user.Email)
        {
            var emailExists = await query
                .Where(i => i.Email == request.User.Email)
                .ExecuteAsync(cancellationToken);
            if (emailExists.ItemsCount != 0)
            {
                errors.Add(nameof(User.Email), new[] { $"Email {request.User.Email} exists" });
            }
        }

        if (request.File is not null)
        {
            if (request.File.ContentType != "image/png" && request.File.ContentType != "image/jpeg")
            {
                errors.Add(nameof(UpdateUserCommand.File), new[] { "Applicatin supports only 'png' and 'jpeg' formats" });
            }
        }

        return errors;
    }

    private static bool IsHttpUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
            && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }
}
