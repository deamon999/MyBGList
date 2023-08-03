using BGList.Model;
using BGList.Swagger;

using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;

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
            {
                opts.ResolveConflictingActions(apiDesc => apiDesc.First());
                opts.ParameterFilter<SortOrderFilter>();
            }
            );
            builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

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

            // MVC Api Versioning
            builder.Services.AddApiVersioning(options =>
            {
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
            });
            builder.Services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(
                builder.Configuration.GetConnectionString("DefaultConnection"))
            );

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
                    foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
                    {
                        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                            description.GroupName.ToUpperInvariant());
                    }
                });
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
            app.MapGet("/v{version:ApiVersion}/error/test",
                [ApiVersion("1.0")]
            [ApiVersion("2.0")]
            [ResponseCache(NoStore = true)] () =>
                    { throw new Exception("test"); }
                    ).RequireCors("AnyOrigin");

            app.MapGet("/v{version:ApiVersion}/code/test",
                [ApiVersion("1.0")]
            [ApiVersion("2.0")]
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