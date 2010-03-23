using System;
using System.Linq;
using System.Xml.Linq;
using NUnit.Framework;
using twopointzero.Lml;

namespace twopointzero.LmlTests.LmlXmlSerializerTests
{
    [TestFixture]
    public class ToLibrary
    {
        [Test]
        public void GivenEmptyLibraryShouldProduceExpectedResult()
        {
            var library = new XElement("l",
                                       new XAttribute("v", "1.0"),
                                       new XAttribute("st", "Unit Tests"));

            var expected = new Library("1.0", "Unit Tests", Enumerable.Empty<Item>());

            var actual = new LmlXmlSerializer().ToLibrary(library);

            Assert.AreEqual(expected.Version, actual.Version);
            Assert.AreEqual(expected.SourceType, actual.SourceType);
            CollectionAssert.AreEqual(expected.Items, actual.Items);
        }

        [Test]
        public void GivenGuestLibraryShouldProduceExpectedResult()
        {
            var item1 = Mother.CreateItemXElement("Artist1", "Title1", "0.421", "691", "2010-01-11T00:00:00",
                                                  "Genre1");
            var item2 = Mother.CreateItemXElement("Artist2", "Title2", "0.422", "692", "2010-01-12T00:00:00",
                                                  "Genre2");
            var item3 = Mother.CreateItemXElement("Artist3", "Title3", "0.423", "693", "2010-01-13T00:00:00",
                                                  "Genre3");
            var library = new XElement("l",
                                       new XAttribute("v", "1.0"),
                                       new XAttribute("st", "Unit Tests"),
                                       item1,
                                       item2,
                                       item3);

            var expectedItem1 = new Item("Artist1", "Title1", 0.421, 691, new DateTime(2010, 1, 11), "Genre1", null);
            var expectedItem2 = new Item("Artist2", "Title2", 0.422, 692, new DateTime(2010, 1, 12), "Genre2", null);
            var expectedItem3 = new Item("Artist3", "Title3", 0.423, 693, new DateTime(2010, 1, 13), "Genre3", null);
            var expected = new Library("1.0", "Unit Tests", new[] { expectedItem1, expectedItem2, expectedItem3 });

            var actual = new LmlXmlSerializer().ToLibrary(library);

            Assert.AreEqual(expected.Version, actual.Version);
            Assert.AreEqual(expected.SourceType, actual.SourceType);
            CollectionAssert.AreEqual(expected.Items, actual.Items);
        }

        [Test]
        public void GivenHostLibraryShouldProduceExpectedResult()
        {
            var item1 = Mother.CreateItemXElement("Artist1", "Title1", "0.421", "691", "2010-01-11T00:00:00",
                                                  "Genre1", @"C:\path\file1.ext");
            var item2 = Mother.CreateItemXElement("Artist2", "Title2", "0.422", "692", "2010-01-12T00:00:00",
                                                  "Genre2", @"C:\path\file2.ext");
            var item3 = Mother.CreateItemXElement("Artist3", "Title3", "0.423", "693", "2010-01-13T00:00:00",
                                                  "Genre3", @"C:\path\file3.ext");
            var library = new XElement("l",
                                       new XAttribute("v", "1.0"),
                                       new XAttribute("st", "Unit Tests"),
                                       item1,
                                       item2,
                                       item3);

            var expectedItem1 = new Item("Artist1", "Title1", 0.421, 691, new DateTime(2010, 1, 11), "Genre1",
                                         @"C:\path\file1.ext");
            var expectedItem2 = new Item("Artist2", "Title2", 0.422, 692, new DateTime(2010, 1, 12), "Genre2",
                                         @"C:\path\file2.ext");
            var expectedItem3 = new Item("Artist3", "Title3", 0.423, 693, new DateTime(2010, 1, 13), "Genre3",
                                         @"C:\path\file3.ext");
            var expected = new Library("1.0", "Unit Tests", new[] { expectedItem1, expectedItem2, expectedItem3 });

            var actual = new LmlXmlSerializer().ToLibrary(library);

            Assert.AreEqual(expected.Version, actual.Version);
            Assert.AreEqual(expected.SourceType, actual.SourceType);
            CollectionAssert.AreEqual(expected.Items, actual.Items);
        }

        [Test]
        public void GivenLibraryWithEmptySourceTypeShouldThrowArgumentOutOfRangeException()
        {
            var library = new XElement("l",
                                       new XAttribute("v", "1.0"),
                                       new XAttribute("st", string.Empty));

            Assert.Throws<ArgumentOutOfRangeException>(() => new LmlXmlSerializer().ToLibrary(library));
        }

        [Test]
        public void GivenLibraryWithEmptyVersionShouldThrowArgumentOutOfRangeException()
        {
            var library = new XElement("l",
                                       new XAttribute("v", string.Empty),
                                       new XAttribute("st", "Unit Tests"));

            Assert.Throws<ArgumentOutOfRangeException>(() => new LmlXmlSerializer().ToLibrary(library));
        }

        [Test]
        public void GivenLibraryWithoutSourceTypeShouldThrowArgumentOutOfRangeException()
        {
            var library = new XElement("l",
                                       new XAttribute("v", "1.0"));

            Assert.Throws<ArgumentOutOfRangeException>(() => new LmlXmlSerializer().ToLibrary(library));
        }

        [Test]
        public void GivenLibraryWithoutVersionShouldThrowArgumentOutOfRangeException()
        {
            var library = new XElement("l",
                                       new XAttribute("st", "Unit Tests"));

            Assert.Throws<ArgumentOutOfRangeException>(() => new LmlXmlSerializer().ToLibrary(library));
        }

        [Test]
        public void GivenNonLibraryShouldThrowArgumentNullException()
        {
            var library = new XElement("z");
            Assert.Throws<ArgumentOutOfRangeException>(() => new LmlXmlSerializer().ToLibrary(library));
        }

        [Test]
        public void GivenNullLibraryShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new LmlXmlSerializer().ToLibrary(null));
        }
    }
}