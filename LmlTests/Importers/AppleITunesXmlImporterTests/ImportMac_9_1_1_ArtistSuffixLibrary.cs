using System;
using System.Linq;
using NUnit.Framework;
using twopointzero.Lml.Importers;

namespace twopointzero.LmlTests.Importers.AppleITunesXmlImporterTests
{
    [TestFixture]
    public class ImportMac_9_1_1_ArtistSuffixLibrary : ImportLibrary
    {
        protected override string LibraryPath
        {
            get { return @"Importers\AppleITunesXmlImporterTests\mac iTML 9_1_1 - artist suffixes.xml"; }
        }

        [Test]
        public void GivenNullShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => AppleITunesXmlImporter.ImportLibrary(null));
        }

        [Test]
        public void GivenTheFileShouldProduceTheExpectedItems()
        {
            ImportLibraryFromFileAndAssertItems();
        }

        [Test]
        public void GivenTheFileShouldProduceTheExpectedLibrarySourceType()
        {
            ImportLibraryFromFile(library => Assert.AreEqual("iTunes 9.1.1", library.SourceType));
        }

        [Test]
        public void GivenTheFileShouldProduceTheExpectedLibraryVersion()
        {
            ImportLibraryFromFile(library => Assert.AreEqual("1.1", library.Version));
        }

        [Test]
        public void GivenTheFileShouldProduceTheExpectedNumberOfItems()
        {
            ImportLibraryFromFile(library => Assert.AreEqual(6, library.Items.Count()));
        }
    }
}