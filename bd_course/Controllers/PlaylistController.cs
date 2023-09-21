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

namespace bd_course.Controllersa
{
    public class PlaylistController : Controller
    {
        //static private IPlaylistRepository playlistRepository = new PlaylistMock();
        //private IArtistService artistService = new ArtistService(artistRepository);

        private IPlaylistService playlistService;

        public PlaylistController(IPlaylistService playlistService)
        {
            this.playlistService = playlistService;
        }

        public IActionResult GetAllPlaylists(PlaylistSortState sortOrder = PlaylistSortState.IdAsc)
        {
            ViewBag.Title = "Playlists";

            ViewData["IdSort"] = sortOrder == PlaylistSortState.IdAsc ? PlaylistSortState.IdDesc : PlaylistSortState.IdAsc;
            ViewData["NameSort"] = sortOrder == PlaylistSortState.NameAsc ? PlaylistSortState.NameDesc : PlaylistSortState.NameAsc;
            ViewData["DurationSort"] = sortOrder == PlaylistSortState.DurationAsc ? PlaylistSortState.DurationDesc : PlaylistSortState.DurationAsc;
            ViewData["CreationDateSort"] = sortOrder == PlaylistSortState.CreationDateAsc ? PlaylistSortState.CreationDateDesc : PlaylistSortState.CreationDateAsc;
            
            var allPlaylists = new PlaylistViewModel
            {
                playlists = playlistService.GetSortPlaylistsByOrder(sortOrder)
            };

            return View(allPlaylists);
        }
    }
}
