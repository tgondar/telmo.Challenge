using Application.Behaviours;
using Application.Interfaces;
using Application.Mappings;
using Backend.Challenge.Middleware;
using FluentValidation;
using Infrastructure;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using static Application.IdeaUpdates.Queries.GetIdeaUpdates.GetIdeaUpdatesQuery;

namespace Backend.Challenge
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
            services.AddScoped<RavenDbContext>();
            services.AddScoped<IIdeaUpdateRepository, IdeaUpdateRepository>();

            services.AddValidatorsFromAssembly(typeof(GetIdeaUpdatesQueryHandler).Assembly);

            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(typeof(GetIdeaUpdatesQueryHandler).Assembly);
                config.AddOpenBehavior(typeof(ValidationBehavior<,>));
            });

            services.AddExceptionHandler<CustomExceptionHandler>();

            services.AddAutoMapper(typeof(IdeaUpdatesProfile).Assembly);

            services.AddRazorPages();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseExceptionHandler(options => { });

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapRazorPages();
            });
        }
    }
}
