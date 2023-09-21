using System;
using System.Collections.Generic;
using System.Linq;
using bd_course.Models;
using bd_course.Interface;
using NodaTime;

namespace bd_course.Services
{
    public interface IRecordingStudioService
    {
        void Add(RecordingStudio recordingStudio);
        void Delete(int id);
        void Update(RecordingStudio recordingStudio);

        IEnumerable<RecordingStudio> GetAll();
        RecordingStudio GetByID(int id);
        RecordingStudio GetByName(string name);
        IEnumerable<RecordingStudio> GetByCountry(string country);
        IEnumerable<RecordingStudio> GetByYearFounded(int yearFounded);
        IEnumerable<RecordingStudio> GetSortRecordingStudiosByOrder(IEnumerable<RecordingStudio> recordingStudios, RecordingStudioSortState sortOrder);
        IEnumerable<RecordingStudio> GetByParameters(string name, string country, int yearFounded);
    }

    public class RecordingStudioService : IRecordingStudioService
    {
        private readonly IRecordingStudioRepository _recordingStudioRepository;

        public RecordingStudioService(IRecordingStudioRepository recordingStudioRepository)
        {
            _recordingStudioRepository = recordingStudioRepository;
        }

        private bool IsExist(RecordingStudio recordingStudio)
        {
            return _recordingStudioRepository.GetAll().FirstOrDefault(elem => elem.Name == recordingStudio.Name) != null;
        }

        private bool IsNotExist(int id)
        {
            return _recordingStudioRepository.GetByID(id) == null;
        }

        public void Add(RecordingStudio recordingStudio)
        {
            if (IsExist(recordingStudio))
                throw new Exception("Студия звукозаписи с таким именем уже существует");

            _recordingStudioRepository.Add(recordingStudio);
        }

        public void Delete(int id)
        {
            if (IsNotExist(id))
                throw new Exception("Такой студии звукозаписи не существует");

            _recordingStudioRepository.Delete(id);
        }

        public IEnumerable<RecordingStudio> GetAll()
        {
            return _recordingStudioRepository.GetAll();
        }

        public RecordingStudio GetByID(int id)
        {
            return _recordingStudioRepository.GetByID(id);
        }

        public RecordingStudio GetByName(string name)
        {
            return _recordingStudioRepository.GetByName(name);
        }

        public IEnumerable<RecordingStudio> GetByCountry(string country)
        {
            return _recordingStudioRepository.GetByCountry(country);
        }

        public IEnumerable<RecordingStudio> GetByYearFounded(int yearFounded)
        {
            return _recordingStudioRepository.GetByYearFounded(yearFounded);
        }

        public void Update(RecordingStudio recordingStudio)
        {
            if (IsNotExist(recordingStudio.Id))
                throw new Exception("Такой студии звукозаписи не существует");

            _recordingStudioRepository.Update(recordingStudio);
        }

        public IEnumerable<RecordingStudio> GetSortRecordingStudiosByOrder(IEnumerable<RecordingStudio> recordingStudios, RecordingStudioSortState sortOrder)
        {
            IEnumerable<RecordingStudio> needRecordingStudioes = sortOrder switch
            {
                RecordingStudioSortState.IdDesc => recordingStudios.OrderByDescending(elem => elem.Id),

                RecordingStudioSortState.NameAsc => recordingStudios.OrderBy(elem => elem.Name),
                RecordingStudioSortState.NameDesc => recordingStudios.OrderByDescending(elem => elem.Name),

                RecordingStudioSortState.CountryAsc => recordingStudios.OrderBy(elem => elem.Country),
                RecordingStudioSortState.CountryDesc => recordingStudios.OrderByDescending(elem => elem.Country),

                RecordingStudioSortState.YearFoundedAsc => recordingStudios.OrderBy(elem => elem.YearFounded),
                RecordingStudioSortState.YearFoundedDesc => recordingStudios.OrderByDescending(elem => elem.YearFounded),

                _ => recordingStudios.OrderBy(elem => elem.Id)
            };

            return needRecordingStudioes;
        }

        public IEnumerable<RecordingStudio> GetByParameters(string name, string country, int yearFounded)
        {
            IEnumerable<RecordingStudio> recordingStudios;
            if (yearFounded == 0)
                recordingStudios = _recordingStudioRepository.GetAll();
            else
                recordingStudios = _recordingStudioRepository.GetAll().Where(elem => elem.YearFounded == yearFounded);

            if (recordingStudios.Count() != 0 && name != null)
                recordingStudios = recordingStudios.Where(elem => elem.Name == name);

            if (recordingStudios.Count() != 0 && country != null)
                recordingStudios = recordingStudios.Where(elem => elem.Country == country);

            return recordingStudios;
        }
    }
}
