using System;
using System.Collections.Generic;
using bd_course.Models;
using bd_course.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using NodaTime;

namespace bd_course.ViewModels
{
    public enum IsAction
    {
        SearchSongs,
        SearchArtists,
        SearchRecordingStudios,
        AddSong,
        DeleteSong
    }

    public class HomeViewModel
    {   
        public FilterSongViewModel filterSongViewModel { get; set; }
        public FilterArtistViewModel filterArtistViewModel { get; set; }
        public FilterRecordingStudioViewModel filterRecordingStudioViewModel { get; set; }

        public AddSongViewModel addSongViewModel { get; set; }
        public DeleteSongViewModel deleteSongViewModel { get; set; }
    }
}