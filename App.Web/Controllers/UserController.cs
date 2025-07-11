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

        [HttpPost]
        [AllowAnonymous]
        [Route("GenerateOTP")]
        public async Task<IActionResult> GenerateOtpAsync([FromBody] User obj)
        {
            var resp = new NeoApiResponse();

            try
            {
                var data = await service.GetAll()
                    .Where(e => e.Email == obj.Email)
                    .FirstOrDefaultAsync();

                if (data?.IsArchived == true)
                {
                    resp = resp.SuccessResponse(null, "Entered Email is already exist in our system.", isSuccess: false);
                    return NeoData(resp);
                }

                var result = Util.Util.GenerateOTPAndSendMail(obj.Email);

                if (!result.isSucess)
                {
                    resp = resp.ErrorResponse("Something went wrong");
                    return NeoData(resp);
                }

                obj.Otp = result.otp;
                obj.IsArchived = false;
                var isDataSavedInDB = false;
                if (data?.IsArchived == false)
                {
                    data.Otp = result.otp;
                    isDataSavedInDB = await UpdateUserAsync(data);
                }else
                {
                    isDataSavedInDB = await CreateInactiveUser(obj);
                }

                resp = isDataSavedInDB
                    ? resp.SuccessResponse(null, "OTP has been sent to your email address.")
                    : resp.ErrorResponse("Failed to save OTP in database.");
            }
            catch (Exception)
            {
                resp = resp.ErrorResponse("Server error occurred while sending OTP.");
            }

            return NeoData(resp);
        }

        [HttpPut]
        [Route("ValidateOTP")]
        [AllowAnonymous]
        public async Task<IActionResult> ValidateOTP(User obj)
        {
            var data = await service.GetAll().Where(e=>e.Email == obj.Email).FirstOrDefaultAsync();
            var resp = new NeoApiResponse();
            if (data?.Otp == obj.Otp && data != null)
            {
                data.IsArchived = true;
                data.Password = obj.Password;
                var isSuccess = await UpdateUserAsync(data);
                resp = isSuccess
                    ? resp.SuccessResponse(null, "Registration completed successfully.")
                    : resp.ErrorResponse("Registration got failed, Please connect to admin.");
            }
            else
            {
                resp = resp.SuccessResponse(null, "OTP is not correct", isSuccess: false);
            }
            return NeoData(resp);
        }

        private async Task<bool> UpdateUserAsync(User obj)
        {
            var resp = false;
            try
            {
                // Update audit fields
                obj.ModifiedOn = DateTime.Now;
                obj.ModifiedBy = User.UserId();

                // Encrypt password
                obj.Password = NeoAuthorization.cryptographyHelper.Encrypt(
                    obj.Password,
                    NeoContext.PassPhrase(NeoAuthorization)
                );

                // Update in DB
                resp = await service.Update(obj);
                    //? resp.SuccessResponse(null, "User has been updated successfully.")
                    //: resp.ErrorResponse("User update failed.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Update failed: " + ex.Message);
                //resp = resp.ErrorResponse("An unexpected error occurred.");
            }

            return resp;
        }
        // $45 Add user logic is already written in a private method "CreateInactiveUser". use that method.
        // Now that method is being used to create user from public website and below method is being used from admin panel.
        // most of the logic is same.
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


        private async Task<bool> CreateInactiveUser(User obj)
        {
            var isSuccess = false;

            try
            {
                // Generate code prefix
                var prefix = "CUN-" + DateTime.Now.ToString("yyMMdd") + "-";
                var data = await service.GetAll()
                                        .OrderByDescending(y => y.UserId)
                                        .FirstOrDefaultAsync();

                obj.Code = data != null ? prefix + data.UserId : prefix + "1";

                // Set audit fields
                var userId = User.UserId();
                var now = DateTime.Now;

                obj.CreatedBy = userId;
                obj.CreatedOn = now;
                obj.ModifiedBy = userId;
                obj.ModifiedOn = now;

                // Encrypt password
                obj.Password = NeoAuthorization.cryptographyHelper.Encrypt(
                    obj.Password,
                    NeoContext.PassPhrase(NeoAuthorization)
                );

                // Insert the user
                isSuccess = await service.Insert(obj);
            }
            catch (Exception ex)
            {
                isSuccess = false;
            }

            return isSuccess;
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
            var resp = new NeoApiResponse();
            var isSuccess = await UpdateUserAsync(obj);
            resp = isSuccess
            ? resp.SuccessResponse(null, "User has been updated successfully.")
            : resp.ErrorResponse("User update failed.");
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