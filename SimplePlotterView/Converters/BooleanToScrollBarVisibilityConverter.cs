using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SimplePlotterView
{
    public class BooleanToScrollBarVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool valueResult = (bool)value;
            if (valueResult == true)
                return System.Windows.Controls.ScrollBarVisibility.Auto;
            else
                return System.Windows.Controls.ScrollBarVisibility.Disabled;

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
