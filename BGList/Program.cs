using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace BGList
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            // Fixing ruting conflicts
            //builder.Services.AddSwaggerGen();
            builder.Services.AddSwaggerGen(opts =>
                opts.ResolveConflictingActions(apiDesc => apiDesc.First())
            );

            // Adding CORS
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(cfg =>
                {
                    cfg.WithOrigins(builder.Configuration["AllowedOrigins"]);
                    cfg.AllowAnyHeader();
                    cfg.AllowAnyMethod();
                });
                options.AddPolicy(name: "AnyOrigin",
                cfg =>
                {
                    cfg.AllowAnyOrigin();
                    cfg.AllowAnyHeader();
                    cfg.AllowAnyMethod();
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            if (app.Configuration.GetValue<bool>("UseDeveloperExceptionPage"))
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
            }

            app.UseHttpsRedirection();
            app.UseCors();
            app.UseAuthorization();

            // Minimal API
            //app.MapGet("/error", [EnableCors("AnyOrigin")] () => Results.Problem());
            app.MapGet("/error/test",
                [ResponseCache(NoStore = true)]
            () =>
                { throw new Exception("test"); }
                ).RequireCors("AnyOrigin");

            app.MapGet("/code/test",
            [EnableCors("AnyOrigin")]
            [ResponseCache(NoStore = true)] () =>
                    Results.Text("<script>" +
                    "window.alert('Your client supports JavaScript!" +
                    "\\r\\n\\r\\n" +
                    $"Server time (UTC): {DateTime.UtcNow.ToString("o")}" +
                    "\\r\\n" +
                    "Client time (UTC): ' + new Date().toISOString());" +
                    "</script>" +
                    "<noscript>Your client does not support JavaScript</noscript>",
                    "text/html"));

            app.MapControllers();

            app.Run();
        }
    }
}