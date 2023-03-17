using BookReservations.Api.BL.Models;
using BookReservations.Api.BL.Validators;
using BookReservations.Api.DAL.Entities;
using BookReservations.Infrastructure.BL;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BookReservations.Api.BL;

public static class BLInstaller
{
    public static IServiceCollection AddBussinessLayer(this IServiceCollection services, IConfiguration configuration)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        return services
            .AddRequestHandlers(assemblies)
            .AddMapper(assemblies)
            .AddServices(assemblies)
            .AddFacades(assemblies)
            .AddGenericCommandHandlers()
            .AddFileService(configuration)
            .AddValidatorsFromAssemblyContaining<MakeReservationModelValidator>(ServiceLifetime.Singleton);
    }

    public static IServiceCollection AddGenericCommandHandlers(this IServiceCollection services)
    {
        return services
            .AddAllRequestHandlers<AuthorModel, Author>()
            .AddAllRequestHandlers<UserModel, User>()
            .AddAllRequestHandlers<BookModel, Book>()
            .AddAllRequestHandlers<ReservationModel, Reservation>()
            .AddSimpleQueryHandler<UserInfoModel, User>()
            .AddAllRequestHandlers<ReviewModel, Review>()
            .AddAllRequestHandlers<RelationModel, UserBookRelations>()
            .AddSimpleQueryHandler<RelationInfoModel, UserBookRelations>()
            .AddSimpleQueryHandler<AuthorSimpleModel, Author>();
    }
}
