using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Authentication.Cookies;
using bd_course.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bd_course.Services;
using bd_course.Repository;
using bd_course.Interface;
using NodaTime;

namespace bd_course
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Configuration["DatabaseConnection"] = configuration.GetConnectionString("guestConnection");
        }


        public void ConfigureServices(IServiceCollection services)
        {
            // Добавление контекста данных и настройка строки подключения к PostgreSQL
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(Configuration["DatabaseConnection"]),
                ServiceLifetime.Transient);

            services.AddSingleton<IConfiguration>(Configuration);
            // Добавление сервисов для аутентификации и авторизации (если необходимо)
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
                    options.AccessDeniedPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
                });

            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IArtistService, ArtistService>();
            services.AddTransient<ISongService, SongService>();
            services.AddTransient<IPlaylistService, PlaylistService>();
            services.AddTransient<IRecordingStudioService, RecordingStudioService>();

            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IArtistRepository, ArtistRepository>();
            services.AddTransient<ISongRepository, SongRepository>();
            services.AddTransient<IPlaylistRepository, PlaylistRepository>();
            services.AddTransient<IRecordingStudioRepository, RecordingStudioRepository>();


            // Добавление поддержки MVC
            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            // Добавление маршрутов для контроллеров
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Account}/{action=Logout}/{id?}");
            });
        }
    }
}
