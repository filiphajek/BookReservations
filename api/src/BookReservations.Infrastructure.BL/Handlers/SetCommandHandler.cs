using BookReservations.Infrastructure.BL.Commands;
using BookReservations.Infrastructure.DAL;
using MapsterMapper;

namespace BookReservations.Infrastructure.BL.Handlers;

public class SetCommandHandler<TModel, TEntity, TKey> : CommandHandler<SetCommand<TModel>, ICollection<TModel>>
    where TModel : class
    where TEntity : Entity<TKey>
    where TKey : IEquatable<TKey>
{
    protected readonly IRepository<TEntity, TKey> repository;
    protected readonly IUnitOfWorkProvider unitOfWorkProvider;

    public SetCommandHandler(IMapper mapper, IRepository<TEntity, TKey> repository, IUnitOfWorkProvider unitOfWorkProvider) : base(mapper)
    {
        this.repository = repository;
        this.unitOfWorkProvider = unitOfWorkProvider;
    }

    public override async Task<ICollection<TModel>> Handle(SetCommand<TModel> request, CancellationToken cancellationToken)
    {
        var items = request.Items.Select(Mapper.Map<TEntity>);

        var result = new List<TEntity>();
        foreach (var item in items)
        {
            result.Add(await repository.InsertAsync(item, cancellationToken));
        }

        await unitOfWorkProvider.UnitOfWork.CommitAsync(cancellationToken);

        return result.Select(Mapper.Map<TModel>).ToList();
    }
}
