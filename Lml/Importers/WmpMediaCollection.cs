using System.Collections.Generic;
using Microsoft.MediaPlayer.Interop;
using twopointzero.Validation;

namespace twopointzero.Lml.Importers
{
    internal class WmpMediaCollection
    {
        public readonly IEnumerable<WmpMedia> Media;
        public readonly string VersionInfo;

        public WmpMediaCollection(string mediaType)
        {
            Validator.IsNotNullOrEmpty(mediaType, "mediaType");

            IWMPPlayer player = new WindowsMediaPlayerClass();
            VersionInfo = player.versionInfo;
            Media = GetItems(player, mediaType);
        }

        internal WmpMediaCollection(string versionInfo, IEnumerable<WmpMedia> media)
        {
            Validator.Create()
                .IsNotNullOrEmpty(versionInfo, "versionInfo")
                .IsNotNull(media, "media")
                .Validate();

            VersionInfo = versionInfo;
            Media = media;
        }

        private static IEnumerable<WmpMedia> GetItems(IWMPPlayer player, string mediaType)
        {
            var collection = player.mediaCollection;
            var list = collection.getAll();
            for (int i = 0; i < list.count; i++)
            {
                var item = list.get_Item(i);
                if (item.getItemInfo("MediaType") == mediaType)
                {
                    yield return GetItem(item);
                }
            }
        }

        private static WmpMedia GetItem(IWMPMedia media)
        {
            var displayArtist = media.getItemInfo("DisplayArtist");
            var title = media.getItemInfo("Title");
            var userRating = media.getItemInfo("UserRating");
            var acquisitionTime = media.getItemInfo("AcquisitionTime");
            var userPlayCount = media.getItemInfo("UserPlayCount");
            var userLastPlayedTime = media.getItemInfo("UserLastPlayedTime");
            var wmGenre = media.getItemInfo("WM/Genre");
            var sourceUrl = media.getItemInfo("SourceURL");
            var duration = media.getItemInfo("Duration");

            return new WmpMedia(displayArtist, title, userRating, acquisitionTime, userPlayCount, userLastPlayedTime,
                                wmGenre, sourceUrl, duration);
        }
    }
}