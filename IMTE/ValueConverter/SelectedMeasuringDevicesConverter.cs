using IMTE.IMTEEntity.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace IMTE.ValueConverter
{
    public class SelectedMeasuringDevicesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //if (value is IEnumerable<MeasuringDevice> devices)
            //{
            //    return new ObservableCollection<MeasuringDevice>(devices);
            //}

            return new ObservableCollection<MeasuringDevice>(value as IEnumerable<MeasuringDevice>);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
