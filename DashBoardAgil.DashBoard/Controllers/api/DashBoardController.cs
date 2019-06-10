using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace DashBoardAgil.DashBoard.Controllers.api
{
    public class DashBoardController : ApiController
    {
        // GET: api/DashBoard
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/DashBoard/5
        public JsonResult Get(int id)
        {
            var result = new JsonResult();
            result.Data = new {sucess = true};
            return result;
        }

        // POST: api/DashBoard
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/DashBoard/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/DashBoard/5
        public void Delete(int id)
        {
        }
    }
}
