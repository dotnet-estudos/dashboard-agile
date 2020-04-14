using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DashBoardAgil.CrossCutting;
using DashBoardAgil.Domain.Models;
using Redmine.Net.Api;
using Microsoft.Ajax.Utilities;
using Redmine.Net.Api.Types;
using WebGrease.Css.Extensions;
using DashBoardAgil.Services;

namespace DashBoardAgil.DashBoardApi.Controllers.Api
{
    public class TimesEntryController : BaseApiController
    {
        public object Get(string project, string time, DateTime dataIni, DateTime dataFim)
        {
            if (project.IsNullOrWhiteSpace())
                return new { status = false, mensagem = "Time/Projeto é Obrigatório", SlaControl = new SlaControl() };

            var proj = GlobalConfig.Instance.ConfiguracaoRedmine.AllowList.FirstOrDefault(x => x.Identifier.Equals(time) || x.ProjetoId.ToString().Equals(time));

            var _parametros = new NameValueCollection
                             {
                               // {RedmineKeys.PROJECT_ID, proj.ide}
              
                                {RedmineKeys.SPENT_ON, String.Format("><{0}|{1}", dataIni.ToString("yyyy-MM-dd"), dataFim.ToString("yyy-MM-dd")) }
                                , {RedmineKeys.SORT, "spent_on:desc" }
                                ,{"criteria", "user" }
                                 ,{"group_by", "hours"}
                              };


            var ConsumoHoras = _RedmineService.ConsumptionHoursByProject(_parametros, project, proj.Identifier);

            return new {status = true, ConsumoHoras = ConsumoHoras, filtro = new { project, time, dataIni, dataFim } };
        }

    
    }
}
