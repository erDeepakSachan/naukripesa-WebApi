using App.Entity;
using App.Service;
using App.Util;
using App.Web.Fx;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Web;

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

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> List([FromQuery] int pageNo = 0)
        {
            var loggedInUser = GetLoggedInUser();
            int offset = WebHelper.DefaultGridPageSize * pageNo;
            int limit = WebHelper.DefaultGridPageSize;
            DateTime now = DateTime.Now;
            DateTime fifteenDaysAgo = now.AddDays(-5);

            var query = service.GetAll()
                .Include(p => p.JobLocation)
                .AsEnumerable() // Switch to in-memory for custom ordering
                .Select(p => new
                {
                    Item = p,
                    OrderGroup = p.InterviewDate.HasValue && p.InterviewDate > now ? 0 :
                                 !p.InterviewDate.HasValue && p.CreatedOn >= fifteenDaysAgo ? 1 : 2
                })
                .OrderBy(x => x.OrderGroup)
                .ThenBy(x => x.OrderGroup == 1 ? -x.Item.CreatedOn?.Ticks : long.MaxValue) // Descending for group 1
                .ThenBy(x => x.Item.InterviewDate ?? DateTime.MaxValue) // Optional: secondary sort for group 0
                .Select(x => x.Item);

            if (loggedInUser != null && loggedInUser.GroupID != 1)
            {
                query = query.Where(x => x.CreatedBy == loggedInUser.UserID);
            }

            var data = query.Skip(offset).Take(limit).ToList();
            var count = query.ToList().Count;//await service.GetAll().CountAsync();

            var list = ToListingResponse(data, pageNo, count);
            return NeoData(list);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("PublicList")]
        public async Task<IActionResult> PublicList([FromQuery] int pageNo = 0, [FromQuery] int cityId = 0, bool isITJOb = false)
        {
            try
            {
                var pageSize = 50;
                int offset = WebHelper.DefaultGridPageSize * pageNo;
                int limit = (pageSize == 0) ? offset + WebHelper.DefaultGridPageSize : pageSize;

                DateTime today = DateTime.Today;
                DateTime fiveDaysAgo = today.AddDays(-5);

                IQueryable<Jobdetail> query = service.GetAll().Include(p => p.JobLocation);

                // Location filter
                if (cityId == -1)
                {
                    var excludedLocationIds = new[] { 1, 2, 3, 4 };
                    query = query.Where(j => !excludedLocationIds.Contains(j.JobLocationId));
                }
                else if (cityId != 0)
                {
                    query = query.Where(j => j.JobLocationId == cityId);
                }

                // IT Job filter
                if (isITJOb)
                {
                    query = query.Where(j => j.IsITJob == isITJOb);
                }

                // Fetch all filtered data first
                var allData = await query.ToListAsync();
                var ttt = DateTime.Now.ToIST();
                var newJobsList = allData.Where(j => j.InterviewDate.HasValue && j.InterviewDate.Value > DateTime.Now.ToIST())
                                         .OrderBy(j => j.InterviewDate)
                                         .ToList();
                var primejobsList = allData.Where(j => !j.InterviewDate.HasValue && (j.CreatedOn > fiveDaysAgo))
                                         .OrderByDescending(j => j.CreatedOn)
                                         .ToList();
                var priorityJobs = newJobsList.Concat(primejobsList).ToList();


                var priorityJobIds = priorityJobs.Select(j => j.JobDetailId).ToHashSet();
                var remainingJobs = allData.Where(j => !priorityJobIds.Contains(j.JobDetailId)).ToList().OrderBy(j => j.CreatedOn).ToList();

                var sortedData = priorityJobs.Concat(remainingJobs).Skip(offset).Take(limit).ToList();


                var list = new NeoListingResponse<Jobdetail>();
                list.Data = sortedData;

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

            var data = await service.GetAll().Include(p => p.JobLocation).Where(p => p.CompanyName.Contains(q)).ToListAsync();
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
            obj.InterviewDate = obj.InterviewDate?.Date.AddHours(16);
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
            obj.InterviewDate = obj.InterviewDate?.Date.AddHours(16);
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