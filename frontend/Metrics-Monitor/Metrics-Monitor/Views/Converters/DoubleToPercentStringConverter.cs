using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetricsMonitorClient.Views.Converters {
    public class DoubleToPercentStringConverter : IValueConverter {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) {
            double? val = (value as double?);
           
            if(!val.HasValue) { return "0.0%"; }

            return (val.Value / 100.0).ToString("P4", CultureInfo.InvariantCulture);
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) {
            return value;
        }
    }
}
