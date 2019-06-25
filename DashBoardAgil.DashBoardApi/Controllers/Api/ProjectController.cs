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



        // GET: api/Project
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Project/5
        public List<Project> Get(string id, int parent = 0, int customField = 0)
        {
            var param = new NameValueCollection();

            param.Add(RedmineKeys.ID, id);

            var retorno = _Redmine.GetObjects<Project>(param);



            //if (parent)
            //{

            //    var ret = from proj in retorno.ToList()
            //              where
            //                  proj.Parent != null &&
            //                  GlobalConfig.Instance.ConfiguracaoRedmine.AllowList.Select(x => x.Id)
            //                      .ToList()
            //                      .Contains(proj.Parent.Id.ToString())
            //              group proj by proj.Parent
            //        into grp
            //              select
            //                  new
            //                  {
            //                      grp.Key.Id,
            //                      grp.Key.Name,
            //                      projetos =
            //                          grp.Select(x => new { id = x.Id, name = x.Name, identifier = x.Identifier }).ToList()
            //                  };
            //}



            return retorno.Where(x => ((parent == 0      || x.Parent != null && x.Parent.Id == parent)) &&
                                       (customField == 0 || x.CustomFields != null && x.CustomFields.Any(y=>y.Id == customField) )
                                        ).ToList();
                                        
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
