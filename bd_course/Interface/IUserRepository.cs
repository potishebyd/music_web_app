using System;
using System.Collections.Generic;
using bd_course.Models;
using NodaTime;

namespace bd_course.Interface
{
    public interface IUserRepository : IRepository<User>
    {
        User GetByLogin(string login);
        IEnumerable<User> GetByPermission(string permission);
    }
}