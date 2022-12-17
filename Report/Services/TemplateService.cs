using Reports.Services.Interfaces;
using System.Collections.Generic;
using System.Text;

namespace Reports.Services
{
    public class TemplateService : ITemplateService
    {
        public string GenerateListTemplate<T>(string template, ICollection<T> collection) where T : class
        {
            var sb = new StringBuilder();

            foreach (var item in collection)
            {
                sb.Append("<li>");

                var properties = item.GetType().GetProperties();
                foreach (var property in properties)
                {
                    var value = property.GetValue(item, null);
                    sb.Append($"{value},");
                }

                sb.Length--;

                sb.AppendLine("</li>");
            }

            template = template.Replace("{VALUES TO POPULATE}", sb.ToString());

            return template;
        }

        public string GenerateTableTemplate<T>(string template, ICollection<T> collection) where T : class
        {
            var sb = new StringBuilder();

            foreach (var item in collection)
            {
                sb.Append("<tr>");

                var properties = item.GetType().GetProperties();
                foreach (var property in properties)
                {
                    var value = property.GetValue(item, null) ?? string.Empty;

                    if (property.PropertyType == typeof(int) || property.PropertyType == typeof(int?) || property.PropertyType == typeof(decimal) || property.PropertyType == typeof(decimal?))
                    {
                        sb.Append($"<td style=\"text-align: right;\">{value}</td>");
                    }
                    else
                    {
                        sb.Append($"<td style=\"text-align: left;\">{value}</td>");
                    }
                }

                sb.AppendLine("</tr>");
            }

            template = template.Replace("{VALUES TO POPULATE}", sb.ToString());

            return template;
        }
    }
}
