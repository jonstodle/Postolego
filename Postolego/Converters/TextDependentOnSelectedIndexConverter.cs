using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Postolego.Converters {
    public class TextDependentOnSelectedIndexConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            var invalue = (int)value;
            string text = "";
            if(invalue < 1) {
                text = "UNREAD";
            } else if(invalue == 1) {
                text = "ARCHIVE";
            } else {
                text = "FAVORITES";
            }
            return text;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            return null;
        }
    }
}
