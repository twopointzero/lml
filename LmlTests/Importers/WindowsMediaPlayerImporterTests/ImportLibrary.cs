using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using twopointzero.Lml;
using twopointzero.Lml.Importers;

namespace twopointzero.LmlTests.Importers.WindowsMediaPlayerImporterTests
{
    [TestFixture]
    public class ImportLibrary
    {
        [Test]
        public void GivenAnInputShouldReturnNonNull()
        {
            var input = new WmpMediaCollection("12.0.7600.16415", Enumerable.Empty<WmpMedia>());
            var actual = WindowsMediaPlayerImporter.ImportLibrary(input, LibraryMode.Host);
            Assert.IsNotNull(actual);
        }

        [Test]
        public void GivenAnInputShouldReturnTheExpectedSourceType()
        {
            var input = new WmpMediaCollection("12.0.7600.16415", Enumerable.Empty<WmpMedia>());
            var library = WindowsMediaPlayerImporter.ImportLibrary(input, LibraryMode.Host);
            Assert.AreEqual("Windows Media Player 12.0.7600.16415", library.SourceType);
        }

        [Test]
        public void GivenAnInputShouldReturnTheExpectedVersion()
        {
            var input = new WmpMediaCollection("12.0.7600.16415", Enumerable.Empty<WmpMedia>());
            var library = WindowsMediaPlayerImporter.ImportLibrary(input, LibraryMode.Host);
            Assert.AreEqual("1.0", library.Version);
        }

        [Test]
        public void GivenEmptyShouldReturnEmpty()
        {
            var input = new WmpMediaCollection("12.0.7600.16415", Enumerable.Empty<WmpMedia>());
            var library = WindowsMediaPlayerImporter.ImportLibrary(input, LibraryMode.Host);
            CollectionAssert.AreEqual(Enumerable.Empty<WmpMedia>(), library.Items);
        }

        [Test]
        public void GivenNullShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => WindowsMediaPlayerImporter.ImportLibrary(null, LibraryMode.Host));
        }

        [Test]
        public void GivenThreeInGuestModeShouldReturnTheExpectedItems()
        {
            var media1 = new WmpMedia("artist1", "title", "", "", "", "", "", @"C:\Data\sample.file", "123.456");
            var media2 = new WmpMedia("artist2", "title", "0", "4/1/2009 4:17:32 PM", "0", "4/1/2010 4:17:32 PM", "",
                                      @"C:\Data\sample.file", "123.456");
            var media3 = new WmpMedia("artist3", "title", "50", "4/1/2009 4:17:32 PM", "42", "4/1/2010 4:17:32 PM",
                                      "Genre", @"C:\Data\sample.file", "123.456");
            IEnumerable<WmpMedia> media = new[] { media1, media2, media3 };
            var input = new WmpMediaCollection("12.0.7600.16415", media);

            var expected1 = new Item("artist1", "title", null, null, null, null, null, null, null);
            var expected2 = new Item("artist2", "title", null, new DateTime(2009, 4, 1, 16, 17, 32), 0,
                                     new DateTime(2010, 4, 1, 16, 17, 32), null, null, null);
            var expected3 = new Item("artist3", "title", 0.6, new DateTime(2009, 4, 1, 16, 17, 32), 42,
                                     new DateTime(2010, 4, 1, 16, 17, 32), "Genre", null, null);

            var library = WindowsMediaPlayerImporter.ImportLibrary(input, LibraryMode.Guest);
            var results = library.Items.ToArray();

            Assert.AreEqual(expected1, results[0]);
            Assert.AreEqual(expected2, results[1]);
            Assert.AreEqual(expected3, results[2]);
        }

        [Test]
        public void GivenThreeInHostModeShouldReturnTheExpectedItems()
        {
            var media1 = new WmpMedia("artist1", "title", "", "", "", "", "", @"C:\Data\sample.file", "123.456");
            var media2 = new WmpMedia("artist2", "title", "0", "4/1/2009 4:17:32 PM", "0", "4/1/2010 4:17:32 PM", "",
                                      @"C:\Data\sample.file", "123.456");
            var media3 = new WmpMedia("artist3", "title", "50", "4/1/2009 4:17:32 PM", "42", "", "Genre",
                                      @"C:\Data\sample.file", "123.456");
            IEnumerable<WmpMedia> media = new[] { media1, media2, media3 };
            var input = new WmpMediaCollection("12.0.7600.16415", media);

            var expected1 = new Item("artist1", "title", null, null, null, null, null, @"C:\Data\sample.file", 123456);
            var expected2 = new Item("artist2", "title", null, new DateTime(2009, 4, 1, 16, 17, 32), 0,
                                     new DateTime(2010, 4, 1, 16, 17, 32), null, @"C:\Data\sample.file", 123456);
            var expected3 = new Item("artist3", "title", 0.6, new DateTime(2009, 4, 1, 16, 17, 32), 42, null, "Genre",
                                     @"C:\Data\sample.file", 123456);

            var library = WindowsMediaPlayerImporter.ImportLibrary(input, LibraryMode.Host);
            var results = library.Items.ToArray();

            Assert.AreEqual(expected1, results[0]);
            Assert.AreEqual(expected2, results[1]);
            Assert.AreEqual(expected3, results[2]);
        }

        [Test]
        public void GivenThreeShouldReturnThree()
        {
            IEnumerable<WmpMedia> media = Enumerable.Empty<WmpMedia>();
            var input = new WmpMediaCollection("12.0.7600.16415", media);
            var library = WindowsMediaPlayerImporter.ImportLibrary(input, LibraryMode.Host);
            Assert.AreEqual(media.Count(), library.Items.Count());
        }
    }
}