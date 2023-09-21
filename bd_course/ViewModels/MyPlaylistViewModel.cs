using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Numerics;
using bd_course.Models;
using bd_course.ViewModels;
using NodaTime;

namespace bd_course.ViewModels
{
    public enum IsUpdata
    {
        SongIsAdded,
        SongIsDeleted,

        IsNotUpdate
    }

    public class MyPlaylistViewModel
    {
        public Playlist myPlaylist { get; set; }
        public IEnumerable<Song> mySongs { get; set; }
        public IEnumerable<Artist> artists { get; set; }
        public IEnumerable<RecordingStudio> recordingStudios { get; set; }

        public Song song { get; set; }
        public IsUpdata _isUpdate { get; set; }
    }
}
