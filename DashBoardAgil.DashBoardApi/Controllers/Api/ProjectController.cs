using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using Redmine.Net.Api;
using Redmine.Net.Api.Types;

namespace DashBoardAgil.DashBoardApi.Controllers.Api
{
    public class ProjectController : BaseApiController
    {


        
        // GET: api/Project
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Project/5
        public IEnumerable<Project> Get(string id)
        {
            var param = new NameValueCollection
            {
                //{ RedmineKeys.PROJECT_ID, id },
                //{ RedmineKeys.INCLUDE, RedmineKeys.SUBJECT }
            };
            var retorno = _Redmine.GetObjects<Project>(param);
           
            return retorno;
        }

        // POST: api/Project
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Project/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Project/5
        public void Delete(int id)
        {
        }
    }
}
