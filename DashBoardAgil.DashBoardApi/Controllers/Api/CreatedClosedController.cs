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

namespace DashBoardAgil.DashBoardApi.Controllers.Api
{
    public class CreatedClosedController : BaseApiController
    {
        public Object Get(string time, DateTime dataIni, DateTime dataFim, TipoChamadoRedmine? tipoChamado, PrioridadeRedmine? prioridade )
        {
            var parm = new NameValueCollection(_defaultTipoChamados)
            {
                _defaultPrioridade,
            };

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
