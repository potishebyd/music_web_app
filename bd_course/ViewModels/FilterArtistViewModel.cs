using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using bd_course.Models;

namespace bd_course.ViewModels
{
    public class FilterArtistViewModel
    {
        public string name { get; set; }
        public string country { get; set; }
    }
}
