#region startup

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Serilog;
using TodoApi.Models;
using TodoApi.Middleware;
using TodoApi.DAL;

namespace TodoApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)//, ILogger logger)
        {
            Configuration = configuration;
            //Logger = logger;          
        }

        public IConfiguration Configuration { get; }
        //public ILogger Logger { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<TodoContext>(opt => opt.UseInMemoryDatabase("TodoList"));
            services.AddControllers();
            services.AddSingleton(Log.Logger);
            services.AddScoped<ITodoItemsRepository, TodoItemsRepository>();

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.WithOrigins("http://localhost:4200", "http://localhost:8080", "http://localhost:80")
                    .WithMethods("POST", "GET", "OPTIONS", "PUT", "DELETE")//AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

            //services.AddCors(options =>
            //{
            //    options.AddPolicy("CorsPolicy",
            //        builder => builder.WithOrigins("http://localhost:4200")
            //        .AllowAnyMethod()
            //        .AllowAnyHeader()
            //        .AllowCredentials());
            //});
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors("CorsPolicy");
            //app.UseCors();
            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            //app.UseSerilogRequestLogging();
            app.ConfigureExceptionHandler(Log.Logger);
        }
    }
}
#endregion