using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using twopointzero.Lml.Xml.Linq.Extensions;
using twopointzero.Validation;

namespace twopointzero.Lml
{
    public class LmlXmlSerializer
    {
        public static XElement ToXElement(Item item)
        {
            Validator.IsNotNull(item, "item");

            var element = new XElement("i");
            element.AddAttributeIfValueNonEmpty("a", item.Artist);
            element.AddAttributeIfValueNonEmpty("t", item.Title);
            element.AddAttributeIfValueNonEmpty("r", item.Rating);
            element.AddAttributeIfValueNonEmpty("pc", item.PlayCount);
            element.AddAttributeIfValueNonEmpty("lp", item.LastPlayed);
            element.AddAttributeIfValueNonEmpty("g", item.Genre);
            element.AddAttributeIfValueNonEmpty("l", item.Location);
            element.AddAttributeIfValueNonEmpty("d", item.Duration);
            return element;
        }

        public static XElement ToXElement(Library library)
        {
            Validator.IsNotNull(library, "library");

            var attributes = new XObject[]
                                 {
                                     new XAttribute("v", library.Version),
                                     new XAttribute("st", library.SourceType)
                                 };
            IEnumerable<XObject> items = library.Items.Select(item => ToXElement(item)).Cast<XObject>();
            IEnumerable<XObject> objects = attributes.Concat(items);
            return new XElement("l", objects);
        }

        public static Item ToItem(XElement element)
        {
            Validator.Create()
                .IsNotNull(element, "element")
                .Validate()
                .IsEqualTo(element.Name, "element", "i", "Item element (<i>) required.")
                .Validate();

            string artist = element.GetNonEmptyAttributeValueOrNull("a");
            string title = element.GetNonEmptyAttributeValueOrNull("t");
            double? rating = element.GetAttributeValueAsNullableDouble("r");
            int? playCount = element.GetAttributeValueAsNullableInt32("pc");
            DateTime? lastPlayed = element.GetAttributeValueAsNullableDateTime("lp");
            string genre = element.GetNonEmptyAttributeValueOrNull("g");
            string location = element.GetNonEmptyAttributeValueOrNull("l");
            int? duration = element.GetAttributeValueAsNullableInt32("d");
            return new Item(artist, title, rating, playCount, lastPlayed, genre, location, duration);
        }

        public static Library ToLibrary(XElement element)
        {
            Validator.Create()
                .IsNotNull(element, "element")
                .Validate()
                .IsEqualTo(element.Name, "element", "l", "Library element (<l>) required.")
                .Validate();

            string version = element.GetNonEmptyAttributeValue("v", "version");
            string sourceType = element.GetNonEmptyAttributeValue("st", "source type");

            IEnumerable<Item> items = element.Elements("i").Select(item => ToItem(item));
            return new Library(version, sourceType, items);
        }
    }
}