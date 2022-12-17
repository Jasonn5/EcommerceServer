using System.Collections.Generic;

namespace Reports.Services.Interfaces
{
    public interface ITemplateService
    {
        string GenerateTableTemplate<T>(string template, ICollection<T> collection) where T : class;
        string GenerateListTemplate<T>(string template, ICollection<T> collection) where T : class;
    }
}
