using System;

namespace twopointzero.Lml
{
    public class Item
    {
        private readonly string _artist;
        private readonly string _genre;
        private readonly DateTime _lastPlayed;
        private readonly string _location;
        private readonly int _playCount;
        private readonly double _rating;
        private readonly string _title;

        public Item(string artist, string title, double rating, int playCount, DateTime lastPlayed, string genre,
                    string location)
        {
            _artist = artist;
            _title = title;
            _rating = rating;
            _playCount = playCount;
            _lastPlayed = lastPlayed;
            _genre = genre;
            _location = location;
        }

        public string Artist
        {
            get { return _artist; }
        }

        public string Title
        {
            get { return _title; }
        }

        public double Rating
        {
            get { return _rating; }
        }

        public int PlayCount
        {
            get { return _playCount; }
        }

        public DateTime LastPlayed
        {
            get { return _lastPlayed; }
        }

        public string Genre
        {
            get { return _genre; }
        }

        public string Location
        {
            get { return _location; }
        }
    }
}