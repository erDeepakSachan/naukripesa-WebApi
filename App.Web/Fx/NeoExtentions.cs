using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using Microsoft.AspNetCore.Mvc.Razor;

namespace App.Web.Fx
{
    public static class NeoExtentions
    {
        public static NeoController GetController(this RazorPage webViewPage)
        {
            return (null as NeoController);
        }

        public static NeoAuthToken LoggedUser(this RazorPage webViewPage)
        {
            return null;//webViewPage.GetController().SessionHelper.LoggedUser;
        }

        public static Int32 UserId(this ClaimsPrincipal user)
        {
            return Int32.Parse(user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value ?? "0");
        }
    }
}