using AudioPlayer.Extension;
using AudioPlayer.Model;
using AudioPlayer.Model.Interface;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace AudioPlayer.Component
{
    public static class LibraryEntryLoader
    {
        const string UNKNOWN = "Unknown";

        public static ILibraryEntry Load(string file)
        {
            if (string.IsNullOrEmpty(file))
                throw new ArgumentException("Invalid media file name");

            try
            {

                var fileRef = TagLib.File.Create(file);

                return new LibraryEntry(file)
                {
                    AlbumArtists = new SortedObservableCollection<string, string>(fileRef.Tag.AlbumArtists.Distinct(), x => x),
                    Composers = new SortedObservableCollection<string, string>(fileRef.Tag.Composers.Distinct(), x => x),
                    Genres = new SortedObservableCollection<string, string>(fileRef.Tag.Genres.Distinct(), x => x),
                    Performers = new SortedObservableCollection<string, string>(fileRef.Tag.Performers.Distinct(), x => x),

                    Album = Format(fileRef.Tag.Album),
                    Disc = fileRef.Tag.Disc,
                    DiscCount = fileRef.Tag.DiscCount,
                    Title = Format(fileRef.Tag.Title),
                    Track = fileRef.Tag.Track,
                    Year = fileRef.Tag.Year
                };
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static bool IsUnknown<T>(this ILibraryEntry entry, Expression<Func<ILibraryEntry, T>> propertyExpression)
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
            else if (propertyInfo.PropertyType == typeof(IEnumerable<string>))
            {
                var collection = propertyValue as IEnumerable<string>;

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
