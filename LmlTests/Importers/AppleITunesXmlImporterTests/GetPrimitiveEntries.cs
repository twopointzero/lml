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
            const string input = @"
<dict>
	<key>Major Version</key><integer>1</integer>
	<key>Minor Version</key><integer>1</integer>
	<key>Application Version</key><string>8.0.2</string>
</dict>";
            var dictNode = XDocument.Parse(input).Element("dict");
            var actual = AppleITunesXmlImporter.GetPrimitiveEntries(dictNode);
            Assert.IsNotNull(actual);
            Assert.AreEqual(3, actual.Count);
            Assert.AreEqual(1, actual["MajorVersion"]);
            Assert.AreEqual(1, actual["MinorVersion"]);
            Assert.AreEqual("8.0.2", actual["ApplicationVersion"]);
        }

        [Test]
        public void GivenNullShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => AppleITunesXmlImporter.GetPrimitiveEntries(null));
        }

        [Test]
        public void GivenOneDataValueShouldReturnIt()
        {
            const string input = @"
<dict>
	<key>Major Version</key><data>AQID</data>
</dict>";
            var dictNode = XDocument.Parse(input).Element("dict");
            var actual = AppleITunesXmlImporter.GetPrimitiveEntries(dictNode);
            Assert.IsNotNull(actual);
            Assert.AreEqual(1, actual.Count);
            CollectionAssert.AreEqual(new byte[] { 1, 2, 3 }, (byte[])actual["MajorVersion"]);
        }

        [Test]
        public void GivenOneDateValueShouldReturnIt()
        {
            const string input = @"
<dict>
	<key>Major Version</key><date>2010-03-23T20:58:52Z</date>
</dict>";
            var dictNode = XDocument.Parse(input).Element("dict");
            var actual = AppleITunesXmlImporter.GetPrimitiveEntries(dictNode);
            Assert.IsNotNull(actual);
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual(new DateTime(2010, 03, 23, 20, 58, 52, DateTimeKind.Utc), actual["MajorVersion"]);
        }

        [Test]
        public void GivenOneFalseValueShouldReturnIt()
        {
            const string input = @"
<dict>
	<key>Major Version</key><false/>
</dict>";
            var dictNode = XDocument.Parse(input).Element("dict");
            var actual = AppleITunesXmlImporter.GetPrimitiveEntries(dictNode);
            Assert.IsNotNull(actual);
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual(false, actual["MajorVersion"]);
        }

        [Test]
        public void GivenOneIntegerValueShouldReturnIt()
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

        [Test]
        public void GivenOneRealScientificValueShouldReturnIt()
        {
            const string input = @"
<dict>
	<key>Major Version</key><real>-0.42E+2</real>
</dict>";
            var dictNode = XDocument.Parse(input).Element("dict");
            var actual = AppleITunesXmlImporter.GetPrimitiveEntries(dictNode);
            Assert.IsNotNull(actual);
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual(-42, (double)actual["MajorVersion"], 0.001);
        }

        [Test]
        public void GivenOneRealValueShouldReturnIt()
        {
            const string input = @"
<dict>
	<key>Major Version</key><real>0.42</real>
</dict>";
            var dictNode = XDocument.Parse(input).Element("dict");
            var actual = AppleITunesXmlImporter.GetPrimitiveEntries(dictNode);
            Assert.IsNotNull(actual);
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual(0.42, (double)actual["MajorVersion"], 0.001);
        }

        [Test]
        public void GivenOneStringValueShouldReturnIt()
        {
            const string input = @"
<dict>
	<key>Major Version</key><string>1</string>
</dict>";
            var dictNode = XDocument.Parse(input).Element("dict");
            var actual = AppleITunesXmlImporter.GetPrimitiveEntries(dictNode);
            Assert.IsNotNull(actual);
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual("1", actual["MajorVersion"]);
        }

        [Test]
        public void GivenOneTrueValueShouldReturnIt()
        {
            const string input = @"
<dict>
	<key>Major Version</key><true/>
</dict>";
            var dictNode = XDocument.Parse(input).Element("dict");
            var actual = AppleITunesXmlImporter.GetPrimitiveEntries(dictNode);
            Assert.IsNotNull(actual);
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual(true, actual["MajorVersion"]);
        }
    }
}