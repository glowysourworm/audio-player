using AudioPlayer.Component;
using AudioPlayer.Model.Interface;
using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace AudioPlayer.View.Converter
{
    public class MusicBrainzConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var entry = value as ILibraryEntry;

            if (entry != null)
            {
                try
                {
                    return MusicBrainzClient.Query(entry);
                }
                catch (Exception)
                {
                    return value;
                }
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
