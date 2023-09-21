using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using bd_course.Models;
using NodaTime;

namespace bd_course.ViewModels
{
    public class FilterSongViewModel
    {
        public string title { get; set; }
        public string albumTitle { get; set; }
        public string genre { get; set; }
        public string artistName { get; set; }
        public string recordingStudioName { get; set; }

        public int playlistId { get; set; }
        public string songSearch { get; set; }
    }
}
