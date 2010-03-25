using System;
using System.Xml.Linq;
using NUnit.Framework;
using twopointzero.Lml.Importers;

namespace twopointzero.LmlTests.Importers.AppleITunesXmlImporterTests
{
    [TestFixture]
    public class GetPrimitiveValue
    {
        private static void AssertPrimitiveEntryIsEqual(string input, object expected)
        {
            object actual = GetPrimitiveEntry(input);
            Assert.AreEqual(expected, actual);
        }

        private static void AssertPrimitiveEntryIsEqual(string input, double expected, double delta)
        {
            object actual = GetPrimitiveEntry(input);
            Assert.AreEqual(expected, (double)actual, delta);
        }

        private static void AssertPrimitiveEntryIsEqual<T>(string input, T[] expected)
        {
            var actual = (T[])GetPrimitiveEntry(input);
            CollectionAssert.AreEqual(expected, actual);
        }

        private static object GetPrimitiveEntry(string input)
        {
            var valueNode = XDocument.Parse(input).FirstNode;
            var value = AppleITunesXmlImporter.GetPrimitiveValue(valueNode);
            Assert.IsNotNull(value);
            return value;
        }

        [Test]
        public void GivenNullShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => AppleITunesXmlImporter.GetPrimitiveEntries(null));
        }

        [Test]
        public void GivenOneDataValueShouldReturnIt()
        {
            const string input = @"<data>AQID</data>";
            var expected = new byte[] { 1, 2, 3 };
            AssertPrimitiveEntryIsEqual(input, expected);
        }

        [Test]
        public void GivenOneDateValueShouldReturnIt()
        {
            const string input = @"<date>2010-03-23T20:58:52Z</date>";
            var expected = new DateTime(2010, 03, 23, 20, 58, 52, DateTimeKind.Utc);
            AssertPrimitiveEntryIsEqual(input, expected);
        }

        [Test]
        public void GivenOneFalseValueShouldReturnIt()
        {
            const string input = @"<false/>";
            const bool expected = false;
            AssertPrimitiveEntryIsEqual(input, expected);
        }

        [Test]
        public void GivenOneIntegerValueShouldReturnIt()
        {
            const string input = @"<integer>1</integer>";
            const int expected = 1;
            AssertPrimitiveEntryIsEqual(input, expected);
        }

        [Test]
        public void GivenOneRealScientificValueShouldReturnIt()
        {
            const string input = @"<real>-0.42E+2</real>";
            const double expected = -42.0;
            AssertPrimitiveEntryIsEqual(input, expected, 0.001);
        }

        [Test]
        public void GivenOneRealValueShouldReturnIt()
        {
            const string input = @"<real>0.42</real>";
            const double expected = 0.42;
            AssertPrimitiveEntryIsEqual(input, expected, 0.001);
        }

        [Test]
        public void GivenOneStringValueShouldReturnIt()
        {
            const string input = @"<string>1</string>";
            const string expected = "1";
            AssertPrimitiveEntryIsEqual(input, expected);
        }

        [Test]
        public void GivenOneTrueValueShouldReturnIt()
        {
            const string input = @"<true/>";
            const bool expected = true;
            AssertPrimitiveEntryIsEqual(input, expected);
        }
    }
}