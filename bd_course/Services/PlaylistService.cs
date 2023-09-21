using System;
using System.Collections.Generic;
using System.Linq;
using bd_course.Models;
using bd_course.Interface;
using System.Numerics;
using bd_course.Repository;
using bd_course.ViewModels;
using NodaTime;

namespace bd_course.Services
{
    public interface IPlaylistService
    {
        void Add(Playlist playlist);
        void Update(Playlist playlist);
        void Delete(int id);

        IEnumerable<Playlist> GetAll();
        Playlist GetByID(int id);
        Playlist GetByName(string name);

        void AddSongToPlaylist(int songId, int playlistId);
        void RemoveSongFromPlaylist(int songId, int playlistId);
        IEnumerable<SongPlaylist> GetAllSongPlaylists();
        IEnumerable<SongPlaylist> GetPlaylistsBySongId(int songId);
        SongPlaylist GetSongPlaylist(int songId, int playlistId);
        IEnumerable<Song> GetSongsByPlaylistId(int playlistId);
        IEnumerable<Song> GetMySongsByUserLogin(string userLogin);
        IEnumerable<Playlist> GetSortPlaylistsByOrder(PlaylistSortState sortOrder);
        Playlist GetByUserId(int userId);

        Playlist UpdateMyPlaylist(IsUpdata isUpdate, int userId, int songId = 0);
    }

    public class PlaylistService : IPlaylistService
    {
        private readonly IPlaylistRepository _playlistRepository;
        private readonly IUserRepository _userRepository;
        private readonly ApplicationDbContext _appDBContext;

        public PlaylistService(IPlaylistRepository playlistRepository, IUserRepository userRepository,
                            ApplicationDbContext appDBContext)
        {
            _playlistRepository = playlistRepository;
            _userRepository = userRepository;
            _appDBContext = appDBContext;
        }

        private bool IsExist(Playlist playlist)
        {
            return _playlistRepository.GetAll().FirstOrDefault(p =>
                    p.Name == playlist.Name) != null;
        }
        private bool IsNotExist(int id)
        {
            return _playlistRepository.GetByID(id) == null;
        }

        private bool PlaylistSongIsExist(int songId, int playlistId)
        {
            return _playlistRepository.GetAllSongPlaylist().FirstOrDefault(sp =>
                    sp.PlaylistId == songId &&
                    sp.SongId == playlistId) != null;
        }

        private bool PlaylistSongIsNotExist(int playlistId, int songId)
        {
            return _playlistRepository.GetSongPlaylist(playlistId, songId) == null;
        }
        public void Add(Playlist playlist)
        {
            _playlistRepository.Add(playlist);
        }

        public void Update(Playlist playlist)
        {
            _playlistRepository.Update(playlist);
        }

        public void Delete(int id)
        {
            _playlistRepository.Delete(id);
        }

        public IEnumerable<Playlist> GetAll()
        {
            return _playlistRepository.GetAll();
        }

        public Playlist GetByID(int id)
        {
            return _playlistRepository.GetByID(id);
        }

        public Playlist GetByName(string name)
        {
            return _playlistRepository.GetByName(name);
        }
        public Playlist GetByUserId(int userId)
        {
            return _playlistRepository.GetByUserId(userId);
        }

        //добавить ошибки
        public void AddSongToPlaylist(int songId, int playlistId)
        {
            _playlistRepository.AddSongPlaylist(songId, playlistId);
        }

        public void RemoveSongFromPlaylist(int songId, int playlistId)
        {
            _playlistRepository.DeleteSongPlaylist(songId, playlistId); 
        }

        public IEnumerable<SongPlaylist> GetAllSongPlaylists()
        {
            return _playlistRepository.GetAllSongPlaylist();
        }

        public IEnumerable<SongPlaylist> GetPlaylistsBySongId(int songId)
        {
            return _playlistRepository.GetSongPlaylistBySongId(songId);
        }

        public SongPlaylist GetSongPlaylist(int songId, int playlistId)
        {
            return _playlistRepository.GetSongPlaylist(songId, playlistId);   
        }

        public IEnumerable<Song> GetSongsByPlaylistId(int playlistId)
        {
            return _playlistRepository.GetSongsByPlaylistId(playlistId);
        }

        public IEnumerable<Song> GetSongsByUserId(int userId)
        {
            return _playlistRepository.GetSongsByUserId(userId);
        }


        //ПЕРЕДЕЛАТЬ
        public IEnumerable<Song> GetMySongsByUserLogin(string userLogin)
        {
            User user = _userRepository.GetByLogin(userLogin);
            IEnumerable<Song> mySongs;

            if (user == null)
                mySongs = null;
            else
                mySongs = _playlistRepository.GetSongsByUserId(user.Id);

            return mySongs;
        }

        public IEnumerable<Playlist> GetSortPlaylistsByOrder(PlaylistSortState sortOrder)
        {
            IEnumerable<Playlist> playlists = _playlistRepository.GetAll();
            IEnumerable<Playlist> needPlaylists = sortOrder switch
            {
                PlaylistSortState.IdDesc => playlists.OrderByDescending(elem => elem.Id),

                PlaylistSortState.NameAsc => playlists.OrderBy(elem => elem.Name),
                PlaylistSortState.NameDesc => playlists.OrderByDescending(elem => elem.Name),

                PlaylistSortState.DurationAsc => playlists.OrderBy(elem => elem.Duration),
                PlaylistSortState.DurationDesc => playlists.OrderByDescending(elem => elem.Duration),

                PlaylistSortState.CreationDateAsc => playlists.OrderBy(elem => elem.CreationDate),
                PlaylistSortState.CreationDateDesc => playlists.OrderByDescending(elem => elem.CreationDate),

                _ => playlists.OrderBy(elem => elem.Id)
            };

            return needPlaylists;
        }
        public Playlist UpdateMyPlaylist(IsUpdata isUpdate, int userId, int songId = 0)
        {
            Playlist playlist = _playlistRepository.GetByUserId(userId);
            if (isUpdate == IsUpdata.SongIsAdded)
            {
                AddSongToPlaylist(songId, playlist.Id);
            }
            else if (isUpdate == IsUpdata.SongIsDeleted)
            {
                RemoveSongFromPlaylist(songId, playlist.Id);
            }


            // if (isUpdate != IsUpdata.IsNotUpdate)
            //{
            //Реализация через процедуру БД
            //_appDBContext.Database.ExecuteSqlInterpolated($"CALL updatePlaylistRating({playlistId})");

            //Реализация через C#
            //UpdateMyPlaylistRating(playlist);

            //Реализация через триггеры;
            //}
            return _playlistRepository.GetByID(playlist.Id);
        }
    }
}
