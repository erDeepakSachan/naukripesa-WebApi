using App.Web.Fx;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace App.Web.Controllers
{
    public class DashboardController : NeoController
    {
        public DashboardController(WebHelper webHelper, WebHelper.SessionHelper sessionHelper) : base(webHelper, sessionHelper)
        {
        }

        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            return Ok();
        }
    }
}