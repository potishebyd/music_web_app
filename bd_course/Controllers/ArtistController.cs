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
using Microsoft.EntityFrameworkCore;
using NodaTime;


namespace bd_course.Controllers
{
    public class ArtistController : Controller
    {
        //static private IArtistRepository artistRepository = new ArtistMock();
        //private IArtistService artistService = new ArtistService(artistRepository);

        IArtistService artistService;
        private readonly ApplicationDbContext _appDBContext;

        public ArtistController(IArtistService artistService, ApplicationDbContext appDBContext)
        {
            this.artistService = artistService;
            this._appDBContext = appDBContext;
        }

        public IActionResult GetAllArtists(ArtistSortState sortOrder = ArtistSortState.IdAsc,
                                         string name = null, string country = null)
        {
            ViewBag.Title = "Artists";

            ViewData["IdSort"] = sortOrder == ArtistSortState.IdAsc ? ArtistSortState.IdDesc : ArtistSortState.IdAsc;
            ViewData["NameSort"] = sortOrder == ArtistSortState.NameAsc ? ArtistSortState.NameDesc : ArtistSortState.NameAsc;
            ViewData["CountrySort"] = sortOrder == ArtistSortState.CountryAsc ? ArtistSortState.CountryDesc : ArtistSortState.CountryAsc;
            //ViewData["FoundationDateSort"] = sortOrder == ArtistSortState.FoundationDateAsc ? ArtistSortState.FoundationDateDesc : ArtistSortState.FoundationDateAsc;

            //Реализация через C#
            //IEnumerable<Artist> artists = artistService.GetByParameters(name, country, minFoundationDate, maxFoundationDate);

            //Реализация через функцию БД
            IEnumerable<Artist> artists = _appDBContext.Artists
                .FromSqlInterpolated($"SELECT * FROM getArtistsByParameters({name}, {country})");

            ArtistViewModel allArtists = new ArtistViewModel
            {
                artists = artistService.GetSortArtistsByOrder(artists, sortOrder),

                filterArtistViewModel = new FilterArtistViewModel
                {
                    name = name,
                    country = country
                }
            };

            return View(allArtists);
        }
    }
}
