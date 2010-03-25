using System;
using System.IO;
using NUnit.Framework;
using twopointzero.Lml.Importers;

namespace twopointzero.LmlTests.Importers.AppleITunesXmlImporterTests
{
    [TestFixture]
    public class GetLibraryMetadata
    {
        [Test]
        public void GivenNullShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => AppleITunesXmlImporter.GetLibraryMetadata(null));
        }

        [Test]
        public void GivenOneDataValueShouldReturnIt()
        {
            const string input =
                @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!DOCTYPE plist PUBLIC ""-//Apple Computer//DTD PLIST 1.0//EN"" ""http://www.apple.com/DTDs/PropertyList-1.0.dtd"">
<plist version=""1.0"">
<dict>
	<key>Major Version</key><data>AQID</data>
</dict>
</plist>";
            var reader = new StringReader(input);
            var actual = AppleITunesXmlImporter.GetLibraryMetadata(reader);
            Assert.IsNotNull(actual);
            Assert.AreEqual(1, actual.Count);
            CollectionAssert.AreEqual(new byte[] { 1, 2, 3 }, (byte[])actual["MajorVersion"]);
        }
    }
}