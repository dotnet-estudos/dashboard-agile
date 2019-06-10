using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http;
using Microsoft.Ajax.Utilities;

namespace DashBoardAgil.DashBoardApi.Controllers.Api
{
    public class CookieController : ApiController
    {

        [HttpPost]
        public HttpResponseMessage Post([FromBody]string name)
        {
            HttpResponseMessage respMessage = new HttpResponseMessage();


            if (!name.IsNullOrWhiteSpace())
            {
                respMessage.Content = new ObjectContent<Object>(new { name = name }, new JsonMediaTypeFormatter());

                CookieHeaderValue cookie = new CookieHeaderValue(name, "");

                cookie.Expires = DateTimeOffset.Now.AddDays(1);
                cookie.Domain = Request.RequestUri.Host;
                cookie.Path = "/";

                respMessage.Headers.AddCookies(new CookieHeaderValue[] { cookie });
            }
            else
            {
                respMessage.Content = new ObjectContent<Object>(new { name = "NotFound" }, new JsonMediaTypeFormatter());
            }

            return respMessage;
        }
    }
}
