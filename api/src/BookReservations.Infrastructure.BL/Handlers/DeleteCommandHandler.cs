using BookReservations.Infrastructure.BL.Commands;
using BookReservations.Infrastructure.DAL;
using BookReservations.Infrastructure.DAL.Query.Interfaces;
using MapsterMapper;
using MediatR;

namespace BookReservations.Infrastructure.BL.Handlers;

public class DeleteCommandHandler<TModel, TEntity, TKey> : CommandHandler<DeleteCommand<TEntity>>
    where TModel : class
    where TEntity : Entity<TKey>
    where TKey : IEquatable<TKey>
{
    protected readonly IUnitOfWorkProvider unitOfWorkProvider;
    protected readonly IRepository<TEntity, TKey> repository;
    protected readonly IQuery<TEntity> query;

    public DeleteCommandHandler(IMapper mapper, IRepository<TEntity, TKey> repository, IUnitOfWorkProvider unitOfWorkProvider, IQuery<TEntity> query) : base(mapper)
    {
        this.unitOfWorkProvider = unitOfWorkProvider;
        this.repository = repository;
        this.query = query;
    }

    public override async Task<Unit> Handle(DeleteCommand<TEntity> request, CancellationToken cancellationToken)
    {
        var entitiesToDelete = await query.Where(request.Predicate).ExecuteAsync(cancellationToken);

        foreach (var entity in entitiesToDelete.Data)
        {
            await repository.RemoveAsync(entity, cancellationToken);
        }

        await unitOfWorkProvider.UnitOfWork.CommitAsync(cancellationToken);
        return Unit.Value;
    }
}