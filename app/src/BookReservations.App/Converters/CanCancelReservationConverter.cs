using BookReservations.Api.Client;
using CommunityToolkit.Maui.Converters;
using System.Globalization;

namespace BookReservations.App.Converters;

public class CanCancelReservationConverter : BaseConverterOneWay<ReservationStatus, bool>
{
    public override bool DefaultConvertReturnValue { get; set; } = false;

    public override bool ConvertFrom(ReservationStatus value, CultureInfo culture)
        => value is ReservationStatus.Created || value is ReservationStatus.CanRetrieve;
}
