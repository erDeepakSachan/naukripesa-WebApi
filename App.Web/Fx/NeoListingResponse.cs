using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace App.Web.Fx
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class NeoListingResponse<T> where T : new()
    {
        public NeoListingResponse()
        {
            Data = new List<T>();
        }

        public int PageSize { get; set; }
        public double TotalPageCount { get; set; }
        public int TotalItemCount { get; set; }
        public int CurrentPageNo { get; set; }
        public List<T> Data { get; set; }
    }
}