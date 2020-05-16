using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FamousQuoteQuiz.Data;
using FamousQuoteQuiz.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using FamousQuoteQuiz.Data.Interfaces;
using FamousQuoteQuiz.Data.Repository;

namespace FamousQuoteQuiz
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddMvc();

            services.AddDbContext<QuizDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("FamousQuoteDatabase")));

            services.AddTransient<IUserAnswerRepository, UserAnswerRepository>();
            services.AddTransient<IQuotesRepository, QuotesRepository>();
            services.AddTransient<IUserRepository, UserRepository>();

            services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequiredUniqueChars = 4;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;

            }).AddEntityFrameworkStores<QuizDbContext>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                   name: "default",
                   pattern: "{controller=Main}/{action=Index}");
            });
        }
    }
}
