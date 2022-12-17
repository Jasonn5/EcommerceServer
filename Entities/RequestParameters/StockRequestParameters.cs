using Microsoft.AspNetCore.Mvc;

namespace Entities.RequestParameters
{
    [BindProperties]
    public class StockRequestParameters
    {
        [BindProperty]
        public string Search { get; set; }
    }
}
