using System;
using System.Linq;
using System.Xml.Linq;
using NUnit.Framework;
using twopointzero.Lml;

namespace twopointzero.LmlTests.LmlXmlSerializerTests
{
    [TestFixture]
    public class ToXElement
    {
        [Test]
        public void GivenEmptyLibraryShouldProduceExpectedResult()
        {
            var library = new Library("1.0", "Unit Tests", Enumerable.Empty<Item>());

            var expected = new XElement("l",
                                        new XAttribute("v", "1.0"),
                                        new XAttribute("st", "Unit Tests"));

            var actual = new LmlXmlSerializer().ToXElement(library);
            Assert.IsTrue(XNode.DeepEquals(expected, actual));
        }

        [Test]
        public void GivenGuestItemShouldProduceExpectedResult()
        {
            var item = new Item("Artist", "Title", 0.42, 69, new DateTime(2010, 1, 1), "Genre", null, null);
            var expected = Mother.CreateItemXElement("Artist", "Title", "0.42", "69", "2010-01-01T00:00:00", "Genre");
            var actual = new LmlXmlSerializer().ToXElement(item);
            Assert.IsTrue(XNode.DeepEquals(expected, actual));
        }

        [Test]
        public void GivenGuestLibraryShouldProduceExpectedResult()
        {
            var item1 = new Item("Artist1", "Title1", 0.421, 691, new DateTime(2010, 1, 11), "Genre1", null, null);
            var item2 = new Item("Artist2", "Title2", 0.422, 692, new DateTime(2010, 1, 12), "Genre2", null, null);
            var item3 = new Item("Artist3", "Title3", 0.423, 693, new DateTime(2010, 1, 13), "Genre3", null, null);
            var library = new Library("1.0", "Unit Tests", new[] { item1, item2, item3 });

            var expectedItem1 = Mother.CreateItemXElement("Artist1", "Title1", "0.421", "691", "2010-01-11T00:00:00",
                                                          "Genre1");
            var expectedItem2 = Mother.CreateItemXElement("Artist2", "Title2", "0.422", "692", "2010-01-12T00:00:00",
                                                          "Genre2");
            var expectedItem3 = Mother.CreateItemXElement("Artist3", "Title3", "0.423", "693", "2010-01-13T00:00:00",
                                                          "Genre3");
            var expected = new XElement("l",
                                        new XAttribute("v", "1.0"),
                                        new XAttribute("st", "Unit Tests"),
                                        expectedItem1,
                                        expectedItem2,
                                        expectedItem3);

            var actual = new LmlXmlSerializer().ToXElement(library);
            Assert.IsTrue(XNode.DeepEquals(expected, actual));
        }

        [Test]
        public void GivenHostItemShouldProduceExpectedResult()
        {
            var item = new Item("Artist", "Title", 0.42, 69, new DateTime(2010, 1, 1), "Genre", @"C:\path\file.ext", 74);
            var expected = Mother.CreateItemXElement("Artist", "Title", "0.42", "69", "2010-01-01T00:00:00", "Genre",
                                                     @"C:\path\file.ext", "74");
            var actual = new LmlXmlSerializer().ToXElement(item);
            Assert.IsTrue(XNode.DeepEquals(expected, actual));
        }

        [Test]
        public void GivenHostItemWithEmptyAttributesShouldProduceExpectedResult()
        {
            var item = new Item("", "", null, null, null, "", "", null);
            var expected = new XElement("i");
            var actual = new LmlXmlSerializer().ToXElement(item);
            Assert.IsTrue(XNode.DeepEquals(expected, actual));
        }

        [Test]
        public void GivenHostItemWithNullAttributesShouldProduceExpectedResult()
        {
            var item = new Item(null, null, null, null, null, null, null, null);
            var expected = new XElement("i");
            var actual = new LmlXmlSerializer().ToXElement(item);
            Assert.IsTrue(XNode.DeepEquals(expected, actual));
        }

        [Test]
        public void GivenHostLibraryShouldProduceExpectedResult()
        {
            var item1 = new Item("Artist1", "Title1", 0.421, 691, new DateTime(2010, 1, 11), "Genre1",
                                 @"C:\path\file1.ext", 741);
            var item2 = new Item("Artist2", "Title2", 0.422, 692, new DateTime(2010, 1, 12), "Genre2",
                                 @"C:\path\file2.ext", 742);
            var item3 = new Item("Artist3", "Title3", 0.423, 693, new DateTime(2010, 1, 13), "Genre3",
                                 @"C:\path\file3.ext", 743);
            var library = new Library("1.0", "Unit Tests", new[] { item1, item2, item3 });

            var expectedItem1 = Mother.CreateItemXElement("Artist1", "Title1", "0.421", "691", "2010-01-11T00:00:00",
                                                          "Genre1", @"C:\path\file1.ext", "741");
            var expectedItem2 = Mother.CreateItemXElement("Artist2", "Title2", "0.422", "692", "2010-01-12T00:00:00",
                                                          "Genre2", @"C:\path\file2.ext", "742");
            var expectedItem3 = Mother.CreateItemXElement("Artist3", "Title3", "0.423", "693", "2010-01-13T00:00:00",
                                                          "Genre3", @"C:\path\file3.ext", "743");
            var expected = new XElement("l",
                                        new XAttribute("v", "1.0"),
                                        new XAttribute("st", "Unit Tests"),
                                        expectedItem1,
                                        expectedItem2,
                                        expectedItem3);

            var actual = new LmlXmlSerializer().ToXElement(library);
            Assert.IsTrue(XNode.DeepEquals(expected, actual));
        }

        [Test]
        public void GivenNullItemShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new LmlXmlSerializer().ToXElement((Item)null));
        }

        [Test]
        public void GivenNullLibraryShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new LmlXmlSerializer().ToXElement((Library)null));
        }
    }
}