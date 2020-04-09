using AudioPlayer.Model;
using AudioPlayer.Model.Interface;

using Avalonia.Data.Converters;

using System;
using System.Globalization;

namespace AudioPlayer.View.Converter
{
    public class ArtworkCollectionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var entry = value as ILibraryEntry;

            if (entry != null)
            {
                if (LibraryManager.CurrentLibrary.Database.ContainsArtworkFor(entry))
                    return LibraryManager.CurrentLibrary.Database.GetArtwork(entry);
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
