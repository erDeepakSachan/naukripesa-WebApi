using App.Web.Fx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Entity;
using App.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.Web.Controllers
{
    public class UserSessionController : NeoController
    {
        private readonly UserSessionService service;

        public UserSessionController(UserSessionService service, WebHelper webHelper, WebHelper.SessionHelper sessionHelper) :
            base(webHelper, sessionHelper)
        {
            this.service = service;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> List([FromQuery]int pageNo = 0)
        {
            int offset = WebHelper.DefaultGridPageSize * pageNo;
            int limit = WebHelper.DefaultGridPageSize;
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

            var data = await service.GetAll().Include(p=>p.User).Where(p => p.User.Name.Contains(q)).ToListAsync();
            var list = ToListingResponse(data, 0, 10);
            return NeoData(list);
        }

        [Route("")]
        [HttpPost]
        public async Task<IActionResult> Add([FromBody]UserSession obj)
        {
            var success = await service.Insert(obj);
            var resp = new NeoApiResponse();
            if (success)
            {
                resp = resp.SuccessResponse(null, "User session has been created successfully.");
            }
            else
            {
                resp = resp.ErrorResponse("User session create failed.");
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
        public async Task<IActionResult> Edit(UserSession obj)
        {
            var success = await service.Update(obj);
            var resp = new NeoApiResponse();
            if (success)
            {
                resp = resp.SuccessResponse(null, "User session has been updated successfully.");
            }
            else
            {
                resp = resp.ErrorResponse("User session update failed.");
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
                resp = resp.SuccessResponse(null, "User session deleted Successfully.");
            }
            else
            {
                resp = resp.ErrorResponse("User session delete Failed.");
            }

            return NeoData(resp);
        }
   }
}