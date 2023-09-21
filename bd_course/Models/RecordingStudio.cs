using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bd_course.Models
{
    public class RecordingStudio
    {
        [Key] // Указывает, что это первичный ключ
        public int Id { get; set; }

        [Required(ErrorMessage = "Studio name is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Studio name must be between 2 and 100 characters.")]
        public string Name { get; set; }

        public int YearFounded { get; set; }

        [Required(ErrorMessage = "Country is required.")]
        public string Country { get; set; }

        //public ICollection<Song> Songs { get; set; }
    }
}
