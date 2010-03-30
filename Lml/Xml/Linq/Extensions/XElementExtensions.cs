using System;
using System.Xml.Linq;

namespace twopointzero.Lml.Xml.Linq.Extensions
{
    internal static class XElementExtensions
    {
        internal static void AddAttributeIfValueNonEmpty(this XElement self, XName name, object value)
        {
            if (value == null || (value is string && ((string)value).Length == 0))
            {
                return;
            }

            self.Add(new XAttribute(name, value));
        }

        internal static DateTime? GetAttributeValueAsNullableDateTime(this XElement self, string attributeName)
        {
            string value = self.GetNonEmptyAttributeValueOrNull(attributeName);
            return value == null ? (DateTime?)null : Convert.ToDateTime(value);
        }

        internal static double? GetAttributeValueAsNullableDouble(this XElement self, string attributeName)
        {
            string value = self.GetNonEmptyAttributeValueOrNull(attributeName);
            return value == null ? (double?)null : Convert.ToDouble(value);
        }

        internal static int? GetAttributeValueAsNullableInt32(this XElement self, string attributeName)
        {
            string value = self.GetNonEmptyAttributeValueOrNull(attributeName);
            return value == null ? (int?)null : Convert.ToInt32(value);
        }

        internal static string GetNonEmptyAttributeValue(this XElement self, string attributeName,
                                                         string attributeClarification)
        {
            string value = self.GetNonEmptyAttributeValueOrNull(attributeName);
            if (value == null)
            {
                throw new ArgumentOutOfRangeException("self",
                                                      "A v (" + attributeClarification + ") attribute is required.");
            }
            return value;
        }

        internal static string GetNonEmptyAttributeValueOrNull(this XElement self, string name)
        {
            XAttribute attribute = self.Attribute(name);
            if (attribute == null || String.IsNullOrEmpty(attribute.Value))
            {
                return null;
            }

            return attribute.Value;
        }
    }
}