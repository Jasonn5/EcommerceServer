using Microsoft.AspNetCore.Mvc;
using System;

namespace Entities.RequestParameters
{
    [BindProperties]
    public class SaleRequestParameters
    {
        [BindProperty]
        public string Search { get; set; }

        [BindProperty]
        public DateTime StartDate { get; set; }

        [BindProperty]
        public DateTime EndDate { get; set; }
    }
}
