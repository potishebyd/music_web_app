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
    public class RecordingStudioController : Controller
    {
        //static private IRecordingStudioRepository recordingStudioRepository = new RecordingStudioMock();
        //private IRecordingStudioService recordingStudioService = new RecordingStudioService(recordingStudioRepository);

        private IRecordingStudioService recordingStudioService;

        public RecordingStudioController(IRecordingStudioService recordingStudioService)
        {
            this.recordingStudioService = recordingStudioService;
        }

        public IActionResult GetAllRecordingStudios(RecordingStudioSortState sortOrder = RecordingStudioSortState.IdAsc,
                                           string name = null, string country = null,
                                           int yearFounded = 0)
        {
            ViewBag.Title = "RecordingStudios";

            ViewData["IdSort"] = sortOrder == RecordingStudioSortState.IdAsc ? RecordingStudioSortState.IdDesc : RecordingStudioSortState.IdAsc;
            ViewData["NameSort"] = sortOrder == RecordingStudioSortState.NameAsc ? RecordingStudioSortState.NameDesc : RecordingStudioSortState.NameAsc;
            ViewData["CountrySort"] = sortOrder == RecordingStudioSortState.CountryAsc ? RecordingStudioSortState.CountryDesc : RecordingStudioSortState.CountryAsc;
            ViewData["YearFoundedSort"] = sortOrder == RecordingStudioSortState.YearFoundedAsc ? RecordingStudioSortState.YearFoundedDesc : RecordingStudioSortState.YearFoundedAsc;

            IEnumerable<RecordingStudio> recordingStudios = recordingStudioService.GetByParameters(name, country, yearFounded);

            RecordingStudioViewModel allRecordingStudios = new RecordingStudioViewModel
            {
                recordingStudios = recordingStudioService.GetSortRecordingStudiosByOrder(recordingStudios, sortOrder),

                filterRecordingStudioViewModel = new FilterRecordingStudioViewModel
                {
                    name = name,
                    country = country
                }
            };

            return View(allRecordingStudios);
        }
    }
}
