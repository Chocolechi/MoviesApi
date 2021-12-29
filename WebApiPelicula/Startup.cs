using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WebApiPelicula.Data;
using WebApiPelicula.Helpers;
using WebApiPelicula.PeliculasMapper;
using WebApiPelicula.Repository;
using WebApiPelicula.Repository.IRepository;

namespace WebApiPelicula
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
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IMovieRepository, MovieRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            /*add token dependency*/

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection("AppSettings:Token").Value)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            services.AddAutoMapper(typeof(PeliculasMappers));

            // Api documentation below
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("MovieApiCategories", new OpenApiInfo
                {
                    Title = "Category Api",
                    Version = "v1",
                    Description = "Backend Movies",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                    {
                        Email = "Abcermin@gmail.com",
                        Name = "Dav",
                        Url = new Uri("https://github.com/DavinciMontas")
                    },
                    License = new Microsoft.OpenApi.Models.OpenApiLicense()
                    {
                        Name = "MIT License",
                        Url = new Uri("https://es.wikipedia.org/wiki/Licencia_MIT")
                    }
                });
                c.SwaggerDoc("WebApiMovies", new OpenApiInfo
                {
                    Title = "Movie Api",
                    Version = "v1",
                    Description = "Backend Movies",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                    {
                        Email = "Abcermin@gmail.com",
                        Name = "Dav",
                        Url = new Uri("https://github.com/DavinciMontas")
                    },
                    License = new Microsoft.OpenApi.Models.OpenApiLicense()
                    {
                        Name = "MIT License",
                        Url = new Uri("https://es.wikipedia.org/wiki/Licencia_MIT")
                    }
                });
                c.SwaggerDoc("ApiMovieUsers", new OpenApiInfo
                {
                    Title = "User Api",
                    Version = "v1",
                    Description = "Backend Movies",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                    {
                        Email = "Abcermin@gmail.com",
                        Name = "Dav",
                        Url = new Uri("https://github.com/DavinciMontas")
                    },
                    License = new Microsoft.OpenApi.Models.OpenApiLicense()
                    {
                        Name = "MIT License",
                        Url = new Uri("https://es.wikipedia.org/wiki/Licencia_MIT")
                    }
                });

                var fileXmlComments = $"{Assembly.GetExecutingAssembly().GetName().Name }.xml";
                var apiCommentsRoute = Path.Combine(AppContext.BaseDirectory, fileXmlComments);
                c.IncludeXmlComments(apiCommentsRoute);

                // Define Security Scheme
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Aunthetication JWT (Bearer)",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement{
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Id = "Bearer",
                            Type = ReferenceType.SecurityScheme
                        }
                    }, new List<string>()
                }
            });
            });
            //support to cors
            services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/WebApiMovies/swagger.json", "Movies Api"));

            }
            else
            {
                app.UseExceptionHandler(builder =>
                {
                builder.Run(async context =>{
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    var error = context.Features.Get<IExceptionHandlerFeature>();
                    if (error != null)
                    {
                        context.Response.AddApplicationError(error.Error.Message);
                        await context.Response.WriteAsync(error.Error.Message);
                    }
                });
            });
        }

        app.UseHttpsRedirection();
            // doc line
            app.UseSwagger();

            app.UseRouting();
            // this methos belows are for the authentication and authorization
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            //support to cors
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
        }
    }
}
