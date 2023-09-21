using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Numerics;
using bd_course.Models;
using bd_course.ViewModels;

namespace bd_course.ViewModels
{
    public class UserViewModel
    {
        public IEnumerable<User> users { get; set; }
        public IEnumerable<Playlist> playlists { get; set; }
    }
}
