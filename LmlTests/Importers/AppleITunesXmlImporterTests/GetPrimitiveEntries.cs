using System;
using System.Xml.Linq;
using NUnit.Framework;
using twopointzero.Lml.Importers;

namespace twopointzero.LmlTests.Importers.AppleITunesXmlImporterTests
{
    [TestFixture]
    public class GetPrimitiveEntries
    {
        [Test]
        public void GivenMainDictButEmptyShouldReturnEmptyDictionary()
        {
            var dictNode = new XElement("dict");
            var actual = AppleITunesXmlImporter.GetPrimitiveEntries(dictNode);
            Assert.IsNotNull(actual);
            Assert.AreEqual(0, actual.Count);
        }

        [Test]
        public void GivenMultipleValuesShouldReturnThem()
        {
            const string input =
                @"
<dict>
	<key>Major Version</key><integer>1</integer>
	<key>Minor Version</key><integer>2</integer>
</dict>";
            var dictNode = XDocument.Parse(input).Element("dict");
            var actual = AppleITunesXmlImporter.GetPrimitiveEntries(dictNode);
            Assert.IsNotNull(actual);
            Assert.AreEqual(2, actual.Count);
            Assert.AreEqual(1, actual["MajorVersion"]);
            Assert.AreEqual(2, actual["MinorVersion"]);
        }

        [Test]
        public void GivenNullShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => AppleITunesXmlImporter.GetPrimitiveEntries(null));
        }

        [Test]
        public void GivenOneValueShouldReturnIt()
        {
            const string input = @"
<dict>
	<key>Major Version</key><integer>1</integer>
</dict>";
            var dictNode = XDocument.Parse(input).Element("dict");
            var actual = AppleITunesXmlImporter.GetPrimitiveEntries(dictNode);
            Assert.IsNotNull(actual);
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual(1, actual["MajorVersion"]);
        }
    }
}