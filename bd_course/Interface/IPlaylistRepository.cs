using System;
using System.Collections.Generic;
using bd_course.Models;
using NodaTime;

namespace bd_course.Interface
{
    public interface IPlaylistRepository : IRepository<Playlist>
    {
        Playlist GetByName(string name);
        Playlist GetByUserId(int userId);


        void AddSongPlaylist(int songId, int playlistId);
        void DeleteSongPlaylist(int songId, int playlistId);

        IEnumerable<SongPlaylist> GetAllSongPlaylist();
        IEnumerable<SongPlaylist> GetSongPlaylistBySongId(int songId);
        SongPlaylist GetSongPlaylist(int songId, int playlistId);
        IEnumerable<Song> GetSongsByPlaylistId(int playlistId);
        IEnumerable<Song> GetSongsByUserId(int userId);

    }
}
