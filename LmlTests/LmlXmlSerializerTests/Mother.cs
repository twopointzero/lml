using System.Xml.Linq;

namespace twopointzero.LmlTests.LmlXmlSerializerTests
{
    internal static class Mother
    {
        public static XElement CreateItemXElement(string artist, string album, string title, string rating,
                                                  string dateAdded, string playCount, string lastPlayed, string genre,
                                                  string location, string duration, string bitsPerSecond)
        {
            return new XElement("i",
                                new XAttribute("a", artist),
                                new XAttribute("al", album),
                                new XAttribute("t", title),
                                new XAttribute("r", rating),
                                new XAttribute("da", dateAdded),
                                new XAttribute("pc", playCount),
                                new XAttribute("lp", lastPlayed),
                                new XAttribute("g", genre),
                                new XAttribute("l", location),
                                new XAttribute("ds", duration),
                                new XAttribute("bps", bitsPerSecond));
        }
    }
}