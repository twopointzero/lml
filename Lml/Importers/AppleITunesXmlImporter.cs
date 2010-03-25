using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace twopointzero.Lml.Importers
{
    public static class AppleITunesXmlImporter
    {
        internal static XElement GetPlistRootNode(TextReader readerAtStartOfDocument)
        {
            if (readerAtStartOfDocument == null)
            {
                throw new ArgumentNullException("readerAtStartOfDocument");
            }

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
            if (plistRootNode == null)
            {
                throw new ArgumentNullException("plistRootNode");
            }

            var mainDict = plistRootNode.Element("dict");
            if (mainDict == null)
            {
                throw new ArgumentOutOfRangeException("plistRootNode",
                                                      "The provided plist XElement does not contain the required main dict element.");
            }
            return mainDict;
        }

        public static IDictionary<string, object> GetLibraryMetadata(TextReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }

            XElement plistRootNode = GetPlistRootNode(reader);
            XElement mainDictNode = GetPlistDictNode(plistRootNode);
            return GetPrimitiveEntries(mainDictNode);
        }

        internal static IDictionary<string, object> GetPrimitiveEntries(XElement dictNode)
        {
            if (dictNode == null)
            {
                throw new ArgumentNullException("dictNode");
            }

            return dictNode.Elements("key").ToDictionary(key => ((string)key).Replace(" ", ""),
                                                         key => GetPrimitiveValue(key.NextNode));
        }

        internal static object GetPrimitiveValue(XNode node)
        {
            var element = node as XElement;
            if (element == null)
            {
                return (string)(XElement)node;
            }

            var localName = element.Name.LocalName;
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
                    return XmlConvert.ToInt32(element.Value);
                default:
                    throw new ArgumentOutOfRangeException("node", node.ToString(),
                                                          "The provided node is of an unsupported kind.");
            }
        }
    }
}