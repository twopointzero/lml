using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using twopointzero.Validation;

namespace twopointzero.Lml.Importers
{
    public static class WindowsMediaPlayerImporter
    {
        private const string LibraryVersion = "1.0";

        public static Library ImportLibrary(LibraryMode libraryMode)
        {
            return ImportLibrary(new WmpMediaCollection("audio"), libraryMode);
        }

        internal static Library ImportLibrary(WmpMediaCollection wmpMediaCollection, LibraryMode libraryMode)
        {
            Validator.IsNotNull(wmpMediaCollection, "wmpMediaCollection");

            return new Library(LibraryVersion, "Windows Media Player " + wmpMediaCollection.VersionInfo,
                               GetItems(wmpMediaCollection.Media, libraryMode));
        }

        private static IEnumerable<Item> GetItems(IEnumerable<WmpMedia> media, LibraryMode libraryMode)
        {
            return media.Select(o => GetItem(o, libraryMode));
        }

        private static Item GetItem(WmpMedia wmpMedia, LibraryMode libraryMode)
        {
            string artist = ImportString(wmpMedia.DisplayArtist);
            string title = ImportString(wmpMedia.Title);
            double? rating = ImportRating(wmpMedia.UserRating);
            int? playCount = ImportNullableInt32(wmpMedia.UserPlayCount);
            DateTime? lastPlayed = ImportDateTime(wmpMedia.UserLastPlayedTime);
            string genre = ImportString(wmpMedia.WmGenre);
            string location = null;
            long? duration = null;

            if (libraryMode == LibraryMode.Host)
            {
                location = ImportString(wmpMedia.SourceUrl);
                duration = ImportDuration(wmpMedia.Duration);
            }

            return new Item(artist, title, rating, playCount, lastPlayed, genre, location, duration);
        }

        private static string ImportString(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }
            return value;
        }

        internal static double? ImportRating(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            double parsedValue;
            if (!double.TryParse(value, out parsedValue) || parsedValue < 0 || parsedValue > 99)
            {
                throw new ArgumentOutOfRangeException("value", value,
                                                      "Expected a number between 0 and 99 (inclusive.)");
            }

            if (parsedValue == 0)
            {
                return null;
            }

            if (parsedValue <= 1)
            {
                return Scale(parsedValue, 0, 1, 0.0, 0.20);
            }
            if (parsedValue <= 25)
            {
                return Scale(parsedValue, 1, 25, 0.20, 0.40);
            }
            if (parsedValue <= 50)
            {
                return Scale(parsedValue, 25, 50, 0.40, 0.60);
            }
            if (parsedValue <= 75)
            {
                return Scale(parsedValue, 50, 75, 0.60, 0.80);
            }
            return Scale(parsedValue, 75, 99, 0.80, 1.0);
        }

        private static double Scale(double value, double fromMin, double fromMax, double toMin, double toMax)
        {
            return (value - fromMin) / (fromMax - fromMin) * (toMax - toMin) + toMin;
        }

        private static int? ImportNullableInt32(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }
            return int.Parse(value);
        }

        internal static DateTime? ImportDateTime(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            return DateTime.Parse(value, CultureInfo.InvariantCulture.DateTimeFormat);
        }

        private static long? ImportDuration(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }
            return (long)(double.Parse(value) * 1000);
        }
    }
}