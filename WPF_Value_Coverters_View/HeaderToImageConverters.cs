using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System.IO;

namespace WPF_Value_Coverters_View
{
    /// <summary>
    /// convert demand we add value to it so we implement the interface for IvalueConverter
    /// convert a full path to a specific image type of a file or folder
    /// </summary>
    [ValueConversion(typeof(string)/*source format*/, typeof(BitmapImage)/*target type*/)]
    public class HeaderToImageConverters : IValueConverter
    {
        public static HeaderToImageConverters Instance = new HeaderToImageConverters();
        //converter support  converting from image to string and convert it back to original value
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //return new object(); testing our image value

            //Getting the full path
            var path = (string)value;

            //Next we check if the path is null we can't do much so ignore
            if (path == null) return null;


            //pack://application:,,, special way of accessing a folder in wpf 
            //return new BitmapImage(new Uri($"pack://application:,,,/Images/Drive.jpeg"));

            //Get the name of the file/folder
            var name = MainWindow.GetFileFolderName(path);

            //By default we just presume an image
            var image = "Images.file.png";

            //We check if the name is blank, we presume it is a Drive as a we cannot have a blan file ot folder

            if (string.IsNullOrEmpty(name)) image = "Images/Drive.jpeg";
            //next we check if it is not file but folder and do that in c# we have to create new folder
            else if (new FileInfo(path).Attributes.HasFlag(FileAttributes.Directory)) //basically we r saying if this file path contain attributes then it a direct folder
                image = "Images/file close.png";
            return new BitmapImage(new Uri($"pack://application:,,,/{image}"));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
