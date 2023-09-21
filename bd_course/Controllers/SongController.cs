using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bd_course.ViewModels;
using bd_course.Interface;
using Microsoft.AspNetCore.Mvc;
using bd_course.Models;
using bd_course.Services;
using static System.Collections.Specialized.BitVector32;
using System.Numerics;
using NodaTime;

namespace db_cp.Controllers
{
    public class SongController : Controller
    {
        //static private ISongRepository SongRepository = new SongMock();
        //static private IClubRepository clubRepository = new ClubMock();

        //private ISongService SongService = new SongService(SongRepository);
        //private IClubService clubService = new ClubService(clubRepository);

        private ISongService songService;
        private IArtistService artistService;
        private IPlaylistService playlistService;
        private IRecordingStudioService recordingStudioService;
        private IUserService userService;

        public SongController(ISongService songService,
                IArtistService artistService,
                IPlaylistService playlistService,
                IRecordingStudioService recordingStudioService,
                IUserService userService)
        {
            this.songService = songService;
            this.artistService = artistService;
            this.playlistService = playlistService;
            this.recordingStudioService = recordingStudioService;
            this.userService = userService;
        }

        public IActionResult GetAllSongs(SongSortState sortOrder = SongSortState.TitleDesc,
                                           string title = null, string albumTitle = null, string genre = null,
                                           string artistName = null, string recordingStudioName = null, int playlistId = 0)
        {
            ViewBag.Title = "Songs";

            ViewData["IdSort"] = sortOrder == SongSortState.IdAsc ? SongSortState.IdDesc : SongSortState.IdAsc;
            ViewData["TitleSort"] = sortOrder == SongSortState.TitleAsc ? SongSortState.TitleDesc : SongSortState.TitleAsc;
            ViewData["AlbumSort"] = sortOrder == SongSortState.AlbumAsc ? SongSortState.AlbumDesc : SongSortState.AlbumAsc;
            ViewData["GenreSort"] = sortOrder == SongSortState.GenreAsc ? SongSortState.GenreDesc : SongSortState.GenreAsc;
            ViewData["DurationSort"] = sortOrder == SongSortState.DurationAsc ? SongSortState.DurationDesc : SongSortState.DurationAsc;
            ViewData["ArtistNameSort"] = sortOrder == SongSortState.ArtistNameAsc ? SongSortState.ArtistNameDesc : SongSortState.ArtistNameAsc;
            ViewData["RecordingStudioNameSort"] = sortOrder == SongSortState.RecordingStudioNameAsc ? SongSortState.RecordingStudioNameDesc : SongSortState.RecordingStudioNameAsc;

            IEnumerable<Song> songs = songService.GetByParameters(title, albumTitle, genre, artistName,
                                                                        recordingStudioName, playlistId);

            SongViewModel allSongs = new SongViewModel
            {
                songs = songService.GetSortSongsByOrder(songs, sortOrder),
                mySongs = playlistService.GetMySongsByUserLogin(User.Identity.Name),
                artists = artistService.GetAll(),
                recordingStudios = recordingStudioService.GetAll(),

                filterSongViewModel = new FilterSongViewModel
                {
                    title = title,
                    albumTitle = albumTitle,
                    genre = genre,
                    artistName = artistName,
                    recordingStudioName = recordingStudioName,
                    playlistId = playlistId
                }
            };

            return View(allSongs);
        }
    }
}