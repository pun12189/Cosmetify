using Cosmetify.Model.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Cosmetify.Helper
{
    public class BatchStatusToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && parameter != null && value is BatchStatus)
            {
                var btnStatus = Enum.Parse(typeof(BatchStatus), parameter.ToString());
                if (btnStatus != null)
                {
                    if ((BatchStatus)value == (BatchStatus)btnStatus)
                    {
                        return Visibility.Visible;
                    }
                }
            }

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
