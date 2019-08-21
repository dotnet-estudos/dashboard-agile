using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Ajax.Utilities;
using Redmine.Net.Api;

namespace DashBoardAgil.DashBoardApi.Controllers.Api
{
    public class CreatedClosedController : BaseApiController
    {
        public Object Get(string time, DateTime DataIni, DateTime DataFim)
        {
            var parm = new NameValueCollection(_defaultTipoChamados)
            {
                _defaultPrioridade,
            };

            if (!time.IsNullOrWhiteSpace())
            {
                parm.Add(RedmineKeys.PROJECT_ID, time);
            }

            var retorno = _RedmineService.GetCreatedClosed(parm, DataIni, DataFim);

            return retorno;
        }

    }
}
