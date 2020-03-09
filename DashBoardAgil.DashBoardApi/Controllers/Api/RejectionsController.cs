using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using DashBoardAgil.Services;
using DashBoardAgil.CrossCutting;
using Redmine.Net.Api;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using WebApi.OutputCache.V2;
using System.Linq;
using DashBoardAgil.Domain.Models;


namespace DashBoardAgil.DashBoardApi.Controllers.Api
{
    public class RejectionsController : BaseApiController
    {

        [CacheOutput(ServerTimeSpan = 1 * (60))]
        public Object Get(string time, DateTime dataIni, DateTime dataFim, bool dashboard = true)
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
                    {RedmineKeys.STATUS_ID, "closed"},
                    {
                        RedmineKeys.CREATED_ON,
                        String.Format("><{0}|{1}", ((DateTime)dataIni).ToString("yyyy-MM-dd"), ((DateTime)dataFim).ToString("yyyy-MM-dd"))
                    }
                };


            if (!time.IsNullOrWhiteSpace() && proj.Identifier != null)
            {
                parm.Add(RedmineKeys.PROJECT_ID, String.Format("{0}", proj.Identifier));
            }
            //verificar os custons fields dos times
            proj.CustonFields.ForEach(custon => {
                parm.Add(String.Format("{0}", custon.Campo), String.Format("{0}", custon.Valor));
            });
            //Fim verificar os custons fields dos times

            //throughput = false;

            /*var ret = throughput? JsonConvert.DeserializeObject(Global.Instance.LerArquivo(Path.Combine(ConstantesCC.PATH_THROUGHPUT_REJECT_DIAGRAM, String.Format("{0}.json", time)))) :
                        _RedmineService.GetRejectionRate(parm);*/
            if (dashboard) {
                return JsonConvert.DeserializeObject(Global.Instance.LerArquivo(Path.Combine(ConstantesCC.PATH_THROUGHPUT_REJECT_DIAGRAM, String.Format("{0}.json", time))));
            } else {
                var reject = _RedmineService.GetRejectionRate(parm);
                var thoupt = new ThroughputControl();
                thoupt.DataFim = reject.DataFim = dataFim;
                thoupt.DataIni = reject.DataIni = dataIni;
                reject.Rejections.ForEach(rejection => thoupt.Throughputs.Add(CrossCutting.Util.ReflectObjec(rejection, new Throughput())));
                var week = CrossCutting.Util.GetWeekList(dataIni, dataFim);
                var unionSerieData = thoupt.ByTypeWeek.ToList();
                reject.ByTypeWeek.ToList().ForEach(x => unionSerieData.Add(x));
                return (new { Rejeicao = reject, Throughput = thoupt, Serie = unionSerieData, Categoria = week });
            }

            //return ret;
        }
    }
}
