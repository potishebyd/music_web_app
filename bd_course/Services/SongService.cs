using System;
using System.Collections.Generic;
using System.Linq;
using bd_course.Models;
using bd_course.Interface;
using System.Numerics;
using bd_course.Repository;
using NodaTime;

namespace bd_course.Services
{
    public interface ISongService
    {
        void Add(Song song);
        void Delete(Song song);
        void Update(Song song);

        IEnumerable<Song> GetAll();
        Song GetByID(int id);
        Song GetSongByTitle(string title);
        IEnumerable<Song> GetByAlbumTitle(string albumTitle);
        IEnumerable<Song> GetByGenre(string genre);
        IEnumerable<Song> GetSortSongsByTitle();
        IEnumerable<Song> GetByParameters(string title = null, string albumTitle = null, string genre = null, string artistName = null, string recordingStudioName = null, int playlistId = 0);
        IEnumerable<Song> GetByArtistName(string artistName);
        IEnumerable<Song> GetSortSongsByOrder(IEnumerable<Song> songs, SongSortState sortOrder);
    }

    public class SongService : ISongService
    {
        private readonly ISongRepository _songRepository;
        private readonly IArtistRepository _artistRepository;
        private readonly IRecordingStudioRepository _recordingStudioRepository;
        private readonly IPlaylistRepository _playlistRepository;

        public SongService(ISongRepository songRepository, IArtistRepository artistRepository, IRecordingStudioRepository recordingStudioRepository, IPlaylistRepository playlistRepository)
        {
            _songRepository = songRepository;
            _artistRepository = artistRepository;
            _recordingStudioRepository = recordingStudioRepository;
            _playlistRepository = playlistRepository;
        }

        private bool IsExist(Song song)
        {
            return _songRepository.GetAll().FirstOrDefault(elem => 
                   elem.Title == song.Title &&
                   elem.AlbumTitle == song.AlbumTitle &&
                   elem.Genre == song.Genre &&
                   elem.ArtistId == song.ArtistId &&
                   elem.RecordingStudioId == song.RecordingStudioId) != null;
        }

        private bool IsNotExist(int id)
        {
            return _songRepository.GetByID(id) == null;
        }

        public void Add(Song song)
        {
            if (IsExist(song))
                throw new Exception("Такая песня уже существует");

            _songRepository.Add(song);
        }

        public void Delete(Song song)
        {
            if (IsNotExist(song.Id))
                throw new Exception("Такой песни не существует");

            _songRepository.Delete(song.Id);
        }

        public IEnumerable<Song> GetAll()
        {
            return _songRepository.GetAll();
        }

        public Song GetByID(int id)
        {
            return _songRepository.GetByID(id);
        }

        public IEnumerable<Song> GetByAlbumTitle(string albumTitle)
        {
            return _songRepository.GetByAlbumTitle(albumTitle);
        }

        public IEnumerable<Song> GetByGenre(string genre)
        {
            return _songRepository.GetByGenre(genre);
        }

        public IEnumerable<Song> GetSortSongsByTitle()
        {
            return _songRepository.GetAll().OrderBy(song => song.Title);
        }
    
        //передалать
        public IEnumerable<Song> GetByParameters(string title = null, string albumTitle = null, string genre = null, string artistName = null, string recordingStudioName = null, int playlistId = 0)
        {
            IEnumerable<Song> songs;
            if (playlistId != 0)
                songs = _playlistRepository.GetSongsByPlaylistId(playlistId);
            else
                songs = _songRepository.GetAll();

            //if (!string.IsNullOrEmpty(artistName))
            //{
             //   songs = songs.Where(song => song.ArtistName == title);
            //}
            // добавть проверку songs.Count() != 0
            if (!string.IsNullOrEmpty(title))
            {
                songs = songs.Where(song => song.Title == title);
            }

            if (!string.IsNullOrEmpty(albumTitle))
            {
                songs = songs.Where(song => song.AlbumTitle == albumTitle);
            }

            if (!string.IsNullOrEmpty(genre))
            {
                songs = songs.Where(song => song.Genre == genre);
            }

            if (!string.IsNullOrEmpty(artistName))
            {
                Artist artist = _artistRepository.GetByName(artistName);
                if (artist == null)
                {
                    songs = Enumerable.Empty<Song>();
                }
                else
                    songs = songs.Where(song => song.ArtistId == artist.Id);
            }

            if (!string.IsNullOrEmpty(recordingStudioName))
            {
                RecordingStudio recordingStudio = _recordingStudioRepository.GetByName(recordingStudioName);
                if (recordingStudioName == null)
                {
                    songs = Enumerable.Empty<Song>();
                }
                else
                    songs = songs.Where(song => song.RecordingStudioId == recordingStudio.Id);
            }

            return songs;
        }

        public Song GetSongByTitle(string title)
        {
            return _songRepository.GetAll().FirstOrDefault(song => song.Title == title);
        }

        public void Update(Song song)
        {
            if (IsNotExist(song.Id))
                throw new Exception("Такой песни не существует");

            _songRepository.Update(song);
        }

        public IEnumerable<Song> GetByArtistName(string artistName)
        {
            Artist artist = _artistRepository.GetByName(artistName);

            if (artist == null)
                return null;

            return _songRepository.GetAll().Where(song => song.ArtistId == artist.Id);
        }

        public IEnumerable<Song> GetSortSongsByOrder(IEnumerable<Song> songs, SongSortState sortOrder)
        {
            IEnumerable<Song> needSongs = sortOrder switch
            {
                SongSortState.IdDesc => songs.OrderByDescending(elem => elem.Id),

                SongSortState.TitleAsc => songs.OrderBy(elem => elem.Title),
                SongSortState.TitleDesc => songs.OrderByDescending(elem => elem.Title),

                SongSortState.AlbumAsc => songs.OrderBy(elem => elem.AlbumTitle),
                SongSortState.AlbumDesc => songs.OrderByDescending(elem => elem.AlbumTitle),

                SongSortState.GenreAsc => songs.OrderBy(elem => elem.Genre),
                SongSortState.GenreDesc => songs.OrderByDescending(elem => elem.Genre),

                SongSortState.ArtistNameAsc => songs.OrderBy(elem => _artistRepository.GetByID(elem.ArtistId).Name),
                SongSortState.ArtistNameDesc => songs.OrderByDescending(elem => _artistRepository.GetByID(elem.ArtistId).Name),

                SongSortState.RecordingStudioNameAsc => songs.OrderBy(elem => _recordingStudioRepository.GetByID(elem.RecordingStudioId).Name),
                SongSortState.RecordingStudioNameDesc => songs.OrderByDescending(elem => _recordingStudioRepository.GetByID(elem.RecordingStudioId).Name),

                SongSortState.DurationAsc => songs.OrderBy(elem => elem.Duration),
                SongSortState.DurationDesc => songs.OrderByDescending(elem => elem.Duration),

                _ => songs.OrderBy(elem => elem.Id)
            };

            return needSongs;
        }
    }
}
