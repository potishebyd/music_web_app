using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bd_course.Models
{
    public class Artist
    {
        [Key] // Указывает, что это первичный ключ
        public int Id { get; set; }

        [Required(ErrorMessage = "Artist name is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Artist name must be between 2 and 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Artist country is required.")]
        public string Country { get; set; }

        //public ICollection<Song> Songs { get; set; }
    }
}
