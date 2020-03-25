using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Linq;
using System.Data.Entity;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DashBoardAgil.CrossCutting;
using DashBoardAgil.Services;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using Redmine.Net.Api;
using WebApi.OutputCache.V2;

namespace DashBoardAgil.DashBoardApi.Controllers.Api
{
    public class ComulativeFlowDiagramController : BaseApiController
    {

       //[CacheOutput(ServerTimeSpan = 1 * (60))]
        public Object Get(string time, DateTime dataIni, DateTime dataFim, bool dashboard = true)
        {

            var proj = GlobalConfig.Instance.ConfiguracaoRedmine.AllowList.FirstOrDefault(x => x.Identifier.Equals(time) || x.Id.Equals(time));

            #region processo
            var processo = _RedmineService.GetProcessoTeam(proj);
            var lststr = processo.ProcessoTipo.Select(x => x.TipoChamado.IdExterno).ToList();
            #endregion

            var parm = new NameValueCollection
                {
                    {RedmineKeys.TRACKER_ID, Global.Instance.ConcatenarLista("|", lststr)},
                    {RedmineKeys.STATUS_ID, "*"}
                };

            #region Data
            if (dataIni != null && dataFim != null)
            {
                parm.Add(RedmineKeys.CREATED_ON, String.Format("><{0}|{1}", ((DateTime)dataIni).ToString("yyyy-MM-dd"), ((DateTime)dataFim).ToString("yyyy-MM-dd")));
            }
            else if (dataIni != null && dataFim == null)
            {
                parm.Add(RedmineKeys.CREATED_ON, String.Format(">={0}", ((DateTime)dataIni).ToString("yyyy-MM-dd")));
            }
            else if (dataIni == null && dataFim != null)
            {
                parm.Add(RedmineKeys.CREATED_ON, String.Format("<={0}", ((DateTime)dataFim).ToString("yyyy-MM-dd")));
            }
            #endregion

            #region time
            if (!time.IsNullOrWhiteSpace() && proj.Identifier != null)
            {
                parm.Add(RedmineKeys.PROJECT_ID, String.Format("{0}", proj.Identifier));
            }
            else
            {
                return new {status = false, mensage = "erro"};
            }
            #endregion

            //verificar os custons fields dos times
            proj.CustonFields.ForEach(custon => {
                parm.Add(String.Format("{0}", custon.Campo), String.Format("{0}", custon.Valor));
            });
            //Fim verificar os custons fields dos times
            // Debugar.WriteLine(string.Format("Inicio: {0}", DateTime.Now.ToString("G")));
            //dashboard = false;

            var comulativeFlow = dashboard ?
                                 JsonConvert.DeserializeObject(Global.Instance.LerArquivo(Path.Combine(ConstantesCC.PATH_CFD_DIAGRAM, String.Format("{0}.json", time)))) :
                                _RedmineService.GetCumulativeFlowDiagram(parm, CrossCutting.Util.GetWeekList(dataIni, dataFim), processo);

            //Debugar.WriteLine(string.Format("Fim: {0}", DateTime.Now.ToString("G")));
            return comulativeFlow;
        }

        
    }
}
