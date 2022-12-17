using Microsoft.AspNetCore.Mvc;

namespace Entities.RequestParameters
{
    [BindProperties]
    public class ProductRequestParameters
    {
        [BindProperty]
        public string Search { get; set; }
    }
}
