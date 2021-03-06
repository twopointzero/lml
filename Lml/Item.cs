﻿using System;

namespace twopointzero.Lml
{
    public class Item : IEquatable<Item>
    {
        private readonly string _artist;
        private readonly DateTime? _dateAdded;
        private readonly TimeSpan? _duration;
        private readonly string _genre;
        private readonly DateTime? _lastPlayed;
        private readonly string _location;
        private readonly int? _playCount;
        private readonly double? _rating;
        private readonly string _title;
        private readonly string _album;
        private readonly int? _bitsPerSecond;

        public Item(string artist, string album, string title, double? rating, DateTime? dateAdded, int? playCount,
                    DateTime? lastPlayed, string genre, string location, TimeSpan? duration, int? bitsPerSecond)
        {
            _artist = artist;
            _album = album;
            _title = title;
            _rating = rating;
            _dateAdded = dateAdded;
            _playCount = playCount;
            _lastPlayed = lastPlayed;
            _genre = genre;
            _location = location;
            _duration = duration;
            _bitsPerSecond = bitsPerSecond;
        }

        public string Artist
        {
            get { return _artist; }
        }

        public string Album
        {
            get { return _album; }
        }

        public string Title
        {
            get { return _title; }
        }

        public double? Rating
        {
            get { return _rating; }
        }

        public DateTime? DateAdded
        {
            get { return _dateAdded; }
        }

        public int? PlayCount
        {
            get { return _playCount; }
        }

        public DateTime? LastPlayed
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

        public TimeSpan? Duration
        {
            get { return _duration; }
        }

        public int? BitsPerSecond
        {
            get { return _bitsPerSecond; }
        }

        #region IEquatable<Item> Members

        public bool Equals(Item other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return Equals(other._artist, _artist) && Equals(other._album, _album) && other._dateAdded.Equals(_dateAdded) &&
                   other._duration.Equals(_duration) && Equals(other._genre, _genre) &&
                   other._lastPlayed.Equals(_lastPlayed) && Equals(other._location, _location) &&
                   other._playCount.Equals(_playCount) && other._rating.Equals(_rating) && Equals(other._title, _title) &&
                   other._bitsPerSecond.Equals(_bitsPerSecond);
        }

        #endregion


        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != typeof (Item))
            {
                return false;
            }
            return Equals((Item)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = (_artist != null ? _artist.GetHashCode() : 0);
                result = (result * 397) ^ (_album != null ? _album.GetHashCode() : 0);
                result = (result * 397) ^ (_dateAdded.HasValue ? _dateAdded.Value.GetHashCode() : 0);
                result = (result * 397) ^ (_duration.HasValue ? _duration.Value.GetHashCode() : 0);
                result = (result * 397) ^ (_genre != null ? _genre.GetHashCode() : 0);
                result = (result * 397) ^ (_lastPlayed.HasValue ? _lastPlayed.Value.GetHashCode() : 0);
                result = (result * 397) ^ (_location != null ? _location.GetHashCode() : 0);
                result = (result * 397) ^ (_playCount.HasValue ? _playCount.Value : 0);
                result = (result * 397) ^ (_rating.HasValue ? _rating.Value.GetHashCode() : 0);
                result = (result * 397) ^ (_title != null ? _title.GetHashCode() : 0);
                result = (result * 397) ^ (_bitsPerSecond.HasValue ? _bitsPerSecond.Value : 0);
                return result;
            }
        }

        public static bool operator ==(Item left, Item right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Item left, Item right)
        {
            return !Equals(left, right);
        }
    }
}