using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Ajax.Utilities;
using Redmine.Net.Api;
using DashBoardAgil.CrossCutting;
using DashBoardAgil.DashBoardApi.Models;
using DashBoardAgil.Services;

namespace DashBoardAgil.DashBoardApi.Controllers.Api
{
    public class ComparativeController : BaseApiController
    {
        public object Get(string time )
        {

            #region processo
            var proj =
               GlobalConfig.Instance.ConfiguracaoRedmine.AllowList.FirstOrDefault(x => x.ProjetoId.ToString().Equals(time));

            var processo = _RedmineService.GetProcessoTeam(proj);
            var lststr = processo.ProcessoTipo.Select(x => x.TipoChamado.IdExterno).ToList();
            #endregion

            var parm = new NameValueCollection
                {
                    {RedmineKeys.TRACKER_ID, Global.Instance.ConcatenarLista("|", lststr)},
                    {RedmineKeys.STATUS_ID, "open"}
                };


            if (!time.IsNullOrWhiteSpace())
            {
                parm.Add(RedmineKeys.PROJECT_ID, String.Format("{0}", proj.Identifier));
                //verificar os custons fields dos times
                proj.CustonFields.ForEach(custon => {
                    parm.Add(String.Format("{0}", custon.Campo), String.Format("{0}", custon.Valor));
                });
                //Fim verificar os custons fields dos times
            }

            var ret = _RedmineService.GetComparativeObject(parm);

            return ret;

        }


    }
}
