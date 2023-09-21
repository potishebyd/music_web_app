using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Numerics;
using bd_course.Models;
using bd_course.ViewModels;

namespace bd_course.ViewModels
{
    public class ArtistViewModel
    {
        public IEnumerable<Artist> artists { get; set; }

        public FilterArtistViewModel filterArtistViewModel { get; set; }
    }
}
