namespace twopointzero.Lml.Importers
{
    internal class WmpMedia
    {
        public readonly string AcquisitionTime;
        public readonly string DisplayArtist;
        public readonly string Duration;
        public readonly string SourceUrl;
        public readonly string Title;
        public readonly string UserLastPlayedTime;
        public readonly string UserPlayCount;
        public readonly string UserRating;
        public readonly string WmGenre;

        public WmpMedia(string displayArtist, string title, string userRating, string acquisitionTime,
                        string userPlayCount, string userLastPlayedTime, string wmGenre, string sourceUrl,
                        string duration)
        {
            DisplayArtist = displayArtist;
            Duration = duration;
            SourceUrl = sourceUrl;
            WmGenre = wmGenre;
            UserLastPlayedTime = userLastPlayedTime;
            UserPlayCount = userPlayCount;
            AcquisitionTime = acquisitionTime;
            UserRating = userRating;
            Title = title;
        }
    }
}