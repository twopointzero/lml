using System;
using System.IO;
using NUnit.Framework;
using twopointzero.Lml.Importers;

namespace twopointzero.LmlTests.Importers.AppleITunesXmlImporterTests
{
    [TestFixture]
    public class GetPlistRootNode
    {
        [Test]
        public void GivenDocWithDtdAndMainPlistRootNodeShouldReturnIt()
        {
            const string input =
                @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!DOCTYPE plist PUBLIC ""-//Apple Computer//DTD PLIST 1.0//EN"" ""http://www.apple.com/DTDs/PropertyList-1.0.dtd"">
<plist version=""1.0"">
</plist>";
            var reader = new StringReader(input);
            var actual = AppleITunesXmlImporter.GetPlistRootNode(reader);
            Assert.IsNotNull(actual, "actual");
            Assert.IsNotNull(actual.Name, "actual.Name");
            Assert.AreEqual("plist", actual.Name.LocalName);
        }

        [Test]
        public void GivenDocWithMainPlistRootNodeShouldReturnIt()
        {
            const string input = @"
<plist version=""1.0"">
</plist>";
            var reader = new StringReader(input);
            var actual = AppleITunesXmlImporter.GetPlistRootNode(reader);
            Assert.IsNotNull(actual, "actual");
            Assert.IsNotNull(actual.Name, "actual.Name");
            Assert.AreEqual("plist", actual.Name.LocalName);
        }

        [Test]
        public void GivenNullShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => AppleITunesXmlImporter.GetPlistRootNode(null));
        }

        [Test]
        public void GivenRootNodeWithWrongVersionShouldThrowArgumentOutOfRangeException()
        {
            const string input =
                @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!DOCTYPE plist PUBLIC ""-//Apple Computer//DTD PLIST 1.0//EN"" ""http://www.apple.com/DTDs/PropertyList-1.0.dtd"">
<plist version=""0.0"">
</plist>";
            var reader = new StringReader(input);
            Assert.Throws<ArgumentOutOfRangeException>(() => AppleITunesXmlImporter.GetPlistRootNode(reader));
        }

        [Test]
        public void GivenRootNodeWithoutVersionShouldThrowArgumentOutOfRangeException()
        {
            const string input =
                @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!DOCTYPE plist PUBLIC ""-//Apple Computer//DTD PLIST 1.0//EN"" ""http://www.apple.com/DTDs/PropertyList-1.0.dtd"">
<plist>
</plist>";
            var reader = new StringReader(input);
            Assert.Throws<ArgumentOutOfRangeException>(() => AppleITunesXmlImporter.GetPlistRootNode(reader));
        }

        [Test]
        public void GivenWrongDtdNameShouldThrowArgumentOutOfRangeException()
        {
            const string input =
                @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!DOCTYPE zlist PUBLIC ""-//Apple Computer//DTD PLIST 1.0//EN"" ""http://www.apple.com/DTDs/PropertyList-1.0.dtd"">
<plist version=""1.0"">
</plist>";
            var reader = new StringReader(input);
            Assert.Throws<ArgumentOutOfRangeException>(() => AppleITunesXmlImporter.GetPlistRootNode(reader));
        }

        [Test]
        public void GivenWrongDtdPublicIdShouldThrowArgumentOutOfRangeException()
        {
            const string input =
                @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!DOCTYPE plist PUBLIC ""-//Snapple Computer//DTD PLIST 1.0//EN"" ""http://www.apple.com/DTDs/PropertyList-1.0.dtd"">
<plist version=""1.0"">
</plist>";
            var reader = new StringReader(input);
            Assert.Throws<ArgumentOutOfRangeException>(() => AppleITunesXmlImporter.GetPlistRootNode(reader));
        }

        [Test]
        public void GivenWrongDtdSystemIdShouldThrowArgumentOutOfRangeException()
        {
            const string input =
                @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!DOCTYPE plist PUBLIC ""-//Apple Computer//DTD PLIST 1.0//EN"" ""http://www.snapple.com/DTDs/PropertyList-1.0.dtd"">
<plist version=""1.0"">
</plist>";
            var reader = new StringReader(input);
            Assert.Throws<ArgumentOutOfRangeException>(() => AppleITunesXmlImporter.GetPlistRootNode(reader));
        }

        [Test]
        public void GivenWrongRootNodeShouldThrowArgumentOutOfRangeException()
        {
            const string input =
                @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!DOCTYPE plist PUBLIC ""-//Apple Computer//DTD PLIST 1.0//EN"" ""http://www.apple.com/DTDs/PropertyList-1.0.dtd"">
<zlist>
</zlist>";
            var reader = new StringReader(input);
            Assert.Throws<ArgumentOutOfRangeException>(() => AppleITunesXmlImporter.GetPlistRootNode(reader));
        }
    }
}