using System;
using System.Globalization;

using AudioPlayer.Model.Vendor;
using AudioPlayer.ViewModel;

using Avalonia.Data.Converters;
using Avalonia.Media;

namespace AudioPlayer.View.Converter
{
    public class MusicBrainzValidColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return MainViewModel.DefaultMusicBrainzBackground;

            return (bool)value ? MainViewModel.ValidMusicBrainzBackground : MainViewModel.InvalidMusicBrainzBackground;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
