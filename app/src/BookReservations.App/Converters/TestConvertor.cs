using CommunityToolkit.Maui.Converters;
using System.Globalization;

namespace BookReservations.App.Converters;

public class TestConvertor : BaseConverterOneWay<bool, string>
{
    public override string DefaultConvertReturnValue { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public override string ConvertFrom(bool value, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}