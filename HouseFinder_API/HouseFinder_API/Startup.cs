using BusinessObjects;
using HouseFinder_API.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;
using Repositories.IRepository;
using Repositories.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseFinder_API
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
            //Add Session
            services.AddDistributedMemoryCache();
            services.AddSession(cfg => {
                cfg.IdleTimeout = TimeSpan.FromHours(1);
                cfg.Cookie.HttpOnly = true;
                cfg.Cookie.IsEssential = true;
                cfg.Cookie.Name = "HOUSEFINDER";
                cfg.Cookie.MaxAge = TimeSpan.FromHours(3);
                cfg.Cookie.SameSite = SameSiteMode.None;
                cfg.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
            });

            string[] origins = { "https://fu-house-finder.vercel.app", "https://localhost:4200" };
            //Add CORS policy
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                    builder.WithOrigins(origins)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

            //Configure OData service
            services.AddControllers().AddOData(option => option.Select().Filter().Count().OrderBy().Expand().SetMaxTop(1000));
            //services.AddDbContext<FUHouseFinderContext>(option =>
            //{
            //    option.UseSqlServer(Configuration.GetConnectionString("DBContext"));
            //});

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "HouseFinder_API", Version = "v1" });
            });
            var key = Configuration.GetSection("AppSettings").GetSection("Secret").Value;

            //JWT Authentication
            services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(token =>
            {
                token.RequireHttpsMetadata = false;
                token.SaveToken = true;
                token.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
                    ValidateIssuer = true,
                    ValidIssuer = Configuration.GetSection("AppSettings").GetSection("WebSiteDomain").Value,
                    ValidateAudience = true,
                    ValidAudience = Configuration.GetSection("AppSettings").GetSection("WebSiteDomain").Value,
                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Verified", policy => policy.RequireClaim("Status", "Verified"));
            });
            services.AddSingleton<IAuthentication>(new AuthenticationManager(key));
            string AwsAccessKey;
            string AwsSecretKey;
            string AwsBucket;

            try
            {
                AwsAccessKey = Configuration.GetSection("AWS").GetSection("AccessKey").Value;
                AwsSecretKey = Configuration.GetSection("AWS").GetSection("SecretKey").Value;
                AwsBucket = Configuration.GetSection("AWS").GetSection("Bucket").Value;
            }
            catch (Exception)
            {
                AwsAccessKey = "";
                AwsSecretKey = "";
                AwsBucket = "";
            }

            services.AddSingleton<IStorageRepository>(new StorageRepository(AwsAccessKey, AwsSecretKey, AwsBucket));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "HouseFinder_API v1"));
            }
            app.UseSession();
            app.Use(async (context, next) =>
            {
                var token = context.Request.Cookies["X-Access-Token"];
                if (!string.IsNullOrEmpty(token)) context.Request.Headers.Add("Authorization", "Bearer " + token);
                await next();
            });


            app.UseHttpsRedirection();

            app.UseRouting();

            //CORS policy
            app.UseCors();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
