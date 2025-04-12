using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using Microsoft.AspNetCore.Mvc;

namespace App.Web.Fx
{
    public class JsonNetResult : ActionResult
    {
        public Encoding ContentEncoding { get; set; }
        public string ContentType { get; set; }
        public object Data { get; set; }
        public int StatusCode { get; set; }

        public JsonSerializerSettings SerializerSettings { get; set; }
        public Formatting Formatting { get; set; }

        public JsonNetResult()
        {
            ApplyDefaultSettings();
        }

        public JsonNetResult(NeoApiResponse data)
        {
            ApplyDefaultSettings();
            Data = data;
            StatusCode = data.Status;
        }

        public JsonNetResult(object data, int statusCode)
        {
            ApplyDefaultSettings(statusCode);
            Data = data;
        }

        public override void ExecuteResult(ActionContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            HttpResponse response = context.HttpContext.Response;
            response.StatusCode = StatusCode;
            response.ContentType = !string.IsNullOrEmpty(ContentType) ? ContentType : "application/json";

            if (Data != null)
            {
                using (var writer = new StreamWriter(response.Body))
                using (var jsonWriter = new JsonTextWriter(writer) { Formatting = Formatting })
                {
                    JsonSerializer serializer = JsonSerializer.Create(SerializerSettings);
                    serializer.Serialize(jsonWriter, Data);
                }
            }
        }

        private void ApplyDefaultSettings(int statusCode = 200)
        {
            StatusCode = statusCode;
            NeoContractResolver contractResolver = new NeoContractResolver
            {
                NamingStrategy = new DefaultNamingStrategy()
            };

            SerializerSettings = new JsonSerializerSettings()
            {
                ContractResolver = contractResolver,
                Formatting = Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            };
            SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.IsoDateTimeConverter());
        }
    }

    public class NeoContractResolver : DefaultContractResolver
    {
        private List<string> ignoredProperties = new List<string>();

        public NeoContractResolver()
        {
            ignoredProperties.Add("connectionstring");
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);
            if (ignoredProperties.Contains(property.PropertyName.ToLower()))
            {
                property.Ignored = true;
            }
            return property;
        }
    }
}