using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bd_course.ViewModels;
using bd_course.Interface;
using bd_course.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using bd_course.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Configuration;
using NodaTime;

namespace bd_course.Controllers
{
    public class AccountController : Controller
    {
        //static private IUserRepository userRepository = new UserMock();
        //private IUserService userService = new UserService(userRepository);

        private readonly IConfiguration configuration;

        IUserService userService;
        IPlaylistService playlistService;

        public AccountController(IConfiguration configuration,
                                 IUserService userService,
                                 IPlaylistService playlistService)
        {
            this.configuration = configuration;
            this.userService = userService;
            this.playlistService = playlistService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            ViewBag.Title = "Register";

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            ViewBag.Title = "Register";

            if (ModelState.IsValid)
            {
                try
                {
                    User user = new User
                    {
                        Login = model.Login,
                        Password = model.Password,
                        Email = model.Email,
                        Permission = "user"
                    };

                    userService.Add(user);

                    Playlist playlist = new Playlist
                    {
                        Name = model.NamePlaylist,
                        UserId = user.Id

                    };

                    playlistService.Add(playlist);

                    await Authenticate(user);
                    ChangeConnection(user.Permission);

                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                    ModelState.AddModelError("", ex.Message);
                }
            }
            else
                ModelState.AddModelError("", "Некорректные данные");

            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            ViewBag.Title = "Login";

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            ViewBag.Title = "Login";

            if (ModelState.IsValid)
            {
                User user = userService.GetByLogin(model.Login);

                if (user != null && user.Password == model.Password)
                {
                    await Authenticate(user);
                    ChangeConnection(user.Permission);

                    return RedirectToAction("Index", "Home");
                }
                else
                    ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            else
                ModelState.AddModelError("", "Некорректные логин и(или) пароль");

            return View(model);
        }

        private async Task Authenticate(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Permission)
            };

            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            ChangeConnection("guest");

            return RedirectToAction("Index", "Home");
        }

        private void ChangeConnection(string permission)
        {
            if (permission == "user")
                configuration["DatabaseConnection"] = configuration.GetConnectionString("userConnection");
            else if (permission == "admin")
                configuration["DatabaseConnection"] = configuration.GetConnectionString("adminConnection");
            else
                configuration["DatabaseConnection"] = configuration.GetConnectionString("guestConnection");

            Console.WriteLine(permission);
            Console.WriteLine(configuration["DatabaseConnection"]);
        }
    }
}