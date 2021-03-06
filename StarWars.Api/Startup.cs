using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using StarWars.Data;
using StarWars.Data.Repositories;
using StarWars.Domain;
using StarWars.Domain.Repositories;
using StarWars.Domain.Services;

namespace StarWars
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(setup =>
            {
                setup.SwaggerDoc("StarWarsApiDocumentation", new OpenApiInfo()
                {
                    Title = "Star Wars Api",
                    Version="1"
                });
                var xmlCommentFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlCommentFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentFile);
                setup.IncludeXmlComments(xmlCommentFullPath);
            });
            services.AddAutoMapper(typeof(Startup));
            services.AddDbContext<StarWarsContext>();
            services.AddScoped<IEpisodeRepository, EpisodeRepository>();
            services.AddScoped<ICharacterRepository, CharacterRepository>();
            services.AddScoped<ICharacterService, CharacterService>();
            services.AddDbContext<StarWarsContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("StarWarsDatabase")));
            services.AddMvc()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(setup =>
            {
                setup.SwaggerEndpoint("/swagger/StarWarsApiDocumentation/swagger.json", "endpoint");
                setup.RoutePrefix = string.Empty;
            });
        }
    }
}
