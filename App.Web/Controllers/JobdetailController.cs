using App.Web.Fx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Entity;
using App.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace App.Web.Controllers
{
    public class JobdetailController : NeoController
    {
        private readonly JobdetailService service;

        public JobdetailController(JobdetailService service, WebHelper webHelper, WebHelper.SessionHelper sessionHelper) :
            base(webHelper, sessionHelper)
        {
            this.service = service;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> List([FromQuery] int pageNo = 0, [FromQuery] int pageSize = 0, [FromQuery] int cityId = 0)
        {
            try
            {
                var loggedInUser = GetLoggedInUser();
                int offset = WebHelper.DefaultGridPageSize * pageNo;
                int limit = (pageSize == 0) ? offset + WebHelper.DefaultGridPageSize : pageSize;

                IQueryable<Jobdetail> query = service.GetAll().Include(p => p.JobLocation);

                if (cityId != 0)
                {
                    query = query.Where(j => j.JobLocationId == cityId);
                }
                else if (loggedInUser != null && loggedInUser.GroupID != 1)
                {
                    query = query.Where(x => x.CreatedBy == loggedInUser.UserID);
                }

                var data = await query
                    .OrderByDescending(p => p.InterviewDate)
                    .Skip(offset)
                    .Take(limit)
                    .ToListAsync();

                var count = await service.GetAll().CountAsync(); // Optional: consider optimizing this

                var list = ToListingResponse(data, pageNo, count);
                return NeoData(list);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpGet]
        [Route("search")]
        public async Task<IActionResult> Search([FromQuery] string q)
        {
            if (String.IsNullOrWhiteSpace(q))
            {
                return await List(0);
            }

            var data = await service.GetAll().Include(p => p.JobLocation).Where(p => p.JobLocation.Location.Contains(q)).ToListAsync();
            var list = ToListingResponse(data, 0, 10);
            return NeoData(list);
        }

        [Route("")]
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Jobdetail obj)
        {
            var loggedInUser = GetLoggedInUser();
            obj.CreatedBy = loggedInUser.UserID;
            obj.CreatedOn = DateTime.Now;
            var success = await service.Insert(obj);
            var resp = new NeoApiResponse();
            if (success)
            {
                resp = resp.SuccessResponse(null, "Jobdetail has been created successfully.");
            }
            else
            {
                resp = resp.ErrorResponse("Jobdetail create failed.");
            }

            return NeoData(resp);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Edit(Int32 id)
        {
            var obj = await service.GetAll()
                .Include(p => p.JobLocation).FirstOrDefaultAsync(p => p.JobDetailId == id); ;
            return NeoData(obj);
        }

        [HttpPut]
        [Route("")]
        public async Task<IActionResult> Edit(Jobdetail obj)
        {
            var loggedInUser = GetLoggedInUser();
            obj.ModifiedBy = loggedInUser.UserID;
            obj.ModifiedOn = DateTime.Now;
            var success = await service.Update(obj);
            var resp = new NeoApiResponse();
            if (success)
            {
                resp = resp.SuccessResponse(null, "Jobdetail has been updated successfully.");
            }
            else
            {
                resp = resp.ErrorResponse("Jobdetail update failed.");
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
                resp = resp.SuccessResponse(null, "Jobdetail deleted Successfully.");
            }
            else
            {
                resp = resp.ErrorResponse("Jobdetail delete Failed.");
            }

            return NeoData(resp);
        }
    }
}