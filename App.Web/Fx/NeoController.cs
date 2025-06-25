using App.Web.Controllers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace App.Web.Fx
{
    [ServiceFilter(typeof(NeoAuthorizeAttribute))]
    [ApiController]
    [Route("api/[controller]")]
    public class NeoController : ControllerBase, IActionFilter
    {
        private WebHelper.SessionHelper sessionHelper;
        protected NeoContext NeoContext;
        protected NeoAuthorization NeoAuthorization;
        protected readonly WebHelper WebHelper;

        public NeoController(WebHelper webHelper, WebHelper.SessionHelper sessionHelper)
        {
            this.WebHelper = webHelper;
            this.sessionHelper = sessionHelper;
        }

        public WebHelper.SessionHelper SessionHelper => sessionHelper;

        private void InitializeSessionHelper(HttpContext httpContext)
        {
            this.NeoAuthorization = httpContext.RequestServices.GetService<NeoAuthorization>();
            this.NeoContext = this.SessionHelper.NeoContext;
        }

        protected NeoListingResponse<T> ToListingResponse<T>(List<T> data, int pageNo = 0, int count = 0)
            where T : class, new()
        {
            int offset = (WebHelper.DefaultGridPageSize * (pageNo)) + 1;
            var actualCount = data.Count;
            if (actualCount < WebHelper.DefaultGridPageSize)
            {
                for (int i = 0; i < WebHelper.DefaultGridPageSize - actualCount; i++)
                {
                    data.Add(new T());
                }
            }

            var list = new NeoListingResponse<T>();
            list.Data = data;
            list.PageSize = WebHelper.DefaultGridPageSize;
            list.CurrentPageNo = pageNo + 1;
            list.TotalItemCount = count;
            list.TotalPageCount = Math.Ceiling(Convert.ToDouble(list.TotalItemCount) / list.PageSize);
            return list;
        }

        [NonAction]
        public void OnActionExecuting(ActionExecutingContext context)
        {
            InitializeSessionHelper(context.HttpContext);
        }

        [NonAction]
        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        [NonAction]
        protected IActionResult NeoData(object model, bool error = false)
        {
            var apiResponse = model as NeoApiResponse;
            if (apiResponse == null)
            {
                apiResponse = PrepareApiResponse(model, error);
            }

            return new JsonNetResult(apiResponse);
        }

        [NonAction]
        protected IActionResult NeoView(string viewName, object model, bool error = false)
        {
            var apiResponse = model as NeoApiResponse;
            if (apiResponse == null)
            {
                apiResponse = PrepareApiResponse(model, error);
            }

            return new JsonNetResult(apiResponse);
        }

        [NonAction]
        protected ActionResult NeoRedirect<T>(string actionName, object routeValues = null, object model = null,
            bool error = false)
        {
            if (this.NeoContext.IsApiRequest)
            {
                var apiResponse = model as NeoApiResponse;
                if (apiResponse == null)
                {
                    apiResponse = PrepareApiResponse(model, error);
                }

                return new JsonNetResult(apiResponse);
            }

            return RedirectToMethod<T>(actionName, routeValues);
        }

        [NonAction]
        protected RedirectToActionResult RedirectToMethod<T>(string actionName, object routeValues = null)
        {
            var controllerName = "";
            var clazz = typeof(T);
            if (clazz.BaseType == typeof(NeoController))
            {
                controllerName = clazz.Name.Replace("Controller", "");
            }

            if (String.IsNullOrEmpty(controllerName))
            {
                return RedirectToAction(actionName, routeValues);
            }

            return RedirectToAction(actionName, controllerName, routeValues);
        }

        private NeoApiResponse PrepareApiResponse(object model, bool error = false)
        {
            if (error)
            {
                return new NeoApiResponse().ErrorResponse(null, errors: model);
            }

            return new NeoApiResponse().SuccessResponse(model);
        }

        protected NeoAuthToken GetLoggedInUser()
        {
            var loggenInUser = HttpContext.RequestServices.GetService<NeoAuthorization>().GetLoggedInUser();
            return loggenInUser;
        }

    }

    public class NeoAuthorizeAttribute : AuthorizeAttribute, IAsyncAuthorizationFilter
    {
        private readonly NeoAuthorization neoAuthorization;

        public NeoAuthorizeAttribute(NeoAuthorization neoAuthorization)
        {
            this.neoAuthorization = neoAuthorization;
        }

        private async Task On_Authorization(AuthorizationFilterContext context)
        {
            // Check if it's a valid Controller action (not a constructor/private method)
            if (context.ActionDescriptor is not ControllerActionDescriptor actionDescriptor)
            {
                return;  // Skip non-action methods
            }

            // Skip if AllowAnonymous is present
            if (context.ActionDescriptor.EndpointMetadata.Any(em => em is AllowAnonymousAttribute))
            {
                return;
            }

            var user = context.HttpContext.User;

            // 🔥 Force authentication to ensure OnChallenge is called
            if (!user.Identity?.IsAuthenticated ?? true)
            {
                context.Result = new ChallengeResult();
                return;
            }
            
            if (!neoAuthorization.IsPageAccessible())
            {
                HandleUnAuthorizeRequest(context);
            }
        }

        private void HandleUnAuthorizeRequest(AuthorizationFilterContext filterContext)
        {
            var model = new NeoApiResponse().ErrorResponse("You are not authorize to access this page.", 403);
            filterContext.Result = new JsonNetResult(model);
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            await On_Authorization(context);
        }
    }
}