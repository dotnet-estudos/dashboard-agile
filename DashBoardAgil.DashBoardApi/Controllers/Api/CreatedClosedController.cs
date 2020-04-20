using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DashBoardAgil.CrossCutting;
using DashBoardAgil.DashBoardApi.Models;
using DashBoardAgil.Domain.Models;
using DashBoardAgil.CrossCutting.Enums;
using DashBoardAgil.Services;
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

            #region processo
            var proj =
               GlobalConfig.Instance.ConfiguracaoRedmine.AllowList.FirstOrDefault(x => x.ProjetoId.Equals(time));

            var processo = _RedmineService.GetProcessoTeam(proj);
            var lststr = processo.ProcessoTipo.Select(x => x.TipoChamado.IdExterno).ToList();
            #endregion

            if (tipoChamado == null && prioridade == null)
            {

                parm = new NameValueCollection
                {
                    {RedmineKeys.TRACKER_ID, Global.Instance.ConcatenarLista("|", lststr)},
                    _defaultPrioridade
                };

                /*parm = new NameValueCollection(_defaultTipoChamados)
                {
                    _defaultPrioridade,
                };*/
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

            //if (!time?.IsNullOrWhiteSpace()?? false)
            if (proj != null)
            {
                //if (time != null)
                parm.Add(RedmineKeys.PROJECT_ID, proj.Identifier);
                //verificar os custons fields dos times
                proj.CustonFields.ForEach(custon => {
                    parm.Add(String.Format("{0}", custon.Campo), String.Format("{0}", custon.Valor));
                });
                //Fim verificar os custons fields dos times
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
