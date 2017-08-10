using System;
using MvvmCross.Platform.Converters;

namespace Toggl.Foundation.MvvmCross.Converters
{
    public class DateTimeToTimeConverter : MvxValueConverter<DateTime, string>
    {
        protected override string Convert(DateTime value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            => value.ToString("t");
    }
}
