using System;
using System.Collections.Generic;

namespace twopointzero.Lml
{
    public class Library
    {
        private readonly IEnumerable<Item> _items;
        private readonly string _sourceType;
        private readonly string _version;

        public Library(string version, string sourceType, IEnumerable<Item> items)
        {
            if (version == null)
            {
                throw new ArgumentNullException("version");
            }
            if (version.Length == 0)
            {
                throw new ArgumentOutOfRangeException("version", "Argument must be non-empty.");
            }

            if (sourceType == null)
            {
                throw new ArgumentNullException("sourceType");
            }
            if (sourceType.Length == 0)
            {
                throw new ArgumentOutOfRangeException("sourceType", "Argument must be non-empty.");
            }

            if (items == null)
            {
                throw new ArgumentNullException("items");
            }

            _version = version;
            _sourceType = sourceType;
            _items = items;
        }

        public string Version
        {
            get { return _version; }
        }

        public string SourceType
        {
            get { return _sourceType; }
        }

        public IEnumerable<Item> Items
        {
            get { return _items; }
        }
    }
}