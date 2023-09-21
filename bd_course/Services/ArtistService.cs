using System;
using System.Collections.Generic;
using System.Linq;
using bd_course.Models;
using bd_course.Interface;
using NodaTime;

namespace bd_course.Services
{
    public interface IArtistService
    {
        void Add(Artist artist);
        void Delete(int id);
        void Update(Artist artist);

        IEnumerable<Artist> GetAll();
        Artist GetByID(int id);
        Artist GetByName(string name);
        IEnumerable<Artist> GetByCountry(string country);
        IEnumerable<Artist> GetSortArtistsByOrder(IEnumerable<Artist> artists, ArtistSortState sortOrder);
        IEnumerable<Artist> GetByParameters(string name, string country);
    }

    public class ArtistService : IArtistService
    {
        private readonly IArtistRepository _artistRepository;

        public ArtistService(IArtistRepository artistRepository)
        {
            _artistRepository = artistRepository;
        }

        private bool IsExist(Artist artist)
        {
            return _artistRepository.GetAll().FirstOrDefault(elem => elem.Name == artist.Name) != null;
        }

        private bool IsNotExist(int id)
        {
            return _artistRepository.GetByID(id) == null;
        }

        public void Add(Artist artist)
        {
            if (IsExist(artist))
                throw new Exception("Артист с таким именем уже существует");

            _artistRepository.Add(artist);
        }

        public void Delete(int id)
        {
            if (IsNotExist(id))
                throw new Exception("Такого артиста не существует");

            _artistRepository.Delete(id);
        }

        public IEnumerable<Artist> GetAll()
        {
            return _artistRepository.GetAll();
        }

        public Artist GetByID(int id)
        {
            return _artistRepository.GetByID(id);
        }

        public Artist GetByName(string name)
        {
            return _artistRepository.GetByName(name);
        }

        public IEnumerable<Artist> GetByCountry(string country)
        {
            return _artistRepository.GetByCountry(country);
        }

        public void Update(Artist artist)
        {
            if (IsNotExist(artist.Id))
                throw new Exception("Такого артиста не существует");

            _artistRepository.Update(artist);
        }

        public IEnumerable<Artist> GetSortArtistsByOrder(IEnumerable<Artist> artists, ArtistSortState sortOrder)
        {
            IEnumerable<Artist> needArtists = sortOrder switch
            {
                ArtistSortState.IdDesc => artists.OrderByDescending(elem => elem.Id),

                ArtistSortState.NameAsc => artists.OrderBy(elem => elem.Name),
                ArtistSortState.NameDesc => artists.OrderByDescending(elem => elem.Name),

                ArtistSortState.CountryAsc => artists.OrderBy(elem => elem.Country),
                ArtistSortState.CountryDesc => artists.OrderByDescending(elem => elem.Country),

                _ => artists.OrderBy(elem => elem.Id),
            };

            return needArtists;
        }

        public IEnumerable<Artist> GetByParameters(string name, string country)
        {
            IEnumerable<Artist> artists = _artistRepository.GetAll();

            if (artists.Count() != 0 && name != null)
                artists = artists.Where(elem => elem.Name == name);

            if (artists.Count() != 0 && country != null)
                artists = artists.Where(elem => elem.Country == country);

            return artists;
        }
    }
}
