using System.Threading.Tasks;
using bd_course.ViewModels;
using bd_course.Interface;
using Microsoft.AspNetCore.Mvc;
using bd_course.Models;
using bd_course.Services;
using static System.Collections.Specialized.BitVector32;
using System.Numerics;
using NodaTime;

namespace bd_course.Controllers
{
    public class MyPlaylistController : Controller
    {
        private IPlaylistService playlistService;
        private IUserService userService;
        private ISongService songService;
        private IArtistService artistService;
        private IRecordingStudioService recordingStudioService;

        public MyPlaylistController(IPlaylistService playlistService,
                                 IUserService userService,
                                 ISongService songService,
                                 IArtistService artistService,
                                 IRecordingStudioService recordingStudioService)
        {
            this.playlistService = playlistService;
            this.userService = userService;
            this.songService = songService;
            this.artistService = artistService;
            this.recordingStudioService = recordingStudioService;   
        }


        public IActionResult GetMyPlaylist(SongSortState sortOrder = SongSortState.IdAsc,
                                        IsUpdata isUpdate = IsUpdata.IsNotUpdate,
                                        int songId = 0, int coachId = 0)
        {
            ViewBag.Title = "MyPlaylist";

            ViewData["IdSort"] = sortOrder == SongSortState.IdAsc ? SongSortState.IdDesc : SongSortState.IdAsc;
            ViewData["TitleSort"] = sortOrder == SongSortState.TitleAsc ? SongSortState.TitleDesc : SongSortState.TitleAsc;
            ViewData["AlbumSort"] = sortOrder == SongSortState.AlbumAsc ? SongSortState.AlbumDesc : SongSortState.AlbumAsc;
            ViewData["GenreSort"] = sortOrder == SongSortState.GenreAsc ? SongSortState.GenreDesc : SongSortState.GenreAsc;
            ViewData["DurationSort"] = sortOrder == SongSortState.DurationAsc ? SongSortState.DurationDesc : SongSortState.DurationAsc;
            ViewData["ArtistNameSort"] = sortOrder == SongSortState.ArtistNameAsc ? SongSortState.ArtistNameDesc : SongSortState.ArtistNameAsc;
            ViewData["RecordingStudioNameSort"] = sortOrder == SongSortState.RecordingStudioNameAsc ? SongSortState.RecordingStudioNameDesc : SongSortState.RecordingStudioNameAsc;

            User user = userService.GetByLogin(User.Identity.Name);
            Playlist playlist = playlistService.UpdateMyPlaylist(isUpdate, user.Id, songId);
            IEnumerable<Song> songs = playlistService.GetSongsByPlaylistId(playlist.Id);

            MyPlaylistViewModel myPlaylistViewModel = new MyPlaylistViewModel
            {
                myPlaylist = playlist,
                mySongs = songService.GetSortSongsByOrder(songs, sortOrder),
                artists = artistService.GetAll(),
                recordingStudios = recordingStudioService.GetAll(),
                song = songService.GetByID(songId),
                _isUpdate = isUpdate
            };

            return View(myPlaylistViewModel);
        }
    }
}
