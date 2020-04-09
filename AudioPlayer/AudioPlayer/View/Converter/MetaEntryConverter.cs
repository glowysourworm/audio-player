using AudioPlayer.Model.Interface;
using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace AudioPlayer.View.Converter
{
    public class MetaEntryConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var entry = value as ILibraryMetaEntry;

            if (entry != null)
            {
                return entry.Album + " - " + entry.Title;
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
