using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Reports.Services;
using Reports.Services.Interfaces;

namespace Reports
{
    public static class StartupHelper
    {
        public static void RegisterTypes(IServiceCollection services)
        {
            services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
            services.AddTransient<IPdfFileGenerator, PdfFileGenerator>();
            services.AddTransient<ITemplateService, TemplateService>();
        }
    }
}
