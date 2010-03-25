using System;
using System.Xml.Linq;
using NUnit.Framework;
using twopointzero.Lml.Importers;

namespace twopointzero.LmlTests.Importers.AppleITunesXmlImporterTests
{
    [TestFixture]
    public class GetPlistDictNode
    {
        [Test]
        public void GivenMainDictChildShouldReturnItsNode()
        {
            var plistRootNode = new XElement("plist");
            plistRootNode.Add(new XElement("dict"));
            var actual = AppleITunesXmlImporter.GetPlistDictNode(plistRootNode);
            Assert.IsNotNull(actual);
            Assert.IsNotNull(actual.Name);
            Assert.AreEqual("dict", actual.Name.LocalName);
        }

        [Test]
        public void GivenNoMainDictChildShouldThrowArgumentOutOfRangeException()
        {
            var plistRootNode = new XElement("plist");
            Assert.Throws<ArgumentOutOfRangeException>(() => AppleITunesXmlImporter.GetPlistDictNode(plistRootNode));
        }

        [Test]
        public void GivenNullShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => AppleITunesXmlImporter.GetPlistDictNode(null));
        }
    }
}