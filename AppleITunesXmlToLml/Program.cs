using System;
using System.IO;
using twopointzero.Lml;
using twopointzero.Lml.Importers;

namespace twopointzero.AppleITunesXmlToLml
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            LibraryMode libraryMode = GetLibraryMode(args[0]);
            string source = args[1];
            string destination = args[2];

            Convert(libraryMode, source, destination);
        }

        private static LibraryMode GetLibraryMode(string libraryMode)
        {
            switch (libraryMode.Substring(0, 1).ToUpperInvariant())
            {
                case "G":
                    return LibraryMode.Guest;
                case "H":
                    return LibraryMode.Host;
                default:
                    throw new ArgumentOutOfRangeException("libraryMode", libraryMode,
                                                          "Expected g or guest (any casing) for guest mode output or h or host (any casing) for host mode output.");
            }
        }

        private static void Convert(LibraryMode libraryMode, string source, string destination)
        {
            using (var reader = new StreamReader(source))
            {
                var library = AppleITunesXmlImporter.ImportLibrary(reader, libraryMode);
                var element = LmlXmlSerializer.ToXElement(library);
                element.Save(destination);
            }
        }
    }
}