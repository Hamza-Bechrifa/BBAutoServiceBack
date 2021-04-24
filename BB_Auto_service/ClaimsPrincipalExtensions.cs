using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Gateway
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetUserId(this ClaimsPrincipal principal)

        {

            if (!principal.Identity.IsAuthenticated)

                return null;


            ClaimsPrincipal currentUser = principal;

            return currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

        }
    }
}
