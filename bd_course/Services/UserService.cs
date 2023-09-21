using System;
using System.Collections.Generic;
using System.Linq;
using bd_course.Models;
using bd_course.Interface;
using NodaTime;

namespace bd_course.Services
{
    public interface IUserService
    {
        void Add(User user);
        void Delete(int id);
        void Update(User user);

        IEnumerable<User> GetAll();
        User GetByID(int id);
        User GetByLogin(string login);
        IEnumerable<User> GetByPermission(string permission);
        IEnumerable<User> GetSortUsersByOrder(UserSortState sortOrder);
    }

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPlaylistRepository _playlistRepository;

        public UserService(IUserRepository userRepository, IPlaylistRepository playlistRepository)
        {
            _userRepository = userRepository;
            _playlistRepository = playlistRepository;   
        }

        private bool IsExist(User user)
        {
            return _userRepository.GetAll().FirstOrDefault(elem => elem.Login == user.Login) != null;
        }

        private bool IsNotExist(int id)
        {
            return _userRepository.GetByID(id) == null;
        }

        public void Add(User user)
        {
            if (IsExist(user))
                throw new Exception("Пользователь с таким логином уже существует");

            _userRepository.Add(user);
        }

        public void Delete(int id)
        {
            if (IsNotExist(id))
                throw new Exception("Такого пользователя не существует");

            _userRepository.Delete(id);
        }

        public IEnumerable<User> GetAll()
        {
            return _userRepository.GetAll();
        }

        public User GetByID(int id)
        {
            return _userRepository.GetByID(id);
        }

        public User GetByLogin(string login)
        {
            return _userRepository.GetByLogin(login);
        }

        public IEnumerable<User> GetByPermission(string permission)
        {
            return _userRepository.GetByPermission(permission);
        }

        public void Update(User user)
        {
            if (IsNotExist(user.Id))
                throw new Exception("Такого пользователя не существует");

            _userRepository.Update(user);
        }
        public IEnumerable<User> GetSortUsersByOrder(UserSortState sortOrder)
        {
            IEnumerable<User> users = sortOrder switch
            {
                UserSortState.IdDesc => _userRepository.GetAll().OrderByDescending(elem => elem.Id),

                UserSortState.LoginAsc => _userRepository.GetAll().OrderBy(elem => elem.Login),
                UserSortState.LoginDesc => _userRepository.GetAll().OrderByDescending(elem => elem.Login),

                UserSortState.PermissionAsc => _userRepository.GetAll().OrderBy(elem => elem.Permission),
                UserSortState.PermissionDesc => _userRepository.GetAll().OrderByDescending(elem => elem.Permission),

                UserSortState.EmailAsc => _userRepository.GetAll().OrderBy(elem => elem.Email),
                UserSortState.EmailDesc => _userRepository.GetAll().OrderByDescending(elem => elem.Email),

                UserSortState.NamePlaylistAsc => _userRepository.GetAll().OrderBy(elem => _playlistRepository.GetByUserId(elem.Id).Name),
                UserSortState.NamePlaylistDesc => _userRepository.GetAll().OrderByDescending(elem => _playlistRepository.GetByUserId(elem.Id).Name),

                _ => _userRepository.GetAll().OrderBy(elem => elem.Id)
            };

            return users;
        }
    }
}
