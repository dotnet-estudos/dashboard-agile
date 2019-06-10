using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using DashBoardAgil.Aplication;

namespace DashBoardAgil.DashBoardApi.Controllers.Api
{
    public class AgenteController : ApiController
    {
        // GET: api/Agente
        public object Get()
        {
            var data = new {status = Auto.Instance.StatusAgent()};
            return data;
        }
         
        // POST: api/Agente
        public void Post([FromBody]string value)
        {
        }

        /// <summary>
        /// toggle Agent
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        // PUT: api/Agente/5
        [System.Web.Http.HttpPut]
        public object Put(int id)
        {
            if(id == 99)
                Auto.Instance.ToggleServico();
           
            var data = new { status = Auto.Instance.StatusAgent() };
            return data;
        }

        // DELETE: api/Agente/5
        public void Delete(int id)
        {
        }
    }
}
