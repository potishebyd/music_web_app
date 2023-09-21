using System;
using System.Collections.Generic;
using bd_course.Models;
using bd_course.Repository;
using NodaTime;

namespace bd_course.Interface
{
    public interface IRecordingStudioRepository : IRepository<RecordingStudio>
    {
        RecordingStudio GetByName(string name);
        IEnumerable<RecordingStudio> GetByCountry(string country);
        IEnumerable<RecordingStudio> GetByYearFounded(int yearFounded);
    }
}