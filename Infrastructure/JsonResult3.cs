using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AngularExample.Infrastructure
{
    public class JsonResult3 : JsonResult
    {
        public JsonResult3() { }
        public JsonResult3(object data) { this.Data = data; }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            HttpResponseBase response = context.HttpContext.Response;
            if (!string.IsNullOrEmpty(this.ContentType))
                response.ContentType = this.ContentType;
            else
                response.ContentType = "application/json";

            if (this.ContentEncoding != null)
                response.ContentEncoding = this.ContentEncoding;

            string json = JsonConvert.SerializeObject(this.Data, Formatting.Indented, new JsonSerializerSettings { Converters = new List<JsonConverter>() { new IsoDateTimeConverter() }, ContractResolver = new EntityFrameworkContractResolver(), ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            response.Write(json);
        }
    }
}
