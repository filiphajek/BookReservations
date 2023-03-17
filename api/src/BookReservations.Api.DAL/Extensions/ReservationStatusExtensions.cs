using BookReservations.Api.DAL.Enums;

namespace BookReservations.Api.DAL.Extensions;

public static class ReservationStatusExtensions
{
    public static bool ReaderHasBook(this ReservationStatus status)
    {
        return status == ReservationStatus.Retrieved || status == ReservationStatus.NotReturned || status == ReservationStatus.Extended || status == ReservationStatus.WantToExtend;
    }

    public static bool CanCreateNewReservation(this ReservationStatus status)
    {
        return status == ReservationStatus.Cancelled || status == ReservationStatus.Returned;
    }

    public static bool CanCancel(this ReservationStatus status)
    {
        return status == ReservationStatus.Created || status == ReservationStatus.CanRetrieve;
    }

    public static bool CanBeUpdated(this ReservationStatus status, ReservationStatus currentStatus)
    {
        switch (status)
        {
            case ReservationStatus.Created:
                return false;
            case ReservationStatus.Cancelled:
                if (!currentStatus.CanCancel())
                {
                    return false;
                }
                break;
            case ReservationStatus.CanRetrieve:
                if (currentStatus != ReservationStatus.Created)
                {
                    return false;
                }
                break;
            case ReservationStatus.Retrieved:
                if (currentStatus != ReservationStatus.CanRetrieve)
                {
                    return false;
                }
                break;
            case ReservationStatus.WantToExtend:
                if (currentStatus != ReservationStatus.Retrieved)
                {
                    return false;
                }
                break;
            case ReservationStatus.Extended:
                if (currentStatus != ReservationStatus.WantToExtend)
                {
                    return false;
                }
                break;
            case ReservationStatus.Returned:
            case ReservationStatus.NotReturned:
                if (currentStatus != ReservationStatus.Extended || currentStatus != ReservationStatus.Retrieved)
                {
                    return false;
                }
                break;
            default:
                throw new NotImplementedException();
        }
        return true;
    }
}
