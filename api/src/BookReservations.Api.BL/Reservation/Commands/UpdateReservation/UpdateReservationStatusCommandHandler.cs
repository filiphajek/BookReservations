using BookReservations.Api.BL.Models;
using BookReservations.Api.DAL.Entities;
using BookReservations.Api.DAL.Enums;
using BookReservations.Api.DAL.Extensions;
using BookReservations.Infrastructure.BL.Common;
using BookReservations.Infrastructure.BL.Handlers;
using BookReservations.Infrastructure.DAL;
using BookReservations.Infrastructure.DAL.EFcore.Extensions;
using BookReservations.Infrastructure.DAL.Query.Interfaces;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace BookReservations.Api.BL.Commands;

public class UpdateReservationStatusCommandHandler : CommandHandler<UpdateReservationStatusCommand, ErrorPropertyResponse>
{
    private readonly IRepository<Reservation> repository;
    private readonly IQuery<Reservation> query;
    private readonly IUnitOfWorkProvider unitOfWorkProvider;

    public UpdateReservationStatusCommandHandler(IRepository<Reservation> repository, IMapper mapper, IUnitOfWorkProvider unitOfWorkProvider, IQuery<Reservation> query) : base(mapper)
    {
        this.repository = repository;
        this.unitOfWorkProvider = unitOfWorkProvider;
        this.query = query;
    }

    public override async Task<ErrorPropertyResponse> Handle(UpdateReservationStatusCommand request, CancellationToken cancellationToken)
    {
        var reservationIds = request.Reservations.Select(i => i.Id).Distinct().ToArray();
        var reservations = (await query.Join(i => i.Include(j => j.Book)).Where(i => reservationIds.Contains(i.Id)).ExecuteAsync(cancellationToken)).Data;

        if (!reservations.Any())
        {
            return new ErrorPropertyResponse(nameof(UpdateReservationModel.Id), "Reservation does not exist");
        }

        var statusErrors = new List<string>();
        foreach (var reservation in reservations)
        {
            var requestedUpdate = request.Reservations.First(i => i.Id == reservation.Id);
            if (!requestedUpdate.Status.CanBeUpdated(reservation.Status))
            {
                statusErrors.Add($"Reservation can not be updated to '{requestedUpdate.Status}', because it is in '{reservation.Status}' state");
                continue;
            }

            reservation.Status = requestedUpdate.Status;
            if (reservation.Status == ReservationStatus.Returned || reservation.Status == ReservationStatus.Returned)
            {
                reservation.Book.AvailableAmount++;
            }
            if (reservation.Status == ReservationStatus.WantToExtend)
            {
                reservation.To = reservation.To.AddDays(14);
            }
            await repository.UpdateAsync(reservation, cancellationToken);
        }
        await unitOfWorkProvider.UnitOfWork.CommitAsync(cancellationToken);

        if (statusErrors.Any())
        {
            return new ErrorPropertyResponse(false, new() { { nameof(UpdateReservationModel.Status), statusErrors.ToArray() } });
        }
        return new ErrorPropertyResponse(true, new());
    }
}