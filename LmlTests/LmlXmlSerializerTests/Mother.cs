using System.Xml.Linq;

namespace twopointzero.LmlTests.LmlXmlSerializerTests
{
    internal static class Mother
    {
        public static XElement CreateGuestItemXElement(string artist, string title, string rating, string dateAdded,
                                                       string playCount, string lastPlayed, string genre)
        {
            return new XElement("i",
                                new XAttribute("a", artist),
                                new XAttribute("t", title),
                                new XAttribute("r", rating),
                                new XAttribute("da", dateAdded),
                                new XAttribute("pc", playCount),
                                new XAttribute("lp", lastPlayed),
                                new XAttribute("g", genre));
        }

        public static XElement CreateHostItemXElement(string artist, string title, string rating, string dateAdded,
                                                      string playCount, string lastPlayed, string genre, string location,
                                                      string duration)
        {
            return new XElement("i",
                                new XAttribute("a", artist),
                                new XAttribute("t", title),
                                new XAttribute("r", rating),
                                new XAttribute("da", dateAdded),
                                new XAttribute("pc", playCount),
                                new XAttribute("lp", lastPlayed),
                                new XAttribute("g", genre),
                                new XAttribute("l", location),
                                new XAttribute("d", duration));
        }
    }
}