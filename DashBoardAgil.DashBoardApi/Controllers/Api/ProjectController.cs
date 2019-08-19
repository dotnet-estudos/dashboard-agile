using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Mvc;
using DashBoardAgil.Services;
using Redmine.Net.Api;
using Redmine.Net.Api.Types;

namespace DashBoardAgil.DashBoardApi.Controllers.Api
{
    public class ProjectController : BaseApiController
    {

        // GET: api/Project/5
        public object Get(string id, bool parent = false, int customField = 0)
        {
            var param = new NameValueCollection {{RedmineKeys.ID, "todos"}};


            var retorno = _Redmine.GetObjects<Project>(param);

            var Parent = GlobalConfig.Instance.ConfiguracaoRedmine.AllowList.FirstOrDefault(x => x.Identifier.Equals(id));

             var ret = retorno.Where(x => ((!parent || parent && (x.Parent?.Id.ToString() ?? "1") == (Parent?.Id ?? "0"))) &&
                                          (customField == 0 || x.CustomFields != null && x.CustomFields.Any(y => y.Id == customField && y.Values != null && y.Values.Any(z=>z.Info != "") ))
                                        ).Select(x => new { x.Id, x.Identifier, x.Name }).ToList();

            return ret;
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
