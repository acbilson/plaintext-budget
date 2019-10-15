using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using PTB.Core;
using PTB.Core.Base;
using PTB.Core.Logging;
using PTB.Core.Statements;
using PTB.Files;
using PTB.Files.FolderAccess;
using PTB.Files.Ledger;
using PTB.Files.Statements;
using PTB.Files.TitleRegex;
using PTB.Report;
using PTB.Reports.Budget;
using PTB.Reports.Categories;
using PTB.Reports.FolderAccess;
using System;
using System.IO;

namespace PTB.Web
{
    public class Startup
    {
        private readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        private IHostingEnvironment _env;

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }

        public IConfiguration Configuration { get; }

        private void LoadServiceConfiguration(IServiceCollection services, string baseDir)
        {
            Console.WriteLine("Beginning service configuration...");
            Console.WriteLine($"base directory is: {baseDir}");

            string settingsPath = Path.Combine(baseDir, "wwwroot", "settings.json");
            Console.WriteLine($"settings path is: {settingsPath}");
            if (!File.Exists(settingsPath)) throw new FileNotFoundException($"Settings could not be found at: {settingsPath}");

            string schemaPath = Path.Combine(baseDir, "wwwroot", "schema.json");
            Console.WriteLine($"schema path is: {schemaPath}");
            if (!File.Exists(schemaPath)) throw new FileNotFoundException($"Schema could not be found at: {schemaPath}");

            var settingsText = File.ReadAllText(settingsPath);
            var settings = JsonConvert.DeserializeObject<PTBSettings>(settingsText);

            var schemaText = File.ReadAllText(schemaPath);
            var fileSchema = JsonConvert.DeserializeObject<FileSchema>(schemaText);
            var reportSchema = JsonConvert.DeserializeObject<ReportSchema>(schemaText);

            var logger = new PTBFileLogger(settings.LoggingLevel, baseDir);

            services.AddSingleton<IPTBLogger>(logger)

            .AddSingleton<PTBSettings>(settings)

            .AddSingleton<FileSchema>(fileSchema)
            .AddSingleton<LedgerSchema>(fileSchema.Ledger)
            .AddSingleton<TitleRegexSchema>(fileSchema.TitleRegex)

            .AddSingleton<ReportSchema>(reportSchema)
            .AddSingleton<BudgetSchema>(reportSchema.Budget)
            .AddSingleton<CategoriesSchema>(reportSchema.Categories)

            .AddSingleton<FileValidation>()
            .AddSingleton<IStatementParser, PNCParser>()
            .AddSingleton<LedgerFileParser>()
            .AddSingleton<CategoriesFileParser>()
            .AddSingleton<TitleRegexFileParser>()
            .AddSingleton<BudgetReportParser>()

            .AddScoped<FileFolderService>()
            .AddScoped<ReportFolderService>()

            .AddScoped<LedgerService>()
            .AddScoped<CategoriesService>()
            .AddScoped<TitleRegexService>()
            .AddScoped<BudgetService>();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string baseDir = _env.ContentRootPath;
            LoadServiceConfiguration(services, baseDir);

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
        public void Configure(IApplicationBuilder app)
        {
            if (_env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                // allow cross-origin to host SPA on a different port
                app.UseCors(MyAllowSpecificOrigins);
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            // maybe need this to use Angular in wwwroot
            //app.UseDefaultFiles();
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

                if (_env.IsDevelopment())
                {
                    spa.UseProxyToSpaDevelopmentServer("http://localhost:4200");
                    //spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}