using System.Globalization;
using System.Linq;
using Distribution.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Microsoft.Extensions.Options;
using Serilog;

namespace Distribution
{
    internal static class ApplicationBuilderExtensions
    {
        internal static IApplicationBuilder UseExceptionHandling(
            this IApplicationBuilder app,
            IWebHostEnvironment env)
        {
            Log.Information("-== ASPNETCORE_ENVIRONMENT:{0} ==-", env.EnvironmentName);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }

            return app;
        }

        //        internal static void ConfigureSwagger(this IApplicationBuilder app)
        //        {
        //            app.UseSwagger();
        //            app.UseSwaggerUI(options =>
        //            {
        //                options.SwaggerEndpoint("/swagger/v1/swagger.json", typeof(Program).Assembly.GetName().Name);
        //                options.RoutePrefix = "swagger";
        //                options.DisplayRequestDuration();
        //            });
        //        }

        //        internal static IApplicationBuilder UseEndpoints(this IApplicationBuilder app)
        //            => app.UseEndpoints(endpoints =>
        //            {
        //                endpoints.MapRazorPages();
        //                endpoints.MapControllers();
        //                endpoints.MapFallbackToFile("index.html");
        //                endpoints.MapHub<SignalRHub>(ApplicationConstants.SignalR.HubUrl);
        //            });

        //        internal static IApplicationBuilder UseRequestLocalizationByCulture(this IApplicationBuilder app)
        //        {
        //            var supportedCultures =
        //                LocalizationConstants.SupportedLanguages.Select(l => new CultureInfo(l.Code)).ToArray();
        //            app.UseRequestLocalization(options =>
        //            {
        //                options.SupportedUICultures = supportedCultures;
        //                options.SupportedCultures = supportedCultures;
        //                options.DefaultRequestCulture = new RequestCulture(supportedCultures.First());
        //                options.ApplyCurrentCultureToResponseHeaders = true;
        //            });

        //            app.UseMiddleware<RequestCultureMiddleware>();

        //            return app;
        //        }



        internal static IApplicationBuilder Initialize(this IApplicationBuilder app,
        Microsoft.Extensions.Configuration.IConfiguration _configuration)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();

            var initializers = serviceScope.ServiceProvider.GetServices<IAppInitialiser>();

            foreach (var initializer in initializers)
            {
                initializer.Initialize();
            }

            return app;

        }
    }
}