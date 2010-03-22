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