using System.IO;
using twopointzero.Lml;
using twopointzero.Lml.Importers;

namespace twopointzero.AppleITunesXmlToLml
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            using (var reader = new StreamReader(args[0]))
            {
                var library = AppleITunesXmlImporter.ImportLibrary(reader, LibraryMode.Guest);
                var serializer = new LmlXmlSerializer();
                var element = serializer.ToXElement(library);
                element.Save(args[1]);
            }
        }
    }
}