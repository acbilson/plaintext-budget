using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using PTB.Core;
using PTB.Core.Logging;
using PTB.Core.Statements;
using PTB.Files.Categories;
using PTB.Files.FolderAccess;
using PTB.Files.Ledger;
using PTB.Files.Statements;
using PTB.Files.TitleRegex;
using System;
using System.IO;

namespace PTB.Web
{
    public class Startup
    {
        private readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        private void LoadServiceConfiguration(IServiceCollection services)
        {
            string home = Environment.GetEnvironmentVariable("ONEDRIVECOMMERCIAL");
            string baseDir = Path.Combine(home, @"Archive\2019\BudgetProject\PTB_Home");

            var settingsText = File.ReadAllText(Path.Combine(baseDir, "settings.json"));
            var settings = JsonConvert.DeserializeObject<PTBSettings>(settingsText);

            var schemaText = File.ReadAllText(Path.Combine(baseDir, "schema.json"));
            var schema = JsonConvert.DeserializeObject<FileSchema>(schemaText);

            var logger = new PTBFileLogger(settings.LoggingLevel, baseDir);

            services.AddSingleton<IPTBLogger>(logger)

            .AddSingleton<PTBSettings>(settings)

            .AddSingleton<FileSchema>(schema)
            .AddSingleton<LedgerSchema>(schema.Ledger)
            .AddSingleton<CategoriesSchema>(schema.Categories)
            .AddSingleton<TitleRegexSchema>(schema.TitleRegex)

            .AddSingleton<IStatementParser, PNCParser>()
            .AddSingleton<LedgerFileParser>()
            .AddSingleton<CategoriesFileParser>()
            .AddSingleton<TitleRegexFileParser>()

            .AddScoped<FileFolderService>()

            .AddScoped<LedgerService>()
            .AddScoped<CategoriesService>()
            .AddScoped<TitleRegexService>();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            LoadServiceConfiguration(services);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            // courtesy of https://docs.microsoft.com/en-us/aspnet/core/security/cors?view=aspnetcore-2.2
            // 4200 is the Angular port, and 4500 is the Protractor e2e port, and 5000 is the .NET Core server port
            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins, builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                // allow cross-origin to host SPA on a different port
                app.UseCors(MyAllowSpecificOrigins);
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseProxyToSpaDevelopmentServer("http://localhost:4200");
                    //spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}