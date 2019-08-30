using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DashBoardAgil.DashBoardApi.Models;
using DashBoardAgil.Services.Domain;
using DashBoardAgil.Services.Enums;
using Microsoft.Ajax.Utilities;
using Redmine.Net.Api;
using WebApi.OutputCache.V2;

namespace DashBoardAgil.DashBoardApi.Controllers.Api
{
    [CacheOutput(ServerTimeSpan = 1 * (60))]
    public class CreatedClosedController : BaseApiController
    {
        public Object Get(string time, DateTime dataIni, DateTime dataFim, TipoChamadoRedmine? tipoChamado, PrioridadeRedmine? prioridade )
        {
            var parm = new NameValueCollection();

            if (tipoChamado == null && prioridade == null)
            {

                parm = new NameValueCollection(_defaultTipoChamados)
                {
                    _defaultPrioridade,
                };
            }
            else
            {
                if (tipoChamado != null)
                {
                    parm.Add(RedmineKeys.TRACKER_ID, tipoChamado.GetHashCode().ToString());
                }
                if (prioridade != null)
                {
                    parm.Add(RedmineKeys.PRIORITY_ID, prioridade.GetHashCode().ToString());
                }
            }

            

            if (!time?.IsNullOrWhiteSpace()?? false)
            {
                if (time != null) parm.Add(RedmineKeys.PROJECT_ID, time);
            }

            if (dataIni == null || dataFim == null)
            {
                return new CreatedClosedControl(null, null);
            }

            var retorno = _RedmineService.GetCreatedClosed(parm, (DateTime)dataIni, (DateTime)dataFim);

            return retorno;
        }

    }
}
