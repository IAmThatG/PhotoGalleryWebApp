using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace PhotoGallery.web
{
    internal static class AuthManager
    {
        public static int GetID(this IIdentity identity)
        {
            var claimsIdentity = identity as ClaimsIdentity;
            var userID = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            return Convert.ToInt32(userID.Value);
        }
    }
}