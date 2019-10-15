using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web.WebPages.Html;
using DashBoardAgil.CrossCutting;
using DashBoardAgil.DashBoardApi.Models;
using DashBoardAgil.Services;
using DashBoardAgil.Domain.Models;
using DashBoardAgil.InfraData.Contexto;
using Microsoft.Ajax.Utilities;
using Redmine.Net.Api;
using Redmine.Net.Api.Types;
using WebGrease.Css.Extensions;

namespace DashBoardAgil.DashBoardApi.Controllers.Api
{
    public class EntregasController : BaseApiController
    {
       
        public object Get(string time, DateTime dataIni, DateTime dataFim)
        {
            if (time.IsNullOrWhiteSpace())
                return new { status = false, mensagem = "Time é Obrigatório",  };

            var _parametrosBase = new NameValueCollection
                             {
                               // {RedmineKeys.PROJECT_ID, "compatibilizacao-do-compras-net"},
                                { RedmineKeys.PROJECT_ID, time}
                                //{RedmineKeys.SPENT_ON, String.Format("><{0}|{1}", dataIni.ToString("yyyy-MM-dd"), dataFim.ToString("yyy-MM-dd")) }
                                //, {RedmineKeys.SORT, "spent_on:desc" }
                                //,{"criteria", "user" }
                                // ,{"group_by", "hours"}
                              };

            var parametroIssue = new NameValueCollection(_parametrosBase)
                                     {
                                       {RedmineKeys.STATUS_ID, "*" },
                                       {RedmineKeys.UPDATED_ON, String.Format("><{0}|{1}", dataIni.ToString("yyyy-MM-dd"), dataFim.ToString("yyy-MM-dd")) }
                                     };


            var projeto = GlobalConfig.Instance.ConfiguracaoRedmine.AllowList.FirstOrDefault(x => x.Identifier == time)??null;
            
            if (projeto?.QuantidadeDesenvolvedores <= 0)
            {
                return new { status = false, mensagem = "Cadastre quantidade de Dev do time"};
            }

            var retorno = _RedmineService.GetEntregasPeriodo(parametroIssue, projeto,  dataIni, dataFim);

            return retorno;
        }
    }
}
