using System;
using System.Linq;
using System.Xml.Linq;
using twopointzero.Lml.Validation;

namespace twopointzero.Lml
{
    public class LmlXmlSerializer
    {
        public XElement ToXElement(Item item)
        {
            Validator.IsNotNull(item, "item");

            var element = new XElement("i");
            AddAttributeIfValueNonEmpty(element, "a", item.Artist);
            AddAttributeIfValueNonEmpty(element, "t", item.Title);
            AddAttributeIfValueNonEmpty(element, "r", item.Rating);
            AddAttributeIfValueNonEmpty(element, "pc", item.PlayCount);
            AddAttributeIfValueNonEmpty(element, "lp", item.LastPlayed);
            AddAttributeIfValueNonEmpty(element, "g", item.Genre);
            AddAttributeIfValueNonEmpty(element, "l", item.Location);
            AddAttributeIfValueNonEmpty(element, "d", item.Duration);
            return element;
        }

        private static void AddAttributeIfValueNonEmpty(XElement element, XName name, object value)
        {
            if (value == null || (value is string && ((string)value).Length == 0))
                return;

            element.Add(new XAttribute(name, value));
        }

        public XElement ToXElement(Library library)
        {
            Validator.IsNotNull(library, "library");

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
            Validator.Create()
                .IsNotNull(element, "element")
                .Validate()
                .IsEqualTo(element.Name, "element", "i", "Item element (<i>) required.")
                .Validate();

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
            string durationValue = GetNonEmptyAttributeValueOrNull(element, "d");
            var duration = durationValue == null ? (int?)null : Convert.ToInt32(durationValue);
            return new Item(artist, title, rating, playCount, lastPlayed, genre, location, duration);
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
            Validator.Create()
                .IsNotNull(element, "element")
                .Validate()
                .IsEqualTo(element.Name, "element", "l", "Library element (<l>) required.")
                .Validate();

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