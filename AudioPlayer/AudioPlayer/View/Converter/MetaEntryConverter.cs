using System;
using System.Globalization;

using Avalonia.Data.Converters;

namespace AudioPlayer.View.Converter
{
    public class MetaEntryConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //var entry = value as ILibraryMetaEntry;

            //if (entry != null)
            //{
            //    return entry.Album + " - " + entry.Title;
            //}

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
