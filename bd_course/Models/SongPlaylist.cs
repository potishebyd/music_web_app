using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bd_course.Models
{
    public class SongPlaylist
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Song")]
        public int SongId { get; set; }
        //public Song Song { get; set; }

        [Required]
        [ForeignKey("Playlist")]
        public int PlaylistId { get; set; }
        //public Playlist Playlist { get; set; }
    }
}
