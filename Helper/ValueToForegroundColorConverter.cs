using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace BahiKitaab.Helper
{
    public class ValueToForegroundColorConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {            
            SolidColorBrush brush = new SolidColorBrush(Colors.LightBlue);
            if (parameter != null && parameter is SolidColorBrush)
            {
                brush = (SolidColorBrush)parameter;
            }

            Double doubleValue = 0.0;
            Double.TryParse(value.ToString(), out doubleValue);

            if (doubleValue < 0)
                return brush = new SolidColorBrush(Colors.Red);

            return brush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
