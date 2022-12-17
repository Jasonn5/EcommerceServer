using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MakingArt.Controllers
{
    public class ExcelExportController : ControllerBase
    {

        internal string GetClaimValue(string claimName)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity.AuthenticationType != null)
            {
                return identity.FindFirst(claimName).Value;
            }

            return null;
        }
    }
}
