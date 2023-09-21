using System;
using System.Collections.Generic;
using bd_course.Models;
using NodaTime;

namespace bd_course.Interface
{
    public interface ISongRepository : IRepository<Song>
    {
        Song GetByTitle(string title);
        IEnumerable<Song> GetByAlbumTitle(string albumTitle);
        IEnumerable<Song> GetByGenre(string genre);
        IEnumerable<Song> GetByDuration(TimeSpan Duration);
    }
}
