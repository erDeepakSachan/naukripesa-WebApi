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
    public class UserTypeController : NeoController
    {
        private readonly UserTypeService service;

        public UserTypeController(UserTypeService service, WebHelper webHelper, WebHelper.SessionHelper sessionHelper) :
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

            var data = await service.GetAll().Where(p => p.Name.Contains(q)).ToListAsync();
            var list = ToListingResponse(data, 0, 10);
            return NeoData(list);
        }

        [Route("")]
        [HttpPost]
        public async Task<IActionResult> Add([FromBody]UserType obj)
        {
            var success = await service.Insert(obj);
            var resp = new NeoApiResponse();
            if (success)
            {
                resp = resp.SuccessResponse(null, "User type has been created successfully.");
            }
            else
            {
                resp = resp.ErrorResponse("User type create failed.");
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
        public async Task<IActionResult> Edit(UserType obj)
        {
            var success = await service.Update(obj);
            var resp = new NeoApiResponse();
            if (success)
            {
                resp = resp.SuccessResponse(null, "User type has been updated successfully.");
            }
            else
            {
                resp = resp.ErrorResponse("User type update failed.");
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
                resp = resp.SuccessResponse(null, "User type deleted Successfully.");
            }
            else
            {
                resp = resp.ErrorResponse("User type delete Failed.");
            }

            return NeoData(resp);
        }
   }
}