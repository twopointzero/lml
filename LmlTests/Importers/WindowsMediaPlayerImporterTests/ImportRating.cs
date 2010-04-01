using System;
using NUnit.Framework;
using twopointzero.Lml.Importers;

namespace twopointzero.LmlTests.Importers.WindowsMediaPlayerImporterTests
{
    [TestFixture]
    public class ImportRating
    {
        private static void AssertIsEqual(string rating, double expected)
        {
            Assert.AreEqual(expected, WindowsMediaPlayerImporter.ImportRating(rating), 0.001);
        }

        [Test]
        public void Given13ShouldReturn30Percent()
        {
            AssertIsEqual("13", 0.3);
        }

        [Test]
        public void Given19ShouldReturn35Percent()
        {
            AssertIsEqual("19", 0.35);
        }

        [Test]
        public void Given1ShouldReturn20Percent()
        {
            AssertIsEqual("1", 0.2);
        }

        [Test]
        public void Given25ShouldReturn40Percent()
        {
            AssertIsEqual("25", 0.4);
        }

        [Test]
        public void Given50ShouldReturn60Percent()
        {
            AssertIsEqual("50", 0.6);
        }

        [Test]
        public void Given50WithDecimalPlaceShouldReturn60Percent()
        {
            AssertIsEqual("50.0", 0.6);
        }

        [Test]
        public void Given67Decimal5ShouldReturn70Percent()
        {
            AssertIsEqual("62.5", 0.7);
        }

        [Test]
        public void Given75ShouldReturn80Percent()
        {
            AssertIsEqual("75", 0.8);
        }

        [Test]
        public void Given87ShouldReturn80Percent()
        {
            AssertIsEqual("87", 0.9);
        }

        [Test]
        public void Given99ShouldReturn100Percent()
        {
            AssertIsEqual("99", 1.0);
        }

        [Test]
        public void GivenANonNumberShouldThrowArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => WindowsMediaPlayerImporter.ImportRating("a"));
        }

        [Test]
        public void GivenAbove99ShouldThrowArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => WindowsMediaPlayerImporter.ImportRating("100"));
        }

        [Test]
        public void GivenBelow0ShouldThrowArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => WindowsMediaPlayerImporter.ImportRating("-1"));
        }

        [Test]
        public void GivenEmptyShouldReturnNull()
        {
            Assert.IsNull(WindowsMediaPlayerImporter.ImportRating(string.Empty));
        }

        [Test]
        public void GivenNullShouldReturnNull()
        {
            Assert.IsNull(WindowsMediaPlayerImporter.ImportRating(null));
        }

        [Test]
        public void GivenZeroShouldReturnNull()
        {
            Assert.AreEqual(null, WindowsMediaPlayerImporter.ImportRating("0"));
        }
    }
}