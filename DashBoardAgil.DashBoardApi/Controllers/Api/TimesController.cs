using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DashBoardAgil.CrossCutting;
using DashBoardAgil.Domain.Models;
using  DashBoardAgil.Services;

namespace DashBoardAgil.DashBoardApi.Controllers.Api
{
    public class TimesController : ApiController
    {
        // GET: api/Times
        public IEnumerable<Projeto> Get()
        {
            return GlobalConfig.Instance.ConfiguracaoRedmine.AllowList;
        }

        // GET: api/Times/5
        public Projeto Get(string id)
        {
            return GlobalConfig.Instance.ConfiguracaoRedmine.AllowList.FirstOrDefault(x => x.Equals(id));
        }

        // POST: api/Times
        public void Post(Projeto projeco)
        {
            GlobalConfig.Instance.ConfiguracaoRedmine.AllowList.Add(projeco);
        }

        // PUT: api/Times/5
        public void Put(string id, Projeto projeto)
        {
           GlobalConfig.Instance.ConfiguracaoRedmine.AllowList.ForEach(x =>
            {
                if(x.Equals(id))
                    Global.ReflectObjec(projeto, x);
            });

        }

        // DELETE: api/Times/5
        public void Delete(string id)
        {
            var delete = GlobalConfig.Instance.ConfiguracaoRedmine.AllowList.FirstOrDefault(x => x.Equals(id));

            GlobalConfig.Instance.ConfiguracaoRedmine.AllowList.Remove(delete);
        }
    }
}
