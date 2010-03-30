using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using twopointzero.Validation;

namespace twopointzero.Lml.Importers
{
    public static class AppleITunesXmlImporter
    {
        private const string LibraryVersion = "1.0";

        internal static XElement GetPlistRootNode(TextReader readerAtStartOfDocument)
        {
            Validator.IsNotNull(readerAtStartOfDocument, "readerAtStartOfDocument");

            XDocument doc = XDocument.Load(readerAtStartOfDocument);

            XDocumentType documentType = doc.DocumentType;
            var requiredDocumentType = new XDocumentType("plist", "-//Apple Computer//DTD PLIST 1.0//EN",
                                                         "http://www.apple.com/DTDs/PropertyList-1.0.dtd", "");
            if (!XNode.DeepEquals(documentType, requiredDocumentType))
            {
                throw new ArgumentOutOfRangeException("readerAtStartOfDocument",
                                                      "The provided TextReader's document does not contain the required DOCTYPE of the form " +
                                                      @"<!DOCTYPE plist PUBLIC ""-//Apple Computer//DTD PLIST 1.0//EN"" ""http://www.apple.com/DTDs/PropertyList-1.0.dtd"">");
            }

            XElement root = doc.Element("plist");
            if (root == null)
            {
                throw new ArgumentOutOfRangeException("readerAtStartOfDocument",
                                                      "The provided TextReader's document does not contain the required plist root node.");
            }

            XAttribute version = root.Attribute("version");
            if (version == null || version.Value != LibraryVersion)
            {
                throw new ArgumentOutOfRangeException("readerAtStartOfDocument",
                                                      "The provided TextReader's document does not contain the required plist root node version of \"1.0\".");
            }

            return root;
        }

        internal static XElement GetPlistDictNode(XElement plistRootNode)
        {
            Validator.IsNotNull(plistRootNode, "plistRootNode");

            XElement mainDict = plistRootNode.Element("dict");
            if (mainDict == null)
            {
                throw new ArgumentOutOfRangeException("plistRootNode",
                                                      "The provided plist XElement does not contain the required main dict element.");
            }
            return mainDict;
        }

        internal static IDictionary<string, object> GetPrimitiveEntries(XElement dictNode)
        {
            Validator.IsNotNull(dictNode, "dictNode");

            return dictNode.Elements("key").ToDictionary(key => (string)key,
                                                         key => GetPrimitiveValue(key.NextNode));
        }

        internal static object GetPrimitiveValue(XNode node)
        {
            var element = node as XElement;
            if (element == null)
            {
                return (string)(XElement)node;
            }

            string localName = element.Name.LocalName;
            switch (localName)
            {
                case "string":
                    return element.Value;
                case "data":
                    return Convert.FromBase64String(element.Value);
                case "date":
                    return XmlConvert.ToDateTime(element.Value, XmlDateTimeSerializationMode.RoundtripKind);
                case "true":
                    return true;
                case "false":
                    return false;
                case "real":
                    return XmlConvert.ToDouble(element.Value);
                case "integer":
                    return XmlConvert.ToInt64(element.Value);
                default:
                    return node;
            }
        }

        public static Library ImportLibrary(TextReader reader, LibraryMode libraryMode)
        {
            Validator.IsNotNull(reader, "reader");

            XElement plistRootNode = GetPlistRootNode(reader);
            XElement mainDictNode = GetPlistDictNode(plistRootNode);
            IDictionary<string, object> metadata = GetPrimitiveEntries(mainDictNode);

            object version;
            if (!metadata.TryGetValue("Application Version", out version))
            {
                throw new ArgumentOutOfRangeException("reader",
                                                      "The provided document does not include the required Application Version property.");
            }

            object tracks;
            if (!metadata.TryGetValue("Tracks", out tracks))
            {
                throw new ArgumentOutOfRangeException("reader",
                                                      "The provided document does not include the required Tracks key.");
            }

            var tracksElement = tracks as XElement;
            if (tracksElement == null)
            {
                throw new ArgumentOutOfRangeException("reader",
                                                      "The provided document does not include the required Tracks value.");
            }

            if (tracksElement.Name.LocalName != "dict")
            {
                throw new ArgumentOutOfRangeException("reader",
                                                      "The provided document's Tracks value is of the wrong type.");
            }

            return new Library(LibraryVersion, "iTunes " + version, GetItems(tracksElement, libraryMode));
        }

        private static IEnumerable<Item> GetItems(XElement tracks, LibraryMode libraryMode)
        {
            return tracks.Elements("dict")
                .Select(dict => GetItem(dict, libraryMode))
                .Where(item => item != null);
        }

        private static Item GetItem(XElement dict, LibraryMode libraryMode)
        {
            IDictionary<string, object> entries = GetPrimitiveEntries(dict);

            object artist;
            entries.TryGetValue("Artist", out artist);

            object title;
            entries.TryGetValue("Name", out title);

            object rating;
            entries.TryGetValue("Rating", out rating);

            object playCount;
            entries.TryGetValue("Play Count", out playCount);

            object lastPlayed;
            entries.TryGetValue("Play Date UTC", out lastPlayed);

            object genre;
            entries.TryGetValue("Genre", out genre);

            object location = null;
            object duration = null;
            if (libraryMode == LibraryMode.Host)
            {
                entries.TryGetValue("Location", out location);
                entries.TryGetValue("Total Time", out duration);
            }

            return new Item(artist as string, title as string, ImportRating(rating), ImportPlayCount(playCount),
                            lastPlayed as DateTime?, genre as string, location as string, duration as long?);
        }

        private static double? ImportRating(object rating)
        {
            var typedRating = rating as long?;
            if (typedRating == null)
            {
                return null;
            }

            return typedRating / 100.0;
        }

        private static int? ImportPlayCount(object playCount)
        {
            return (int?)(playCount as long?);
        }
    }
}