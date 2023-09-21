using System;
using System.Collections.Generic;
using System.Linq;
using bd_course.Models;
using bd_course.Interface;
using Microsoft.EntityFrameworkCore;
using NodaTime;

namespace bd_course.Repository
{
    public class ArtistRepository : IArtistRepository
    {
        private readonly ApplicationDbContext _context;

        public ArtistRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(Artist model)
        {
            try
            {
                _context.Artists.Add(model);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Ошибка при добавлении артиста");
            }
        }

        public void Update(Artist model)
        {
            try
            {
                _context.Artists.Update(model);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Ошибка при обновлении артиста");
            }
        }

        public void Delete(int id)
        {
            try
            {
                var artist = _context.Artists.Find(id);
                if (artist != null)
                {
                    _context.Artists.Remove(artist);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Ошибка при удалении артиста");
            }
        }

        public IEnumerable<Artist> GetAll()
        {
            return _context.Artists.ToList();
        }

        public Artist GetByID(int id)
        {
            return _context.Artists.Find(id);
        }

        public Artist GetByName(string name)
        {
            return _context.Artists.FirstOrDefault(a => a.Name == name);
        }

        public IEnumerable<Artist> GetByCountry(string country)
        {
            return _context.Artists.Where(a => a.Country == country).ToList();
        }
    }
}
