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
        public void GivenMainDictButEmptyShouldReturnEmptyDictionary()
        {
            const string input =
                @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!DOCTYPE plist PUBLIC ""-//Apple Computer//DTD PLIST 1.0//EN"" ""http://www.apple.com/DTDs/PropertyList-1.0.dtd"">
<plist version=""1.0"">
<dict>
</dict>
</plist>";
            var reader = new StringReader(input);
            var actual = new AppleITunesXmlImporter().GetLibraryMetadata(reader);
            Assert.IsNotNull(actual);
            Assert.AreEqual(0, actual.Count);
        }

        [Test]
        public void GivenMultipleValuesShouldReturnThem()
        {
            const string input =
                @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!DOCTYPE plist PUBLIC ""-//Apple Computer//DTD PLIST 1.0//EN"" ""http://www.apple.com/DTDs/PropertyList-1.0.dtd"">
<plist version=""1.0"">
<dict>
	<key>Major Version</key><integer>1</integer>
	<key>Minor Version</key><integer>1</integer>
	<key>Application Version</key><string>8.0.2</string>
</dict>
</plist>";
            var reader = new StringReader(input);
            var actual = new AppleITunesXmlImporter().GetLibraryMetadata(reader);
            Assert.IsNotNull(actual);
            Assert.AreEqual(3, actual.Count);
            Assert.AreEqual(1, actual["MajorVersion"]);
            Assert.AreEqual(1, actual["MinorVersion"]);
            Assert.AreEqual("8.0.2", actual["ApplicationVersion"]);
        }

        [Test]
        public void GivenNoDtdShouldThrowArgumentOutOfRangeException()
        {
            const string input = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<plist version=""1.0"">
</plist>";
            var reader = new StringReader(input);
            Assert.Throws<ArgumentOutOfRangeException>(() => new AppleITunesXmlImporter().GetLibraryMetadata(reader));
        }

        [Test]
        public void GivenNoMainDictShouldThrowArgumentOutOfRangeException()
        {
            const string input =
                @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!DOCTYPE plist PUBLIC ""-//Apple Computer//DTD PLIST 1.0//EN"" ""http://www.apple.com/DTDs/PropertyList-1.0.dtd"">
<plist version=""1.0"">
</plist>";
            var reader = new StringReader(input);
            Assert.Throws<ArgumentOutOfRangeException>(() => new AppleITunesXmlImporter().GetLibraryMetadata(reader));
        }

        [Test]
        public void GivenNullShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new AppleITunesXmlImporter().GetLibraryMetadata(null));
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
            var actual = new AppleITunesXmlImporter().GetLibraryMetadata(reader);
            Assert.IsNotNull(actual);
            Assert.AreEqual(1, actual.Count);
            CollectionAssert.AreEqual(new byte[] { 1, 2, 3 }, (byte[])actual["MajorVersion"]);
        }

        [Test]
        public void GivenOneDateValueShouldReturnIt()
        {
            const string input =
                @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!DOCTYPE plist PUBLIC ""-//Apple Computer//DTD PLIST 1.0//EN"" ""http://www.apple.com/DTDs/PropertyList-1.0.dtd"">
<plist version=""1.0"">
<dict>
	<key>Major Version</key><date>2010-03-23T20:58:52Z</date>
</dict>
</plist>";
            var reader = new StringReader(input);
            var actual = new AppleITunesXmlImporter().GetLibraryMetadata(reader);
            Assert.IsNotNull(actual);
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual(new DateTime(2010, 03, 23, 20, 58, 52, DateTimeKind.Utc), actual["MajorVersion"]);
        }

        [Test]
        public void GivenOneFalseValueShouldReturnIt()
        {
            const string input =
                @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!DOCTYPE plist PUBLIC ""-//Apple Computer//DTD PLIST 1.0//EN"" ""http://www.apple.com/DTDs/PropertyList-1.0.dtd"">
<plist version=""1.0"">
<dict>
	<key>Major Version</key><false/>
</dict>
</plist>";
            var reader = new StringReader(input);
            var actual = new AppleITunesXmlImporter().GetLibraryMetadata(reader);
            Assert.IsNotNull(actual);
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual(false, actual["MajorVersion"]);
        }

        [Test]
        public void GivenOneIntegerValueShouldReturnIt()
        {
            const string input =
                @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!DOCTYPE plist PUBLIC ""-//Apple Computer//DTD PLIST 1.0//EN"" ""http://www.apple.com/DTDs/PropertyList-1.0.dtd"">
<plist version=""1.0"">
<dict>
	<key>Major Version</key><integer>1</integer>
</dict>
</plist>";
            var reader = new StringReader(input);
            var actual = new AppleITunesXmlImporter().GetLibraryMetadata(reader);
            Assert.IsNotNull(actual);
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual(1, actual["MajorVersion"]);
        }

        [Test]
        public void GivenOneRealScientificValueShouldReturnIt()
        {
            const string input =
                @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!DOCTYPE plist PUBLIC ""-//Apple Computer//DTD PLIST 1.0//EN"" ""http://www.apple.com/DTDs/PropertyList-1.0.dtd"">
<plist version=""1.0"">
<dict>
	<key>Major Version</key><real>-0.42E+2</real>
</dict>
</plist>";
            var reader = new StringReader(input);
            var actual = new AppleITunesXmlImporter().GetLibraryMetadata(reader);
            Assert.IsNotNull(actual);
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual(-42, (double)actual["MajorVersion"], 0.001);
        }

        [Test]
        public void GivenOneRealValueShouldReturnIt()
        {
            const string input =
                @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!DOCTYPE plist PUBLIC ""-//Apple Computer//DTD PLIST 1.0//EN"" ""http://www.apple.com/DTDs/PropertyList-1.0.dtd"">
<plist version=""1.0"">
<dict>
	<key>Major Version</key><real>0.42</real>
</dict>
</plist>";
            var reader = new StringReader(input);
            var actual = new AppleITunesXmlImporter().GetLibraryMetadata(reader);
            Assert.IsNotNull(actual);
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual(0.42, (double)actual["MajorVersion"], 0.001);
        }

        [Test]
        public void GivenOneStringValueShouldReturnIt()
        {
            const string input =
                @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!DOCTYPE plist PUBLIC ""-//Apple Computer//DTD PLIST 1.0//EN"" ""http://www.apple.com/DTDs/PropertyList-1.0.dtd"">
<plist version=""1.0"">
<dict>
	<key>Major Version</key><string>1</string>
</dict>
</plist>";
            var reader = new StringReader(input);
            var actual = new AppleITunesXmlImporter().GetLibraryMetadata(reader);
            Assert.IsNotNull(actual);
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual("1", actual["MajorVersion"]);
        }

        [Test]
        public void GivenOneTrueValueShouldReturnIt()
        {
            const string input =
                @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!DOCTYPE plist PUBLIC ""-//Apple Computer//DTD PLIST 1.0//EN"" ""http://www.apple.com/DTDs/PropertyList-1.0.dtd"">
<plist version=""1.0"">
<dict>
	<key>Major Version</key><true/>
</dict>
</plist>";
            var reader = new StringReader(input);
            var actual = new AppleITunesXmlImporter().GetLibraryMetadata(reader);
            Assert.IsNotNull(actual);
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual(true, actual["MajorVersion"]);
        }

        [Test]
        public void GivenRootNodeWithWrongVersionShouldThrowArgumentOutOfRangeException()
        {
            const string input =
                @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!DOCTYPE plist PUBLIC ""-//Apple Computer//DTD PLIST 1.0//EN"" ""http://www.apple.com/DTDs/PropertyList-1.0.dtd"">
<plist version=""0.0"">
</plist>";
            var reader = new StringReader(input);
            Assert.Throws<ArgumentOutOfRangeException>(() => new AppleITunesXmlImporter().GetLibraryMetadata(reader));
        }

        [Test]
        public void GivenRootNodeWithoutVersionShouldThrowArgumentOutOfRangeException()
        {
            const string input =
                @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!DOCTYPE plist PUBLIC ""-//Apple Computer//DTD PLIST 1.0//EN"" ""http://www.apple.com/DTDs/PropertyList-1.0.dtd"">
<plist>
</plist>";
            var reader = new StringReader(input);
            Assert.Throws<ArgumentOutOfRangeException>(() => new AppleITunesXmlImporter().GetLibraryMetadata(reader));
        }

        [Test]
        public void GivenWrongDtdNameShouldThrowArgumentOutOfRangeException()
        {
            const string input =
                @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!DOCTYPE zlist PUBLIC ""-//Apple Computer//DTD PLIST 1.0//EN"" ""http://www.apple.com/DTDs/PropertyList-1.0.dtd"">
<plist version=""1.0"">
</plist>";
            var reader = new StringReader(input);
            Assert.Throws<ArgumentOutOfRangeException>(() => new AppleITunesXmlImporter().GetLibraryMetadata(reader));
        }

        [Test]
        public void GivenWrongDtdPublicIdShouldThrowArgumentOutOfRangeException()
        {
            const string input =
                @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!DOCTYPE plist PUBLIC ""-//Snapple Computer//DTD PLIST 1.0//EN"" ""http://www.apple.com/DTDs/PropertyList-1.0.dtd"">
<plist version=""1.0"">
</plist>";
            var reader = new StringReader(input);
            Assert.Throws<ArgumentOutOfRangeException>(() => new AppleITunesXmlImporter().GetLibraryMetadata(reader));
        }

        [Test]
        public void GivenWrongDtdSystemIdShouldThrowArgumentOutOfRangeException()
        {
            const string input =
                @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!DOCTYPE plist PUBLIC ""-//Apple Computer//DTD PLIST 1.0//EN"" ""http://www.snapple.com/DTDs/PropertyList-1.0.dtd"">
<plist version=""1.0"">
</plist>";
            var reader = new StringReader(input);
            Assert.Throws<ArgumentOutOfRangeException>(() => new AppleITunesXmlImporter().GetLibraryMetadata(reader));
        }

        [Test]
        public void GivenWrongRootNodeShouldThrowArgumentOutOfRangeException()
        {
            const string input =
                @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!DOCTYPE plist PUBLIC ""-//Apple Computer//DTD PLIST 1.0//EN"" ""http://www.apple.com/DTDs/PropertyList-1.0.dtd"">
<zlist>
</zlist>";
            var reader = new StringReader(input);
            Assert.Throws<ArgumentOutOfRangeException>(() => new AppleITunesXmlImporter().GetLibraryMetadata(reader));
        }
    }
}