using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Numerics;
using bd_course.Models;
using bd_course.ViewModels;
using NodaTime;

namespace bd_course.ViewModels
{
    public class SongViewModel
    {
        public IEnumerable<Song> songs { get; set; }
        public IEnumerable<Song> mySongs { get; set; }
        public IEnumerable<Artist> artists { get; set; }
        public IEnumerable<RecordingStudio> recordingStudios { get; set; }

        public FilterSongViewModel filterSongViewModel { get; set; }
    }
}
