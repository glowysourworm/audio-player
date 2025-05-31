using System;
using System.Globalization;
using System.Linq;

using AudioPlayer.Component;
using AudioPlayer.Model;
using AudioPlayer.Model.Database;

using Avalonia.Data.Converters;

namespace AudioPlayer.View.Converter
{
    public class ArtworkFileConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            var fileName = (string)value;

            if (string.IsNullOrEmpty(fileName))
                return null;

            var fileRef = TagLib.File.Create(fileName);

            // Creates IImage "source" for the Avalonia Image control
            if (fileRef.Tag.Pictures.Any())
                return SerializableBitmap.ReadIPicture(fileRef.Tag.Pictures.First());

            else
                return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
