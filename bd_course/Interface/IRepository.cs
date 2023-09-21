using System;
using System.Collections.Generic;
using bd_course.Models;
using NodaTime;

namespace bd_course.Interface
{
    public interface IRepository<T>
    {
        void Add(T model);
        void Update(T model);
        void Delete(int id);

        IEnumerable<T> GetAll();
        T GetByID(int id);
    }
}
