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

       [CacheOutput(ServerTimeSpan = 1 * (60))]
        public Object Get(string time, DateTime dataIni, DateTime dataFim, bool dashboard = true)
        {

            var parm = new NameValueCollection(_defaultTipoChamados);

            parm.Add(RedmineKeys.STATUS_ID, "*");

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

           if (!time.IsNullOrWhiteSpace())
           {
               parm.Add(RedmineKeys.PROJECT_ID, String.Format("{0}", time));
           }
           else
           {
               return new {status = false, mensage = "erro"};
           }
            #endregion
            // Debugar.WriteLine(string.Format("Inicio: {0}", DateTime.Now.ToString("G")));

           var proj =
               GlobalConfig.Instance.ConfiguracaoRedmine.AllowList.FirstOrDefault(x => x.Identifier.Equals(time) || x.Id.Equals(time));

            var db = new DashBoardAgil.InfraData.Contexto.Contexto(false);

            var processo = db.Processos
                             .Include(x => x.PFS)
                             .Include(x => x.ProcessoTipo)
                             .Include(x => x.ProcessoTipo.Select(y => y.TipoChamado))
                             .Include(x => x.PFS.Select(y => y.Fase))
                             .Include(x => x.PFS.Select(y => y.SubFase))
                             .FirstOrDefault(x => x.ProcessoId == proj.ProcessoId);

            processo.PFS.ForEach(x =>
            {
                x.Processo = null;
                x.SubFase.PFS = null;
                x.Fase.PFS = null;
            });

            var comulativeFlow = dashboard ?
                                 JsonConvert.DeserializeObject(Global.Instance.LerArquivo(Path.Combine(ConstantesCC.PATH_CFD_DIAGRAM, String.Format("{0}.json", time)))) :
                                _RedmineService.GetCumulativeFlowDiagram(parm, CrossCutting.Util.GetWeekList(dataIni, dataFim), processo);

            //Debugar.WriteLine(string.Format("Fim: {0}", DateTime.Now.ToString("G")));
            return comulativeFlow;
        }
    }
}
