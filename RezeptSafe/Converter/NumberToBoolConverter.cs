using System.Globalization;

namespace Rezeptbuch.Converter
{
    public class NumberToBoolConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            var invert = parameter?.ToString()?.ToLower() == "invert";

            if (value is int count)
                return invert ? count <= 0 : count > 0;

            return false;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
