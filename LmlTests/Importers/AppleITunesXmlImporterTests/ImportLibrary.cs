using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using twopointzero.Lml;
using twopointzero.Lml.Importers;

namespace twopointzero.LmlTests.Importers.AppleITunesXmlImporterTests
{
    [TestFixture]
    public class ImportLibrary
    {
        private static void ImportLibraryFromSampleFile(LibraryMode libraryMode, Action<Library> libraryAction)
        {
            using (var reader = new StreamReader(@"Importers\AppleITunesXmlImporterTests\mac iTunes Music Library.xml"))
            {
                var library = AppleITunesXmlImporter.ImportLibrary(reader, libraryMode);
                libraryAction(library);
            }
        }

        private static void FindItemAndAssertPropertyEquality(IEnumerable<Item> items, string artist, string title,
                                                              double? rating, DateTime? dateAdded, int? playCount, DateTime? lastPlayed,
                                                              string genre, string location, long? duration)
        {
            var item = items.First(o => o.Title == title);
            Assert.AreEqual(artist, item.Artist);
            if (rating == null)
            {
                Assert.IsNull(item.Rating);
            }
            else
            {
                Assert.IsNotNull(item.Rating);
                Assert.AreEqual(rating.Value, item.Rating.Value, 0.001);
            }
            Assert.AreEqual(dateAdded, item.DateAdded);
            Assert.AreEqual(playCount, item.PlayCount);
            Assert.AreEqual(lastPlayed, item.LastPlayed);
            Assert.AreEqual(genre, item.Genre);
            Assert.AreEqual(location, item.Location);
            Assert.AreEqual(duration, item.Duration);
        }

        [Test]
        public void GivenNullShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => AppleITunesXmlImporter.ImportLibrary(null, LibraryMode.Host));
        }

        [Test]
        public void GivenTheMacITunesMusicLibrarySampleFileShouldProduceTheExpectedGuestModeItems()
        {
            ImportLibraryFromSampleFile(
                LibraryMode.Guest,
                library =>
                    {
                        IEnumerable<Item> items = library.Items.ToList();

                        FindItemAndAssertPropertyEquality(
                            items,
                            "dj twopointzero", // artist
                            "Maraschino", // title
                            null, // rating
                            new DateTime(2010, 3, 23, 19, 55, 00, DateTimeKind.Utc), // dateAdded
                            null, // playCount
                            null, // lastPlayed
                            "Podcast", // genre
                            null, // location
                            null); // duration

                        FindItemAndAssertPropertyEquality(
                            items,
                            "Nine Inch Nails", // artist
                            "999,999", // title
                            null, // rating
                            new DateTime(2010, 3, 23, 19, 56, 22, DateTimeKind.Utc), // dateAdded
                            null, // playCount
                            null, // lastPlayed
                            null, // genre
                            null, // location
                            null); // duration

                        FindItemAndAssertPropertyEquality(
                            items,
                            "Depeche Mode", // artist
                            "Love, In Itself", // title
                            1, // rating
                            new DateTime(2010, 3, 23, 19, 56, 22, DateTimeKind.Utc), // dateAdded
                            null, // playCount
                            null, // lastPlayed
                            null, // genre
                            null, // location
                            null); // duration

                        FindItemAndAssertPropertyEquality(
                            items,
                            "Vitalic", // artist
                            "See The Sea (Red)", // title
                            null, // rating
                            new DateTime(2010, 3, 23, 19, 56, 22, DateTimeKind.Utc), // dateAdded
                            null, // playCount
                            null, // lastPlayed
                            "Electro", // genre
                            null, // location
                            null); // duration

                        FindItemAndAssertPropertyEquality(
                            items,
                            "Nine Inch Nails", // artist
                            "1,000,000", // title
                            null, // rating
                            new DateTime(2010, 3, 23, 19, 56, 22, DateTimeKind.Utc), // dateAdded
                            null, // playCount
                            null, // lastPlayed
                            null, // genre
                            null, // location
                            null); // duration

                        FindItemAndAssertPropertyEquality(
                            items,
                            "Depeche Mode", // artist
                            "More Than A Party", // title
                            null, // rating
                            new DateTime(2010, 3, 23, 19, 56, 22, DateTimeKind.Utc), // dateAdded
                            null, // playCount
                            null, // lastPlayed
                            null, // genre
                            null, // location
                            null); // duration

                        FindItemAndAssertPropertyEquality(
                            items,
                            "Vitalic", // artist
                            "Poison Lips", // title
                            null, // rating
                            new DateTime(2010, 3, 23, 19, 56, 22, DateTimeKind.Utc), // dateAdded
                            1, // playCount
                            new DateTime(2010, 3, 23, 20, 54, 50, DateTimeKind.Utc), // lastPlayed
                            "Electro", // genre
                            null, // location
                            null); // duration
                    }
                );
        }

        [Test]
        public void GivenTheMacITunesMusicLibrarySampleFileShouldProduceTheExpectedHostModeItems()
        {
            ImportLibraryFromSampleFile(
                LibraryMode.Host,
                library =>
                    {
                        IEnumerable<Item> items = library.Items.ToList();

                        FindItemAndAssertPropertyEquality(
                            items,
                            "dj twopointzero", // artist
                            "Maraschino", // title
                            null, // rating
                            new DateTime(2010, 3, 23, 19, 55, 00, DateTimeKind.Utc), // dateAdded
                            null, // playCount
                            null, // lastPlayed
                            "Podcast", // genre
                            "file://localhost/Users/jeremygray/Music/iTunes/iTunes%20Media/Podcasts/dj%20twopointzero%20%C2%BB%20Podcasts/Maraschino.m4a",
                            1780041); // duration

                        FindItemAndAssertPropertyEquality(
                            items,
                            "Nine Inch Nails", // artist
                            "999,999", // title
                            null, // rating
                            new DateTime(2010, 3, 23, 19, 56, 22, DateTimeKind.Utc), // dateAdded
                            null, // playCount
                            null, // lastPlayed
                            null, // genre
                            "file://localhost/Users/jeremygray/Music/iTunes/iTunes%20Media/Music/Nine%20Inch%20Nails/The%20Slip/1-01%20999,999.m4a",
                            85159); // duration

                        FindItemAndAssertPropertyEquality(
                            items,
                            "Depeche Mode", // artist
                            "Love, In Itself", // title
                            1, // rating
                            new DateTime(2010, 3, 23, 19, 56, 22, DateTimeKind.Utc), // dateAdded
                            null, // playCount
                            null, // lastPlayed
                            null, // genre
                            "file://localhost/Users/jeremygray/Music/iTunes/iTunes%20Media/Music/Depeche%20Mode/Construction%20Time%20Again/01%20Love,%20In%20Itself.m4a",
                            269840); // duration

                        FindItemAndAssertPropertyEquality(
                            items,
                            "Vitalic", // artist
                            "See The Sea (Red)", // title
                            null, // rating
                            new DateTime(2010, 3, 23, 19, 56, 22, DateTimeKind.Utc), // dateAdded
                            null, // playCount
                            null, // lastPlayed
                            "Electro", // genre
                            "file://localhost/Users/jeremygray/Music/iTunes/iTunes%20Media/Music/Vitalic/Flashmob/01%20See%20The%20Sea%20(Red).m4a",
                            244666); // duration

                        FindItemAndAssertPropertyEquality(
                            items,
                            "Nine Inch Nails", // artist
                            "1,000,000", // title
                            null, // rating
                            new DateTime(2010, 3, 23, 19, 56, 22, DateTimeKind.Utc), // dateAdded
                            null, // playCount
                            null, // lastPlayed
                            null, // genre
                            "file://localhost/Users/jeremygray/Music/iTunes/iTunes%20Media/Music/Nine%20Inch%20Nails/The%20Slip/1-02%201,000,000.m4a",
                            236197); // duration

                        FindItemAndAssertPropertyEquality(
                            items,
                            "Depeche Mode", // artist
                            "More Than A Party", // title
                            null, // rating
                            new DateTime(2010, 3, 23, 19, 56, 22, DateTimeKind.Utc), // dateAdded
                            null, // playCount
                            null, // lastPlayed
                            null, // genre
                            "file://localhost/Users/jeremygray/Music/iTunes/iTunes%20Media/Music/Depeche%20Mode/Construction%20Time%20Again/02%20More%20Than%20A%20Party.m4a",
                            285453); // duration

                        FindItemAndAssertPropertyEquality(
                            items,
                            "Vitalic", // artist
                            "Poison Lips", // title
                            null, // rating
                            new DateTime(2010, 3, 23, 19, 56, 22, DateTimeKind.Utc), // dateAdded
                            1, // playCount
                            new DateTime(2010, 3, 23, 20, 54, 50, DateTimeKind.Utc), // lastPlayed
                            "Electro", // genre
                            "file://localhost/Users/jeremygray/Music/iTunes/iTunes%20Media/Music/Vitalic/Flashmob/02%20Poison%20Lips.m4a",
                            232240); // duration
                    }
                );
        }

        [Test]
        public void GivenTheMacITunesMusicLibrarySampleFileShouldProduceTheExpectedLibrarySourceType()
        {
            ImportLibraryFromSampleFile(LibraryMode.Host, library => Assert.AreEqual("iTunes 9.0.3", library.SourceType));
        }

        [Test]
        public void GivenTheMacITunesMusicLibrarySampleFileShouldProduceTheExpectedLibraryVersion()
        {
            ImportLibraryFromSampleFile(LibraryMode.Host, library => Assert.AreEqual("1.0", library.Version));
        }

        [Test]
        public void GivenTheMacITunesMusicLibrarySampleFileShouldProduceTheExpectedNumberOfItems()
        {
            ImportLibraryFromSampleFile(LibraryMode.Host, library => Assert.AreEqual(7, library.Items.Count()));
        }
    }
}