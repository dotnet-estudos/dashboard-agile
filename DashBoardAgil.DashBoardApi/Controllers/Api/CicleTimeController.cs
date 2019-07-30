using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DashBoardAgil.CrossCutting;
using DashBoardAgil.Services.Domain;
using DashBoardAgil.Services.Enums;
using Microsoft.Ajax.Utilities;
using Redmine.Net.Api;
using WebApi.OutputCache.V2;

namespace DashBoardAgil.DashBoardApi.Controllers.Api
{
    public class CicleTimeController : BaseApiController
    {
       [CacheOutput(ServerTimeSpan = 1 * (60*60))]
        public Object Get(string time, DateTime? dataIni, DateTime? dataFim,  StatusRedmine statusDe, StatusRedmine statusPara, TipoChamadoRedmine? tipoChamado=null, PrioridadeRedmine? prioridade=null, int primeiroSalto=5, int quantidadeSaltos = 10, bool leadTime =false, bool issueCanceled = true, bool  programada=false)
        {
            var parm = new NameValueCollection(_defaultTipoChamados);

            #region tipo/status
            if (tipoChamado != null)
            {
                parm = new NameValueCollection();
                parm.Add(RedmineKeys.TRACKER_ID, String.Format("{0}", tipoChamado.GetDisplayName()));
            }

            parm.Add(RedmineKeys.STATUS_ID, (leadTime && issueCanceled) ? "closed" : (leadTime ? StatusRedmine.Fechado.GetHashCode().ToString() : "*" ));

          
            #endregion

            #region Data
            if (dataIni != null && dataFim != null)
            {
                parm.Add(RedmineKeys.CREATED_ON,String.Format("><{0}|{1}", ((DateTime)dataIni).ToString("yyyy-MM-dd"), ((DateTime)dataFim).ToString("yyyy-MM-dd")));
            }
            else if (dataIni != null && dataFim == null)
            {
                parm.Add(RedmineKeys.CREATED_ON, String.Format(">={0}",  ((DateTime) dataIni).ToString("yyyy-MM-dd") ));
            }
            else if (dataIni == null && dataFim != null)
            {
                parm.Add(RedmineKeys.CREATED_ON, String.Format("<={0}", ((DateTime)dataFim).ToString("yyyy-MM-dd")));
            }
            #endregion

            #region prioridade/Time
            if (prioridade != null)
            {
                parm.Add(RedmineKeys.PRIORITY_ID, String.Format("{0}", prioridade.GetDisplayName()));
            }
            else if (!programada)
            {
                parm.Add(new NameValueCollection { { RedmineKeys.PRIORITY, Global.Instance.ConcatenarLista("|", _lsitPrioridae.Where(x => !x.Equals(PrioridadeRedmine.Programada.GetHashCode().ToString())).ToList()) } });
            }

            if (!time.IsNullOrWhiteSpace())
            {
                parm.Add(RedmineKeys.PROJECT_ID, String.Format("{0}", time));
            }
            #endregion
            
            var projectosAnalise = _RedmineService.GetCicleTimeTeam(parm, statusDe, statusPara, primeiroSalto, quantidadeSaltos, leadTime);

            return projectosAnalise;
        }
    }
}
