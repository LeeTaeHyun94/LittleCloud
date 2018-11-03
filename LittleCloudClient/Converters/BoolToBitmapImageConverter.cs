using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace LittleCloudClient.Converters
{
    public class BoolToBitmapImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
            {
                Uri uriSource = new Uri(@"/LittleCloudClient;component/Resources/if_Account_online.png", UriKind.Relative);
                return new BitmapImage(uriSource);
            }
            else
            {
                Uri uriSource = new Uri(@"/LittleCloudClient;component/Resources/if_Account_offline.png", UriKind.Relative);
                return new BitmapImage(uriSource);
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
