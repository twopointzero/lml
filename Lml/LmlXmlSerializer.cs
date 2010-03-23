using System;
using System.Linq;
using System.Xml.Linq;

namespace twopointzero.Lml
{
    public class LmlXmlSerializer
    {
        public XElement ToXElement(Item item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            var element = new XElement("i");
            AddAttributeIfValueNotNull(element, "a", item.Artist);
            AddAttributeIfValueNotNull(element, "t", item.Title);
            AddAttributeIfValueNotNull(element, "r", item.Rating);
            AddAttributeIfValueNotNull(element, "pc", item.PlayCount);
            AddAttributeIfValueNotNull(element, "lp", item.LastPlayed);
            AddAttributeIfValueNotNull(element, "g", item.Genre);
            AddAttributeIfValueNotNull(element, "l", item.Location);
            return element;
        }

        private static void AddAttributeIfValueNotNull(XElement element, XName name, object value)
        {
            if (value == null)
                return;

            element.Add(new XAttribute(name, value));
        }

        public XElement ToXElement(Library library)
        {
            if (library == null)
            {
                throw new ArgumentNullException("library");
            }

            var attributes = new XObject[]
                                 {
                                     new XAttribute("v", library.Version),
                                     new XAttribute("st", library.SourceType)
                                 };
            var items = library.Items.Select(item => ToXElement(item)).Cast<XObject>();
            var objects = attributes.Concat(items);
            return new XElement("l", objects);
        }

        public Item ToItem(XElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            if (element.Name != "i")
            {
                throw new ArgumentOutOfRangeException("element", element.Name, "Item element (<i>) required.");
            }

            string artist = GetNonEmptyAttributeValueOrNull(element, "a");
            string title = GetNonEmptyAttributeValueOrNull(element, "t");
            string ratingValue = GetNonEmptyAttributeValueOrNull(element, "r");
            var rating = ratingValue == null ? (double?)null : Convert.ToDouble(ratingValue);
            string playCountValue = GetNonEmptyAttributeValueOrNull(element, "pc");
            var playCount = playCountValue == null ? (int?)null : Convert.ToInt32(playCountValue);
            string lastPlayedValue = GetNonEmptyAttributeValueOrNull(element, "lp");
            var lastPlayed = lastPlayedValue == null ? (DateTime?)null : Convert.ToDateTime(lastPlayedValue);
            string genre = GetNonEmptyAttributeValueOrNull(element, "g");
            string location = GetNonEmptyAttributeValueOrNull(element, "l");
            return new Item(artist, title, rating, playCount, lastPlayed, genre, location);
        }

        private static string GetNonEmptyAttributeValueOrNull(XElement element, string name)
        {
            XAttribute attribute = element.Attribute(name);
            if (attribute == null || string.IsNullOrEmpty(attribute.Value))
            {
                return null;
            }

            return attribute.Value;
        }

        public Library ToLibrary(XElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            if (element.Name != "l")
            {
                throw new ArgumentOutOfRangeException("element", element.Name, "Library element (<l>) required.");
            }

            string version = GetNonEmptyAttributeValueOrNull(element, "v");
            if (version == null)
            {
                throw new ArgumentOutOfRangeException("element", "A v (version) attribute is required.");
            }

            string sourceType = GetNonEmptyAttributeValueOrNull(element, "st");
            if (sourceType == null)
            {
                throw new ArgumentOutOfRangeException("element", "An st (source type) attribute is required.");
            }

            var items = element.Elements("i").Select(item => ToItem(item));
            return new Library(version, sourceType, items);
        }
    }
}