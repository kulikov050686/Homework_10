using BaseClasses;
using System;
using System.Diagnostics;
using System.Globalization;

namespace Homework_10
{
    /// <summary>
    /// Преобразует <see cref="ApplicationPage"/> в фактический вид / страницу
    /// </summary>
    public class ApplicationPageValueConverter : BaseValueConverter<ApplicationPageValueConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((ApplicationPage)value)
            {
                case ApplicationPage.Chat:
                    return new ChatBot();
                default:
                    Debugger.Break();
                    return null;
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
