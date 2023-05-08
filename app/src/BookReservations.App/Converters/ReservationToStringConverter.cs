using BookReservations.Api.Client;
using CommunityToolkit.Maui.Converters;
using System.Globalization;

namespace BookReservations.App.Converters;

public class ReservationToStringConverter : BaseConverterOneWay<ReservationStatus, string>
{
    public override string DefaultConvertReturnValue { get; set; } = string.Empty;

    public override string ConvertFrom(ReservationStatus value, CultureInfo culture)
        => value switch
        {
            ReservationStatus.Created => "Created",
            ReservationStatus.Cancelled => "Cancelled",
            ReservationStatus.CanRetrieve => "Can retrieve",
            ReservationStatus.Retrieved => "Retrieved",
            ReservationStatus.WantToExtend => "Want to extend",
            ReservationStatus.Extended => "Extended",
            ReservationStatus.Returned => "Returned",
            ReservationStatus.NotReturned => "Not returned",
            _ => "All statuses",
        };
}
