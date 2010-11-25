using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using twopointzero.Validation;

namespace twopointzero.Lml.Importers
{
    public static class WindowsMediaPlayerImporter
    {
        public static Library ImportLibrary()
        {
            return ImportLibrary(new WmpMediaCollection("audio"));
        }

        internal static Library ImportLibrary(WmpMediaCollection wmpMediaCollection)
        {
            Validator.IsNotNull(wmpMediaCollection, "wmpMediaCollection");

            return new Library("1.1", "Windows Media Player " + wmpMediaCollection.VersionInfo,
                               GetItems(wmpMediaCollection.Media));
        }

        private static IEnumerable<Item> GetItems(IEnumerable<WmpMedia> media)
        {
            return media.Select(o => GetItem(o));
        }

        private static Item GetItem(WmpMedia wmpMedia)
        {
            string artist = ImportString(wmpMedia.DisplayArtist);
            string album = ImportString(wmpMedia.Album);
            string title = ImportString(wmpMedia.Title);
            double? rating = ImportRating(wmpMedia.UserRating);
            DateTime? dateAdded = ImportDateTime(wmpMedia.AcquisitionTime);
            int? playCount = ImportNullableInt32(wmpMedia.UserPlayCount);
            DateTime? lastPlayed = ImportDateTime(wmpMedia.UserLastPlayedTime);
            string genre = ImportString(wmpMedia.WmGenre);
            string location = ImportString(wmpMedia.SourceUrl);
            TimeSpan? duration = ImportDuration(wmpMedia.Duration);
            int? bitsPerSecond = ImportNullableInt32(wmpMedia.Bitrate);

            return new Item(artist, album, title, rating, dateAdded, playCount, lastPlayed, genre, location, duration,
                            bitsPerSecond);
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

        private static TimeSpan? ImportDuration(string seconds)
        {
            if (string.IsNullOrEmpty(seconds))
            {
                return null;
            }
            return TimeSpan.FromSeconds(double.Parse(seconds));
        }
    }
}