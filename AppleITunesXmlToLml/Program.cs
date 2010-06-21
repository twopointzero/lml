using System.IO;
using twopointzero.Lml;
using twopointzero.Lml.Importers;

namespace twopointzero.AppleITunesXmlToLml
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string source = args[0];
            string destination = args[1];

            Convert(source, destination);
        }

        private static void Convert(string source, string destination)
        {
            using (var reader = new StreamReader(source))
            {
                var library = AppleITunesXmlImporter.ImportLibrary(reader);
                var element = LmlXmlSerializer.ToXElement(library);
                element.Save(destination);
            }
        }
    }
}