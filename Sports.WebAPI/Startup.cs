﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sports.Repository.Context;
using Sports.Logic;
using Sports.Logic.Interface;
using Sports.WebAPI.Models;
using Sports.Repository.Interface;
using Sports.Repository.UnitOfWork;

namespace Sports.WebAPI
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
            services.AddMvc().AddJsonOptions(
            options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
        ).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
           /* services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });*/
            services.AddDbContext<RepositoryContext>(options => options.UseSqlServer(Configuration.GetConnectionString("SportsDatabase")));
            services.AddScoped<IUserLogic, UserLogic>();
            services.AddScoped<IFavoriteLogic, FavoriteLogic>();
            services.AddScoped<ICompetitorLogic, CompetitorLogic>();
            services.AddScoped<ISportLogic, SportLogic>();
            services.AddScoped<IMatchLogic, MatchLogic>();
            services.AddScoped<IFixtureLogic, FixtureLogic>();
            services.AddScoped<ISessionLogic, SessionLogic>();
            services.AddScoped<ILogLogic, TextLog>();
            services.AddScoped<IRepositoryUnitOfWork, RepositoryUnitOfWork>();
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder =>
                    {
                        builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().AllowCredentials();
                    });

            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("AllowAllOrigins");
            app.UseMvc();
        }
    }
}
