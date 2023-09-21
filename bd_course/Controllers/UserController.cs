using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bd_course.ViewModels;
using bd_course.Interface;
using Microsoft.AspNetCore.Mvc;
using bd_course.Models;
using bd_course.Services;
using static System.Collections.Specialized.BitVector32;
using System.Numerics;
using NodaTime;

namespace bd_course.Controllers
{
    public class UserController : Controller
    {
        private IUserService userService;
        private IPlaylistService playlistService;

        public UserController(IUserService userService, IPlaylistService playlistService)
        {
            this.userService = userService;
            this.playlistService = playlistService;
        }

        public IActionResult GetAllUsers(UserSortState sortOrder = UserSortState.IdAsc)
        {
            ViewBag.Title = "Users";

            ViewData["IdSort"] = sortOrder == UserSortState.IdAsc ? UserSortState.IdDesc : UserSortState.IdAsc;
            ViewData["LoginSort"] = sortOrder == UserSortState.LoginAsc ? UserSortState.LoginDesc : UserSortState.LoginAsc;
            ViewData["EmailSort"] = sortOrder == UserSortState.EmailAsc ? UserSortState.EmailDesc : UserSortState.EmailAsc;
            ViewData["NamePlaylistSort"] = sortOrder == UserSortState.NamePlaylistAsc ? UserSortState.NamePlaylistDesc : UserSortState.NamePlaylistAsc;
            ViewData["PermissionSort"] = sortOrder == UserSortState.PermissionAsc ? UserSortState.PermissionDesc : UserSortState.PermissionAsc;
            var allUsers = new UserViewModel
            {
                playlists = playlistService.GetAll(),
                users = userService.GetSortUsersByOrder(sortOrder)
            };

            return View(allUsers);
        }

        public IActionResult СhangePermission(int id, string permission)
        {
            User user = userService.GetByID(id);

            user.Permission = permission;
            userService.Update(user);

            return RedirectToAction("GetAllUsers");
        }
    }
}
