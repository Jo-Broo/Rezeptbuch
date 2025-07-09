using System.Globalization;
using static Rezeptbuch.Model.Recipe;

namespace Rezeptbuch.Converter
{
    class StepToBoolConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if(value is Step currentStep && parameter is string stepString && int.TryParse(stepString, out int targetStep))
            {
                return (int)currentStep == targetStep;
            }

            return false;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
