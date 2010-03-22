using System;
using System.Xml.Linq;
using NUnit.Framework;
using twopointzero.Lml;

namespace twopointzero.LmlTests.LmlXmlSerializerTests
{
    [TestFixture]
    public class ToItem
    {
        [Test]
        public void GivenGuestItemShouldProduceExpectedResult()
        {
            var item = Mother.CreateItemXElement("Artist", "Title", "0.42", "69", "2010-01-01T00:00:00", "Genre");
            var expected = new Item("Artist", "Title", 0.42, 69, new DateTime(2010, 1, 1), "Genre", null);
            var actual = new LmlXmlSerializer().ToItem(item);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GivenHostItemShouldProduceExpectedResult()
        {
            var item = Mother.CreateItemXElement("Artist", "Title", "0.42", "69", "2010-01-01T00:00:00", "Genre",
                                                 @"C:\path\file.ext");
            var expected = new Item("Artist", "Title", 0.42, 69, new DateTime(2010, 1, 1), "Genre", @"C:\path\file.ext");
            var actual = new LmlXmlSerializer().ToItem(item);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GivenNonItemShouldThrowArgumentNullException()
        {
            var item = new XElement("z");
            Assert.Throws<ArgumentOutOfRangeException>(() => new LmlXmlSerializer().ToItem(item));
        }

        [Test]
        public void GivenNullItemShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new LmlXmlSerializer().ToItem(null));
        }
    }
}