using System;
using System.Globalization;

using AudioPlayer.Component;
using AudioPlayer.Model;

using Avalonia.Data.Converters;

namespace AudioPlayer.View.Converter
{
    public class MusicBrainzConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //var entry = value as LibraryEntry;

            //if (entry != null)
            //{
            //    try
            //    {
            //        return MusicBrainzClient.Query(entry);
            //    }
            //    catch (Exception)
            //    {
            //        return value;
            //    }
            //}

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
