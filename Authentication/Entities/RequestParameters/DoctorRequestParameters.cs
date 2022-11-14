using Microsoft.AspNetCore.Mvc;

namespace Authentication.Entities.RequestParameters
{
    [BindProperties]
    public class DoctorRequestParameters
    {
        [BindProperty]
        public string Search { get; set; }
    }
}
