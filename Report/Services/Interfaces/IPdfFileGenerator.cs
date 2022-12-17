using DinkToPdf;

namespace Reports.Services.Interfaces
{
    public interface IPdfFileGenerator
    {
        byte[] GeneratePdf(string template);
        byte[] GeneratePdf(string template, GlobalSettings settings);
        GlobalSettings GetDefaultSettings();
    }
}
