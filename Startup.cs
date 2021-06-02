using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Data.Interfaces;
using BookStore.Data.Models;
using BookStore.Data.Repositories;
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

namespace BookstoreAPI
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
            services.AddTransient<IBookRepository, BookDatabase>();         //dependency injection, Bookdatabase is subbed everywhere there's IBookRepository
            //services.AddTransient<IBookRepository, BookRepository>();  

            services.AddControllers();

            var connection = @"Data Source=MARCDESKTOP;Initial Catalog=Bookstore.mdf;Integrated Security=True";
            
            services.AddDbContext<BookstoreContext>
                (options => options.UseSqlServer(connection));

            // Register the Swagger generator, defining 1 or more Swagger Documents
            services.AddSwaggerGen(c =>
           {
               c.SwaggerDoc("v1", new OpenApiInfo { Title = "The API", Version = "v1" });
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

            //this config must go exactly here. Url and port number of UI/forms
            app.UseCors(
                options => options.WithOrigins("https://localhost:44308")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
            );

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //for dropping and recreating the database
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<BookstoreContext>();
                //context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }

            app.UseSwagger();

            //Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            //Specifying the Swagger JSON endpoint
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                //To serve the Swagger UI at the app's root (http://localhost:<port>/), set the RoutePrefix property to an empty
                c.RoutePrefix = string.Empty;
            });
        }
    }
}
