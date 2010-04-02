﻿using System;
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
            var item = Mother.CreateGuestItemXElement("Artist", "Title", "0.42", "2009-01-01T00:00:00", "69",
                                                      "2010-01-01T00:00:00", "Genre");
            var expected = new Item("Artist", "Title", 0.42, new DateTime(2009, 1, 1), 69, new DateTime(2010, 1, 1),
                                    "Genre", null, null);
            var actual = LmlXmlSerializer.ToItem(item);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GivenHostItemShouldProduceExpectedResult()
        {
            var item = Mother.CreateHostItemXElement("Artist", "Title", "0.42", "2009-01-01T00:00:00", "69",
                                                     "2010-01-01T00:00:00", "Genre", @"C:\path\file.ext", "74");
            var expected = new Item("Artist", "Title", 0.42, new DateTime(2009, 1, 1), 69, new DateTime(2010, 1, 1),
                                    "Genre", @"C:\path\file.ext",
                                    74);
            var actual = LmlXmlSerializer.ToItem(item);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GivenItemWithEmptyAttributesShouldProduceExpectedResult()
        {
            var item = Mother.CreateHostItemXElement("", "", "", "", "", "", "", "", "");
            var expected = new Item(null, null, null, null, null, null, null, null, null);
            var actual = LmlXmlSerializer.ToItem(item);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GivenItemWithoutAttributesShouldProduceExpectedResult()
        {
            var item = new XElement("i");
            var expected = new Item(null, null, null, null, null, null, null, null, null);
            var actual = LmlXmlSerializer.ToItem(item);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GivenNonItemShouldThrowArgumentNullException()
        {
            var item = new XElement("z");
            Assert.Throws<ArgumentOutOfRangeException>(() => LmlXmlSerializer.ToItem(item));
        }

        [Test]
        public void GivenNullItemShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => LmlXmlSerializer.ToItem(null));
        }
    }
}