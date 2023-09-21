using System;
using System.Collections.Generic;
using bd_course.Models;
using NodaTime;

namespace bd_course.Interface
{
    public interface IArtistRepository : IRepository<Artist>
    {
        Artist GetByName(string name);
        IEnumerable<Artist> GetByCountry(string country);
    }
}
