using System;
using NUnit.Framework;
using twopointzero.Lml.Importers;

namespace twopointzero.LmlTests.Importers.WindowsMediaPlayerImporterTests
{
    [TestFixture]
    public class ImportDateTime
    {
        [Test]
        public void GivenALegalValueShouldReturnItsDateTimeRepresentation()
        {
            var expected = new DateTime(2010, 04, 24, 23, 43, 9, DateTimeKind.Local);
            var actual = WindowsMediaPlayerImporter.ImportDateTime("4/24/2010 11:43:09 PM");
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GivenAnIllegalValueShouldThrowFormatException()
        {
            Assert.Throws<FormatException>(() => WindowsMediaPlayerImporter.ImportDateTime("24/4/2010 11:43:09 PM"));
        }

        [Test]
        public void GivenEmptyShouldReturnNull()
        {
            Assert.IsNull(WindowsMediaPlayerImporter.ImportDateTime(string.Empty));
        }

        [Test]
        public void GivenNullShouldReturnNull()
        {
            Assert.IsNull(WindowsMediaPlayerImporter.ImportDateTime(null));
        }
    }
}