using App.Web.Fx;
using App.Web.Models.Vm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Globalization;
using System.Text;
using App.Entity;
using App.Service;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using App.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Web.Controllers
{
    public class HomeController : NeoController
    {
        private readonly UserService service;
        private readonly CommonService commonService;

        public HomeController(UserService service, CommonService commonService, WebHelper webHelper,
            WebHelper.SessionHelper sessionHelper) : base(
            webHelper, sessionHelper)
        {
            this.service = service;
            this.commonService = commonService;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("/")]
        public IActionResult Index()
        {
            return Redirect("/swagger");
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("NotyOptions")]
        public ActionResult NotyOptions(bool forError = false)
        {
            var defaultType = "information";
            var varKey = "notyOpt";
            if (forError)
            {
                defaultType = "error";
                varKey = "notyOptError";
            }

            var defaultOpt = new { text = "", layout = "top", type = defaultType, closeButton = true, timeout = 700 };
            var notyOpt = JObject.Parse(JsonConvert.SerializeObject(defaultOpt));
            var pos = NeoContext.NotificationOptionLayout(NeoAuthorization);
            if (!string.IsNullOrWhiteSpace(pos))
            {
                notyOpt["layout"] = pos;
            }

            var type = NeoContext.NotificationOptionType(NeoAuthorization, forError);
            if (!string.IsNullOrWhiteSpace(type))
            {
                notyOpt["type"] = type;
            }

            return Content("var " + varKey + " = " + notyOpt.ToString() + ";");
        }
        [HttpPost]
        [Route("check-auth-status")]
        public IActionResult CheckAuthStatus()
        {
            return NeoData(new { status = "ok" });
        }

        [HttpPost]
        [Route("Logout")]
        public IActionResult Logout()
        {
            NeoAuthToken.ClearAuthCookie(NeoAuthorization);
            var model = new LoginRequest();
            return NeoView("Login", model);
        }

        [HttpPost]
        [Route("ClearAppCache")]
        public IActionResult ClearAppCache()
        {
            NeoContext.ClearAppCache();
            return NeoData("Done!");
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public IActionResult Login([FromBody] LoginRequest model)
        {
            try
            {
                var resp = NeoAuthorization.SignMeIn(model);
                if (resp != null && resp.SID != Guid.Empty)
                {
                    var encToken = resp.SetAuthCookie(NeoAuthorization);
                    return NeoData(model: new { token = encToken });
                }

                return NeoView("Login", model);
            }
            catch (NeoAuthException ex)
            {
                var model1 = new List<string>() { ex.Message };
                return NeoView("Login", model1, true);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("ResetPassword")]
        public IActionResult ResetPassword([FromBody] ResetPasswordRequest model)
        {
            try
            {
                var resp = NeoAuthorization.VerifyUser(model);
                if (resp)
                {
                    //Reset password in DB
                    User obj = new User();
                    obj.ModifiedOn = DateTime.UtcNow;
                    obj.Email = model.UserName;
                    obj.MobileNo = model.MobileNo;
                    NeoAuthorization.cryptographyHelper.EncodingType = CryptographyHelper.EncodingBaseTypes.Hex;

                    #region CreateTempPassword

                    Random random = new Random();
                    int RandomNum = random.Next(999, 99999);
                    string TempPassword = model.UserName.Substring(1, 2) + RandomNum + model.UserName.Substring(0, 3);

                    #endregion

                    obj.Password =
                        NeoAuthorization.cryptographyHelper.Encrypt(TempPassword,
                            NeoContext.PassPhrase(NeoAuthorization));
                    var success = service.Update(obj).Result;
                    var resp2 = new NeoApiResponse();
                    if (success)
                    {
                        //Send OTP
                        // Util util = new Util();
                        // Util.Email(model.UserName, TempPassword);
                        resp2 = resp2.SuccessResponse(null, "Password Updated successfully.");
                    }
                    else
                    {
                        resp2 = resp2.ErrorResponse("There is something wrong, Please contact to system admin.");
                    }
                }

                return NeoView("ResetPassword", model);
            }
            catch (NeoAuthException ex)
            {
                return NeoView("Login", new { error = ex.Message });
            }
        }

        [HttpPost]
        [Route("LoadDropDownOptions")]
        public IActionResult LoadDropDownOptions([FromQuery] string type, [FromQuery] string selectedValue)
        {
            var options = commonService.GetStaticDdl(type);
            return NeoData(options);
        }
    }
}