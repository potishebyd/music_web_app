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

namespace bd_course.Controllers
{
    public class HomeController : Controller
    {
        private IUserService userService;
        private ISongService songService;
        private IPlaylistService playlistService;
        private IArtistService artistService;
        private IRecordingStudioService recordingStudioService;


        public HomeController(IUserService userService,
                              ISongService songService,
                              IPlaylistService playlistService,
                              IArtistService artistService,
                              IRecordingStudioService recordingStudioService)
        {
            this.userService = userService;
            this.songService = songService;
            this.playlistService = playlistService;
            this.artistService = artistService;
            this.recordingStudioService = recordingStudioService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "DoStart";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(HomeViewModel model, IsAction isAction = IsAction.SearchSongs)
        {
            ViewBag.Title = "Find";
            //if (ModelState.IsValid)
            //{
                if (isAction == IsAction.SearchSongs)
                {
                    return SearchSnogs(model.filterSongViewModel);
                }
                else if (isAction == IsAction.SearchArtists)
                {
                    return SearchArtists(model.filterArtistViewModel);
                }
                else if (isAction == IsAction.SearchRecordingStudios)
                {
                    return SearchRecordingStudios(model.filterRecordingStudioViewModel);
                }
                else if (isAction == IsAction.AddSong)
                {
                    return AddSong(model.addSongViewModel);
                }
                else if (isAction == IsAction.DeleteSong)
                {
                // Настроить удаление из плейлистов!!!!
                    return DeleteSong(model.deleteSongViewModel);
                }
            //}
           // else
             //   ModelState.AddModelError("", "Некорректные данные Index");

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        private IActionResult SearchSnogs(FilterSongViewModel model)
        {
            int playlistId = getPlaylistId(model.songSearch);

            return RedirectToAction("GetAllSongs", "Song",
                new
                {
                    title = model.title,
                    genre = model.genre,
                    albumTitle = model.albumTitle,
                    artistName = model.artistName,
                    recordingStudioName = model.recordingStudioName,
                    playlistId = playlistId
                });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        private IActionResult SearchArtists(FilterArtistViewModel model)
        {
            return RedirectToAction("GetAllArtists", "Artist",
                new
                {
                    name = model.name,
                    country = model.country
                });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        private IActionResult SearchRecordingStudios(FilterRecordingStudioViewModel model)
        {

            return RedirectToAction("GetAllRecordingStudios", "RecordingStudio",
                new
                {
                    name = model.name,
                    country = model.country,
                    yearFounded = model.yearFounded
                });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        private IActionResult AddSong(AddSongViewModel model)
        {
            try
            {
                Console.WriteLine(model.artistName);
                Console.WriteLine(model.recordingStudioName);
                Song song = new Song
                {
                    Title = model.title,
                    AlbumTitle = model.albumTitle,
                    Genre = model.genre,
                    Duration = model.duration,
                    ArtistId = artistService.GetByName(model.artistName).Id,
                    RecordingStudioId = recordingStudioService.GetByName(model.recordingStudioName).Id
                };

                songService.Add(song);

                return RedirectToAction("GetAllSongs", "Song",
                    new
                    {
                        title = model.title,
                        albumTitle = model.albumTitle,
                        genre = model.genre,
                        duration = model.duration,
                        artistName = model.artistName,
                        recordingStudioName = model.recordingStudioName,
                        playlistId = 0
                    });
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                ModelState.AddModelError("", "Некорректные данные AddSong");
            }

            return View(new HomeViewModel { addSongViewModel = model });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        private IActionResult DeleteSong(DeleteSongViewModel model)
        {
            try
            {
                Song song = songService.GetByParameters(model.title, model.albumTitle, model.genre, model.artistName, model.recordingStudioName).First();
                //ДОБАВТЬ!!!!
                //playlistService.DeleteAllPlaylistSong(song.Id);
                songService.Delete(song);

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                ModelState.AddModelError("", "Некорректные данные");
            }

            return View(new HomeViewModel { deleteSongViewModel = model });
        }

        private uint checkForNull(uint? value)
        {
            uint newValue;

            if (value != null)
                newValue = (uint)value;
            else
                newValue = 0;

            return newValue;
        }

        private int getPlaylistId(string playerSearch)
        {
            int playlistId;

            if (playerSearch == "MyPlaylistSongs")
            {
                int userId = userService.GetByLogin(User.Identity.Name).Id;
                playlistId = playlistService.GetByUserId(userId).Id;
            }

            else
                playlistId = 0;

            return playlistId;
        }
    }
}