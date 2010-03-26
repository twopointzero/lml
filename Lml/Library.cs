using System.Collections.Generic;
using twopointzero.Lml.Validation;

namespace twopointzero.Lml
{
    public class Library
    {
        private readonly IEnumerable<Item> _items;
        private readonly string _sourceType;
        private readonly string _version;

        public Library(string version, string sourceType, IEnumerable<Item> items)
        {
            Validator.Create()
                .IsNotNullOrEmpty(version, "version")
                .IsNotNullOrEmpty(sourceType, "sourceType")
                .IsNotNull(items, "items")
                .Validate();

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