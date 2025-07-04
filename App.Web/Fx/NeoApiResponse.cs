using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace App.Web.Fx
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class NeoApiResponse
    {
        public int Status { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
        public object Errors { get; set; }

        public NeoApiResponse SuccessResponse(object data, string message = "Success", int status = (int)HttpStatusCode.OK, bool isSuccess = true)
        {
            this.Data = data;
            this.IsSuccess = isSuccess;
            this.Message = message;
            this.Errors = null;
            this.Status = status;
            return this;
        }

        public NeoApiResponse ErrorResponse(string message, int status = (int)HttpStatusCode.InternalServerError, object errors = null, Exception ex = null)
        {
            this.Data = null;
            this.IsSuccess = false;
            this.Status = status;
            this.Message = !String.IsNullOrWhiteSpace(message) ? message : null;
            if (String.IsNullOrWhiteSpace(this.Message))
            {
                this.Message = ex?.Message;
            }
            this.Errors = (errors != null) ? errors : null;
            if (this.Errors == null && ex != null)
            {
                this.Errors = new { StackTrace = ex.StackTrace };
            }
            return this;
        }
    }
}