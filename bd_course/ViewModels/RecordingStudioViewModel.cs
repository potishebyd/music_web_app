using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Numerics;
using bd_course.Models;
using bd_course.ViewModels;

namespace bd_course.ViewModels
{
    public class RecordingStudioViewModel
    {
        public IEnumerable<RecordingStudio> recordingStudios { get; set; }

        public FilterRecordingStudioViewModel filterRecordingStudioViewModel { get; set; }
    }
}