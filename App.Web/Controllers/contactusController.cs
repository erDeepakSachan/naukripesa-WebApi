using App.Entity;
using App.Service;
using App.Web.Fx;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using App.Util;

namespace App.Web.Controllers
{
    public class contactusController : NeoController
    {
        private readonly contactusService service;
        private readonly CommonService commonService;

        public contactusController(contactusService service, WebHelper webHelper, WebHelper.SessionHelper sessionHelper, CommonService commonService) :
            base(webHelper, sessionHelper)
        {
            this.service = service;
            this.commonService = commonService;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> List([FromQuery]int pageNo = 0)
        {
            int offset = (WebHelper.DefaultGridPageSize * (pageNo));
            int limit = (offset + WebHelper.DefaultGridPageSize);
            var data = await service.GetAll().Skip(offset).Take(limit).ToListAsync();
            var count = await service.GetAll().CountAsync();
            var list = ToListingResponse(data, pageNo, count);
            return NeoData(list);
        }

        [HttpGet]
        [Route("search")]
        public async Task<IActionResult> Search([FromQuery]string q)
        {
            if (String.IsNullOrWhiteSpace(q))
            {
                return await List(0);
            }

            var data = await service.GetAll().Where(p => p.Name.Contains(q)).ToListAsync();
            var list = ToListingResponse(data, 0, 10);
            return NeoData(list);
        }

        [AllowAnonymous]
        [Route("")]
        [HttpPost]
        public async Task<IActionResult> Add([FromBody]contactus obj)
        {
            var success = await service.Insert(obj);
            var resp = new NeoApiResponse();
            if (success)
            {
                try
                {
                    var toEmail = commonService.GetSettingValueFromDb("EnquiryReceiverMail");
                    var emailSend = Utility.SendEmailViaResend(toEmail, "Enquiry From ContactUs Page", obj.Message);
                }
                catch (Exception)
                {

                }
                resp = resp.SuccessResponse(null, "Thanks to contact us, we will get back to you soon.", isSuccess: true);
            }
            else
            {
                resp = resp.ErrorResponse("Contactus create failed.");
            }

            return NeoData(resp);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Edit(Int32 id)
        {
            var obj = await service.Get(id);
            return NeoData(obj);
        }

        [HttpPut]
        [Route("")]
        public async Task<IActionResult> Edit(contactus obj)
        {
            var success = await service.Update(obj);
            var resp = new NeoApiResponse();
            if (success)
            {
                resp = resp.SuccessResponse(null, "Contactus has been updated successfully.");
            }
            else
            {
                resp = resp.ErrorResponse("Contactus update failed.");
            }

            return NeoData(resp);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(Int32 id)
        {
            var obj = await service.Get(id);
            var sucess = await service.Delete(obj);
            var resp = new NeoApiResponse();
            if (sucess)
            {
                resp = resp.SuccessResponse(null, "Contactus deleted Successfully.");
            }
            else
            {
                resp = resp.ErrorResponse("Contactus delete Failed.");
            }

            return NeoData(resp);
        }
   }
}