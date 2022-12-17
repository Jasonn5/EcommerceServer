using DinkToPdf;
using DinkToPdf.Contracts;
using Reports.Services.Interfaces;

namespace Reports.Services
{
    public class PdfFileGenerator : IPdfFileGenerator
    {
        private readonly IConverter _converter;

        public PdfFileGenerator(IConverter converter)
        {
            _converter = converter;
        }

        public byte[] GeneratePdf(string template)
        {
            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.Letter,
                Margins = new MarginSettings(),
                DocumentTitle = "PDF Report"
            };

            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = template,
                WebSettings = { DefaultEncoding = "utf-8" },
                //HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [page] of [toPage]", Line = false },
                //FooterSettings = { FontName = "Arial", FontSize = 9, Line = false, Center = "Report Footer" }
            };

            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };

            var file = _converter.Convert(pdf);

            return file;
        }

        public byte[] GeneratePdf(string template, GlobalSettings settings)
        {
            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = template,
                WebSettings = { DefaultEncoding = "utf-8" },
                //HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [page] of [toPage]", Line = false },
                //FooterSettings = { FontName = "Arial", FontSize = 9, Line = false, Center = "Report Footer" }
            };

            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = settings,
                Objects = { objectSettings }
            };

            var file = _converter.Convert(pdf);

            return file;
        }

        public GlobalSettings GetDefaultSettings()
        {
            return new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.Letter,
                Margins = new MarginSettings(),
                DocumentTitle = "PDF Report"
            };
        }
    }
}
