using System.ComponentModel.DataAnnotations;
using NodaTime;

namespace bd_course.ViewModels
{
    public class DeleteSongViewModel
    {
        [Required(ErrorMessage = "Не указано название")]
        public string title { get; set; }

        [Required(ErrorMessage = "Не указан альбом")]
        public string albumTitle { get; set; }

        [Required(ErrorMessage = "Не указан жанр")]
        public string genre { get; set; }

        [Required(ErrorMessage = "Не указано имя артиста")]
        public string artistName { get; set; }
        [Required(ErrorMessage = "Не указана студия звукозаписи")]
        public string recordingStudioName { get; set; }
    }
}
