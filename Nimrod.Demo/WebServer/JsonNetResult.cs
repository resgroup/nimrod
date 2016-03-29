using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Nimrod.Demo
{
    public class JsonNetResult : JsonNetResult<object>
    {
        public JsonNetResult(object data) : base(data)
        { }

        public JsonNetResult(object data, JsonRequestBehavior behavior, string contentType, Encoding contentEncoding)
            : base(data, behavior, contentType, contentEncoding)
        { }

    }

    /// <summary>
    /// Using the Newtonsoft.Json library (Json.Net) to return Json by the server
    /// This permit to avoid circular references and we have more features
    /// http://stackoverflow.com/questions/23348262/using-json-net-to-return-actionresult
    /// </summary>
    public class JsonNetResult<T> : JsonResult
    {
        public JsonNetResult(T data)
            : this(data, JsonRequestBehavior.AllowGet, null, null)
        {
        }

        public JsonNetResult(T data, JsonRequestBehavior behavior, string contentType, Encoding contentEncoding)
        {
            this.Data = data;
            this.JsonRequestBehavior = behavior;
            this.ContentType = contentType;
            this.ContentEncoding = ContentEncoding;
        }
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (this.JsonRequestBehavior == JsonRequestBehavior.DenyGet
                && string.Equals(context.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("JSON GET is not allowed");
            }

            HttpResponseBase response = context.HttpContext.Response;
            response.ContentType = string.IsNullOrEmpty(this.ContentType) ? "application/json" : this.ContentType;

            if (this.ContentEncoding != null)
            {
                response.ContentEncoding = this.ContentEncoding;
            }
            if (this.Data != null)
            {
                var json = this.Data.ToJson();

                if (this.Data is Exception)
                {
                    response.StatusCode = 500;
                }

                response.Write(json);
            }
        }


        public static JsonNetResult<T2> Create<T2>(T2 data)
        {
            return new JsonNetResult<T2>(data);
        }
    }
}
