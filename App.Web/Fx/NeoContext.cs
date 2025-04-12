using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Entity;
using App.Service;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace App.Web.Fx
{
    public class NeoContext
    {
        private IHttpContextAccessor context;
        private Dictionary<string, int> webPages;
        private List<Entity.UserGroupPermission> webPagesPermissions;
        private String currentTheme;

        public HttpContext InnerContext { get; set; }
        private readonly IMemoryCache cache;
        private readonly CommonService commonService;
        private readonly WebpageService webpageService;
        private readonly UserGroupPermissionService userGroupPermissionService;

        public NeoContext(CommonService commonService
            , WebpageService webpageService
            , UserGroupPermissionService userGroupPermissionService
            , IHttpContextAccessor context, IMemoryCache cache)
        {
            this.commonService = commonService;
            this.webpageService = webpageService;
            this.userGroupPermissionService = userGroupPermissionService;
            this.context = context;
            this.InnerContext = this.context.HttpContext;
            this.cache = cache;
        }

        public bool IsApiRequest
        {
            get
            {
                if ("NeoApiCall".Equals(this.context.HttpContext.Request.Headers["X-Neo-Agent"],
                        StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }

                return false;
            }
        }

        public string ControllerName
        {
            get { return Convert.ToString(context.HttpContext.Request.RouteValues["controller"]).ToLower(); }
        }

        public string ActionName
        {
            get { return Convert.ToString(context.HttpContext.Request.RouteValues["action"]).ToLower(); }
        }

        public string AuthTokenFromRqeuest
        {
            get
            {
                // var auth = NeoAuthToken.GetAuthTokenFromHeader(this);
                // if (String.IsNullOrWhiteSpace(auth))
                // {
                //     auth = NeoAuthToken.GetAuthCookie(this);
                // }

                var auth = InnerContext.User.FindFirst(NeoAuthToken.AuthTokenKey)?.Value;
                return auth;
            }
        }

        public List<Entity.UserGroupPermission> WebPagePermissions(NeoAuthorization neoAuthorization)
        {
            if (this.webPagesPermissions == null)
            {
                this.webPagesPermissions = cache.Get<List<Entity.UserGroupPermission>>("X-Neo-WebPages-Permissions");
            }

            if (this.webPagesPermissions == null)
            {
                this.webPagesPermissions = userGroupPermissionService.GetAll().ToList();
                cache.Set("X-Neo-WebPages-Permissions", this.webPagesPermissions);
            }

            return this.webPagesPermissions;
        }

        public Dictionary<string, int> WebPages(NeoAuthorization neoAuthorization)
        {

            if (this.webPages == null)
            {
                this.webPages = cache.Get<Dictionary<string, int>>("X-Neo-WebPages");
            }

            if (this.webPages == null)
            {
                this.webPages = new Dictionary<string, int>();
                foreach (Webpage P in webpageService.GetAll().ToList())
                {
                    webPages.Add(P.Url.Replace(".aspx", "").Trim().ToLower(), P.WebpageId);
                }

                cache.Set("X-Neo-WebPages", this.webPages);
            }

            return webPages;
        }

        public void ClearAppCache()
        {
            (cache as MemoryCache)?.Compact(1.0);
        }

        public string CurrentTheme(NeoAuthorization neoAuthorization)
        {
            if (String.IsNullOrWhiteSpace(this.currentTheme))
            {
                this.currentTheme = Convert.ToString(cache.Get("X-Neo-Current-Theme"));
            }

            if (String.IsNullOrWhiteSpace(this.currentTheme))
            {
                this.currentTheme = commonService.GetSettingValueFromDb("AppTheme");
                cache.Set("X-Neo-Current-Theme", this.currentTheme);
            }

            return this.currentTheme;
        }

        public string NotificationOptionLayout(NeoAuthorization neoAuthorization)
        {
            var currentValue = Convert.ToString(cache.Get("X-Neo-Noty-Opt-Layout"));

            if (String.IsNullOrWhiteSpace(currentValue))
            {
                currentValue = commonService.GetSettingValueFromDb("AppNotyOptLayout");
                cache.Set("X-Neo-Noty-Opt-Layout", currentValue);
            }

            return currentValue;
        }

        public string PassPhrase(NeoAuthorization neoAuthorization)
        {
            var currentValue = Convert.ToString(cache.Get("X-Neo-PassPhrase"));

            if (String.IsNullOrWhiteSpace(currentValue))
            {
                currentValue = commonService.GetSettingValueFromDb("Passphrase");
                cache.Set("X-Neo-PassPhrase", currentValue);
            }

            return currentValue;
        }

        public string NotificationOptionType(NeoAuthorization neoAuthorization, bool forError = false)
        {
            var hashKey = "X-Neo-Noty-Opt-Type";
            var settingKey = "AppNotyOptType";
            if (forError)
            {
                hashKey = "X-Neo-Noty-Opt-Type-Error";
                settingKey = "AppNotyOptTypeError";
            }

            var currentValue = Convert.ToString(cache.Get(hashKey));

            if (String.IsNullOrWhiteSpace(currentValue))
            {
                currentValue = commonService.GetSettingValueFromDb(settingKey);
                cache.Set(hashKey, currentValue);
            }

            return currentValue;
        }
    }
}