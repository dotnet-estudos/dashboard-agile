using System;
using System.Collections.Generic;
using DashBoardAgil.Services;
using DashBoardAgil.CrossCutting;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Ajax.Utilities;
using Redmine.Net.Api;

namespace DashBoardAgil.DashBoardApi.Controllers.Api
{
    public class ThroughputController : BaseApiController
    {
        public Object Get(string time, DateTime dataIni, DateTime dataFim)
        {

            #region processo
            var proj =
               GlobalConfig.Instance.ConfiguracaoRedmine.AllowList.FirstOrDefault(x => x.Identifier.Equals(time) || x.Id.Equals(time));

            var processo = _RedmineService.GetProcessoTeam(proj);
            var lststr = processo.ProcessoTipo.Select(x => x.TipoChamado.IdExterno).ToList();
            #endregion

            var parm = new NameValueCollection
                {
                    {RedmineKeys.TRACKER_ID, Global.Instance.ConcatenarLista("|", lststr)},
                    {RedmineKeys.STATUS_ID, "*"},
                    {
                        RedmineKeys.CREATED_ON,
                        String.Format("><{0}|{1}", dataIni.ToString("yyyy-MM-dd"), dataFim.ToString("yyyy-MM-dd"))
                    }
                };

            if (!time.IsNullOrWhiteSpace())
            {
                parm.Add(RedmineKeys.PROJECT_ID, String.Format("{0}", time));
            }
            //verificar os custons fields dos time
            proj.CustonFields.ForEach(custon => {
                parm.Add(String.Format("{0}", custon.Campo), String.Format("{0}", custon.Valor));
            });
            //Fim verificar os custons fields dos time
            var ret = _RedmineService.GetThroughputRate(parm);

            return ret;

        }
    }
}
