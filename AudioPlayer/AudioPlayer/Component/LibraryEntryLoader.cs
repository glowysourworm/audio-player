using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using AudioPlayer.Extension;
using AudioPlayer.Model;
using AudioPlayer.Model.Database;

namespace AudioPlayer.Component
{
    public static class LibraryEntryLoader
    {
        const string UNKNOWN = "Unknown";

        public static LibraryEntry Load(string file)
        {
            if (string.IsNullOrEmpty(file))
                throw new ArgumentException("Invalid media file name");

            try
            {

                var fileRef = TagLib.File.Create(file);

                return new LibraryEntry(file)
                {
                    AlbumArtists = new SortedObservableCollection<Artist>(fileRef.Tag.AlbumArtists.Where(z => !string.IsNullOrEmpty(z)).Distinct().Select(x => new Artist()
                    {
                        Name = x
                    })),
                    Genres = new SortedObservableCollection<string>(fileRef.Tag.Genres.Where(z => !string.IsNullOrEmpty(z)).Distinct()),

                    AlbumArt = new SortedObservableCollection<SerializableBitmap>(fileRef.Tag.Pictures.Select(x => SerializableBitmap.ReadIPicture(x))),
                    Album = Format(fileRef.Tag.Album),
                    Disc = fileRef.Tag.Disc,
                    DiscCount = fileRef.Tag.DiscCount,
                    Duration = fileRef.Properties.Duration,
                    Title = Format(fileRef.Tag.Title),
                    Track = fileRef.Tag.Track,
                    Year = fileRef.Tag.Year
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool IsUnknown<T>(this LibraryEntry entry, Expression<Func<LibraryEntry, T>> propertyExpression)
        {
            var expression = (MemberExpression)propertyExpression.Body;
            var propertyInfo = (PropertyInfo)expression.Member;
            var propertyValue = propertyInfo.GetValue(entry);

            // String
            if (propertyInfo.PropertyType == typeof(string))
            {
                if (string.IsNullOrWhiteSpace((string)propertyValue))
                    return true;

                else if ((string)propertyValue == UNKNOWN)
                    return true;

                else
                    return false;
            }

            // Collections of strings
            else if (propertyInfo.PropertyType == typeof(SortedObservableCollection<string>))
            {
                var collection = propertyValue as SortedObservableCollection<string>;

                return collection.Count() == 0;
            }

            // uint
            else if (propertyInfo.PropertyType == typeof(uint))
            {
                return (uint)propertyValue <= 0;
            }

            else
                throw new Exception("Unhandled unknown field type LibraryEntry.IsUnknown");
        }

        private static string Format(string tagField)
        {
            return string.IsNullOrWhiteSpace(tagField) ? UNKNOWN : tagField;
        }
    }
}
