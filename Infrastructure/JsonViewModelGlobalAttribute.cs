using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AngularExample.Infrastructure
{
    public class JsonViewModelGlobalAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (typeof(RedirectToRouteResult).IsInstanceOfType(filterContext.Result))
                return;

            var acceptTypes = filterContext.HttpContext.Request.AcceptTypes ?? new[] { "text/html" };

            var model = filterContext.Controller.ViewData.Model;

            if (acceptTypes.Any(x=>x.Contains("html")))
            {
                string json = JsonConvert.SerializeObject(model, Formatting.Indented, new JsonSerializerSettings { Converters = new List<JsonConverter>() { new IsoDateTimeConverter() }, ContractResolver = new EntityFrameworkContractResolver(), ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

                filterContext.Controller.ViewData["JsonModel"] = json;
            }
        }
    }
}