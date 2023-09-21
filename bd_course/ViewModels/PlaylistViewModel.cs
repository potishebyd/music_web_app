using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Numerics;
using bd_course.Models;
using bd_course.ViewModels;
using NodaTime;

namespace bd_course.ViewModels
{
    public class PlaylistViewModel
    {
        public IEnumerable<Playlist> playlists { get; set; }
    }
}