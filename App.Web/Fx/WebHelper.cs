using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace App.Web.Fx
{
    public class WebHelper
    {
        public const string SESSION_LOGGED_USER = "SESSION_LOGGED_USER";
        public const string APP_SETTINGS_SECTION = "AppSettings";
        public string AppConnectionString { get; set; }
        public string NeoPassPhrase{ get; set; }
        public string AppName{ get; set; }
        public int DefaultGridPageSize{ get; set; }
        public string AppLogoPath{ get; set; }
        public string AppDescription{ get; set; }
        public string AuthDomain { get; set; }
        public string AuthKey { get; set; }

        public class SessionHelper
        {
            private IHttpContextAccessor httpContext;
            private readonly NeoContext neoContext;

            public SessionHelper(IHttpContextAccessor httpContext,NeoContext neoContext)
            {
                this.httpContext = httpContext;
                this.neoContext=neoContext;
            }

            public NeoAuthToken LoggedUser
            {
                get
                {
                    var authToken = this.httpContext.HttpContext.Items[WebHelper.SESSION_LOGGED_USER] as NeoAuthToken;
                    if (authToken == null)
                    {
                        authToken = new NeoAuthToken();
                    }
                    return authToken;
                }
                set
                {
                    this.httpContext.HttpContext.Items[WebHelper.SESSION_LOGGED_USER] = value;
                }
            }

            public NeoContext NeoContext => neoContext;
        }
    }
}