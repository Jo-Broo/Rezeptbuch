using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RezeptSafe.Model.Recipe;

namespace RezeptSafe.Converter
{
    class StepToBoolConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is Step currentStep && parameter is string stepString && int.TryParse(stepString, out int targetStep))
                return (int)currentStep == targetStep;

            return false;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
