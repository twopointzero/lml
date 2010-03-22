using System.Xml.Linq;

namespace twopointzero.LmlTests.LmlXmlSerializerTests
{
    internal static class Mother
    {
        public static XElement CreateItemXElement(string artist, string title, string rating, string playCount,
                                                  string lastPlayed, string genre)
        {
            return new XElement("i",
                                new XAttribute("a", artist),
                                new XAttribute("t", title),
                                new XAttribute("r", rating),
                                new XAttribute("pc", playCount),
                                new XAttribute("lp", lastPlayed),
                                new XAttribute("g", genre));
        }

        public static XElement CreateItemXElement(string artist, string title, string rating, string playCount,
                                                  string lastPlayed, string genre, string location)
        {
            return new XElement("i",
                                new XAttribute("a", artist),
                                new XAttribute("t", title),
                                new XAttribute("r", rating),
                                new XAttribute("pc", playCount),
                                new XAttribute("lp", lastPlayed),
                                new XAttribute("g", genre),
                                new XAttribute("l", location));
        }
    }
}