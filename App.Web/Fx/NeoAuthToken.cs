using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App.Web.Fx
{
    public class NeoAuthToken
    {
        public const string AuthTokenKey = "X-Neo-Auth";

        public Guid SID { get; set; }
        public Int32 UserID { get; set; }
        public String DisplayName { get; set; }
        public Int32 GroupID { get; set; }
        public List<Int32> AllowedPages { get; set; }

        public string SetAuthCookie(NeoAuthorization neoAuthorization)
        {
            if (neoAuthorization == null)
                throw new ArgumentNullException(nameof(neoAuthorization));

            var encToken = neoAuthorization.CreateAuthToken(this);
    
            if (string.IsNullOrEmpty(encToken))
                throw new InvalidOperationException("Generated token is null or empty.");

            var response = neoAuthorization.sessionHelper?.NeoContext?.InnerContext?.Response;
            if (response == null)
                throw new InvalidOperationException("Response object is null.");

            // response.Cookies.Append(NeoAuthToken.AuthTokenKey, encToken, new CookieOptions
            // {
            //     HttpOnly = true,  // Prevents access from JavaScript
            //     Secure = true,    // Ensures HTTPS usage
            //     SameSite = SameSiteMode.Strict, // Prevents CSRF
            //     Expires = DateTimeOffset.UtcNow.AddHours(1) // Adjust expiration as needed
            // });

            return encToken;
        }

        public static void ClearAuthCookie(NeoAuthorization neoAuthorization)
        {
            if (neoAuthorization == null)
                throw new ArgumentNullException(nameof(neoAuthorization));

            neoAuthorization.sessionHelper.LoggedUser = null;

            var response = neoAuthorization.sessionHelper?.NeoContext?.InnerContext?.Response;
            if (response == null)
                throw new InvalidOperationException("Response object is null.");

            response.Cookies.Delete(NeoAuthToken.AuthTokenKey);
        }

        public static string GetAuthCookie(NeoContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (context.InnerContext?.Request?.Cookies == null)
                throw new InvalidOperationException("Request cookies collection is null.");

            context.InnerContext.Request.Cookies.TryGetValue(NeoAuthToken.AuthTokenKey, out string auth);

            return auth;
        }

        public static string GetAuthTokenFromHeader(NeoContext context)
        {
            return Convert.ToString(context.InnerContext.Request.Headers[NeoAuthToken.AuthTokenKey]);
        }
    }
}