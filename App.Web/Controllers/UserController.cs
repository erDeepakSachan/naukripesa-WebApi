using App.Entity;
using App.Service;
using App.Web.Fx;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Writers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App.Web.Controllers
{
    public class UserController : NeoController
    {
        private readonly UserService service;

        public UserController(UserService service, WebHelper webHelper, WebHelper.SessionHelper sessionHelper) :
            base(webHelper, sessionHelper)
        {
            this.service = service;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> List([FromQuery] int pageNo = 0)
        {
            int offset = WebHelper.DefaultGridPageSize * pageNo;
            int limit = WebHelper.DefaultGridPageSize;
            var data = await service.GetAll()
                .Include(p => p.Company)
                .Include(p => p.UserType)
                .Include(p => p.UserGroup)
                .Skip(offset).Take(limit).ToListAsync();
            var count = await service.GetAll().CountAsync();
            var list = ToListingResponse(data, pageNo, count);
            return NeoData(list);
        }

        [HttpGet]
        [Route("search")]
        public async Task<IActionResult> Search([FromQuery] string q)
        {
            if (String.IsNullOrWhiteSpace(q))
            {
                return await List(0);
            }

            var data = await service.GetAll()
                .Include(p => p.Company)
                .Include(p => p.UserType)
                .Include(p => p.UserGroup)
                .Where(p => p.Name.Contains(q)).ToListAsync();
            var list = ToListingResponse(data, 0, 10);
            return NeoData(list);
        }

        [Route("")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Add([FromBody]User obj)
        {
            var data = await service.GetAll().OrderByDescending(y => y.UserId).FirstOrDefaultAsync();
            var prefix = "CUN-" + DateTime.Now.ToString("yyMMdd") + "-";
            if (data != null)
            {
                obj.Code = prefix + data.UserId;
            }
            else
            {
                obj.Code = prefix + 1;
            }
            obj.CreatedBy = User.UserId();
            obj.CreatedOn = DateTime.Now;
            obj.ModifiedOn = obj.CreatedOn;
            obj.ModifiedBy = User.UserId();
            obj.Password = NeoAuthorization.cryptographyHelper.Encrypt(obj.Password,
                            NeoContext.PassPhrase(NeoAuthorization));
            var success = await service.Insert(obj);
            var resp = new NeoApiResponse();
            if (success)
            {
                resp = resp.SuccessResponse(null, "User has been created successfully.");
            }
            else
            {
                resp = resp.ErrorResponse("User create failed.");
            }

            return NeoData(resp);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Edit(Int32 id)
        {
            var obj = await service.Get(id);
            obj.Password = NeoAuthorization.cryptographyHelper.Decrypt(obj.Password, NeoContext.PassPhrase(NeoAuthorization));
            return NeoData(obj);
        }

        [HttpPut]
        [Route("")]
        public async Task<IActionResult> Edit(User obj)
        {
            obj.ModifiedOn = DateTime.Now;
            obj.ModifiedBy = User.UserId();
            obj.Password = NeoAuthorization.cryptographyHelper.Encrypt(obj.Password,
                            NeoContext.PassPhrase(NeoAuthorization));
            var success = await service.Update(obj);
            var resp = new NeoApiResponse();
            if (success)
            {
                resp = resp.SuccessResponse(null, "User has been updated successfully.");
            }
            else
            {
                resp = resp.ErrorResponse("User update failed.");
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
                resp = resp.SuccessResponse(null, "User deleted Successfully.");
            }
            else
            {
                resp = resp.ErrorResponse("User delete Failed.");
            }

            return NeoData(resp);
        }
   }
}