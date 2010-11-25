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
        internal static XElement GetPlistRootNode(TextReader readerAtStartOfDocument)
        {
            Validator.IsNotNull(readerAtStartOfDocument, "readerAtStartOfDocument");

            var doc = XDocument.Load(readerAtStartOfDocument);

            var documentType = doc.DocumentType;
            var requiredDocumentType = new XDocumentType("plist", "-//Apple Computer//DTD PLIST 1.0//EN",
                                                         "http://www.apple.com/DTDs/PropertyList-1.0.dtd", "");
            if (!XNode.DeepEquals(documentType, requiredDocumentType))
            {
                throw new ArgumentOutOfRangeException("readerAtStartOfDocument",
                                                      "The provided TextReader's document does not contain the required DOCTYPE of the form " +
                                                      @"<!DOCTYPE plist PUBLIC ""-//Apple Computer//DTD PLIST 1.0//EN"" ""http://www.apple.com/DTDs/PropertyList-1.0.dtd"">");
            }

            var root = doc.Element("plist");
            if (root == null)
            {
                throw new ArgumentOutOfRangeException("readerAtStartOfDocument",
                                                      "The provided TextReader's document does not contain the required plist root node.");
            }

            var version = root.Attribute("version");
            if (version == null || version.Value != "1.0")
            {
                throw new ArgumentOutOfRangeException("readerAtStartOfDocument",
                                                      "The provided TextReader's document does not contain the required plist root node version of \"1.0\".");
            }

            return root;
        }

        internal static XElement GetPlistDictNode(XElement plistRootNode)
        {
            Validator.IsNotNull(plistRootNode, "plistRootNode");

            var mainDict = plistRootNode.Element("dict");
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

        public static Library ImportLibrary(TextReader reader)
        {
            Validator.IsNotNull(reader, "reader");

            var plistRootNode = GetPlistRootNode(reader);
            var mainDictNode = GetPlistDictNode(plistRootNode);
            var metadata = GetPrimitiveEntries(mainDictNode);

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

            return new Library("1.1", "iTunes " + version, GetItems(tracksElement));
        }

        private static IEnumerable<Item> GetItems(XElement tracks)
        {
            return tracks.Elements("dict")
                .Select(dict => GetItem(dict))
                .Where(item => item != null);
        }

        private static Item GetItem(XElement dict)
        {
            var entries = GetPrimitiveEntries(dict);

            if (ShouldSkip(entries))
                return null;

            object artist;
            entries.TryGetValue("Artist", out artist);

            object album;
            entries.TryGetValue("Album", out album);

            object name;
            entries.TryGetValue("Name", out name);
            string title = PrepareTitle(name, artist);

            object rating;
            entries.TryGetValue("Rating", out rating);

            object dateAdded;
            entries.TryGetValue("Date Added", out dateAdded);

            object playCount;
            entries.TryGetValue("Play Count", out playCount);

            object lastPlayed;
            entries.TryGetValue("Play Date UTC", out lastPlayed);

            object genre;
            entries.TryGetValue("Genre", out genre);

            object location;
            entries.TryGetValue("Location", out location);

            object durationInMilliseconds;
            entries.TryGetValue("Total Time", out durationInMilliseconds);
            TimeSpan? duration = durationInMilliseconds == null
                                     ? (TimeSpan?)null
                                     : TimeSpan.FromMilliseconds((long)durationInMilliseconds);

            object bitRate;
            entries.TryGetValue("Bit Rate", out bitRate);

            return new Item(artist as string, album as string, title, ImportRating(rating), dateAdded as DateTime?,
                            ImportPlayCount(playCount), lastPlayed as DateTime?, genre as string, location as string,
                            duration, ImportBitRate(bitRate));
        }

        private static string PrepareTitle(object name, object artist)
        {
            if (name == null)
                return null;
            
            if (artist == null && !(name is string))
                return null;

            var nameString = (string)name;

            if (artist == null)
                return nameString;

            //  remove artist from the beginning or end of the name value, if necessary

            string artistSuffix = " - " + artist;
            if (nameString.EndsWith(artistSuffix))
                return nameString.Substring(0, nameString.Length - artistSuffix.Length);

            string artistPrefix = artist + " - ";
            if (nameString.StartsWith(artistPrefix))
                return nameString.Substring(artistPrefix.Length);

            return nameString;
        }

        private static bool ShouldSkip(IDictionary<string, object> entries)
        {
            return IsPresentAndTrue(entries, "Movie") || IsPresentAndTrue(entries, "Podcast");
        }

        private static bool IsPresentAndTrue(IDictionary<string, object> entries, string key)
        {
            object movie;
            return entries.TryGetValue(key, out movie) && (bool)movie;
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

        private static int? ImportBitRate(object bitRate)
        {
            return bitRate == null ? (int?)null : (int)(1000 * (long)bitRate);
        }
    }
}