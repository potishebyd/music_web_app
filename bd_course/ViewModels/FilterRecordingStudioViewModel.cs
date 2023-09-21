using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using bd_course.Models;

namespace bd_course.ViewModels
{
    public class FilterRecordingStudioViewModel
    {
        public string name { get; set; }
        public string country { get; set; }
        public int yearFounded { get; set; }
    }
}
