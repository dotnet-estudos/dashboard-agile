using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DashBoardAgil.CrossCutting;
using DashBoardAgil.DashBoardApi.Models;
using DashBoardAgil.Services;
using DashBoardAgil.Domain.Models;
using DashBoardAgil.Domain.Enums;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using Redmine.Net.Api;
using WebApi.OutputCache.V2;

namespace DashBoardAgil.DashBoardApi.Controllers.Api
{
    public class CicleTimeController : BaseApiController
    {
       [CacheOutput(ServerTimeSpan = 1 * (60*60))]
        public Object Get(string time, DateTime? dataIni, DateTime? dataFim,  StatusRedmine statusDe, StatusRedmine statusPara, TipoChamadoRedmine? tipoChamado=null, PrioridadeRedmine? prioridade=null, int primeiroSalto=5, int quantidadeSaltos = 10, bool leadTime =false, bool issueCanceled = true, bool  programada=false)
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
                };

            #region tipo/status
            if (tipoChamado != null)
            {
                parm = new NameValueCollection();
                parm.Add(RedmineKeys.TRACKER_ID, String.Format("{0}", tipoChamado.GetDisplayName()));
            }

            parm.Add(RedmineKeys.STATUS_ID, (leadTime && issueCanceled) ? "closed" : (leadTime ? Global.Instance.ConcatenarLista("|", GetStatusFechamento()) : "*" ));
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

            if (!time.IsNullOrWhiteSpace() && proj.Identifier != null)
            {
                parm.Add(RedmineKeys.PROJECT_ID, String.Format("{0}", proj.Identifier));
            }
            #endregion

            //verificar os custons fields dos times
            proj.CustonFields.ForEach(custon => {
                parm.Add(String.Format("{0}", custon.Campo), String.Format("{0}", custon.Valor));
            });
            //Fim verificar os custons fields dos times

            var projectosAnalise = _RedmineService.GetCicleTimeTeam(parm, statusDe, statusPara, primeiroSalto, quantidadeSaltos, leadTime);

            return projectosAnalise;
        }

        /// <summary>
        /// Relatorio por perido
        /// </summary>
        /// <param name="time"></param>
        /// <param name="dataIni"></param>
        /// <param name="dataFim"></param>
        /// <param name="statusDe"></param>
        /// <param name="statusPara"></param>
        /// <returns></returns>
        [CacheOutput(ServerTimeSpan = 1 * (60 * 60))]
        public Object Get(string time, DateTime? dataIni, DateTime? dataFim, StatusRedmine statusDe, StatusRedmine statusPara, bool rel)
        {
            var parm = new NameValueCollection(_defaultTipoChamados);

            var datas = new List<ParameterDefault>()
           {
               new  ParameterDefault{dataIni = new DateTime(2019, 01, 1), dataFim = new DateTime(2019, 01, 31)},
               new  ParameterDefault{dataIni = new DateTime(2019, 02, 1), dataFim = new DateTime(2019, 02, 28)},
               new  ParameterDefault{dataIni = new DateTime(2019, 03, 1), dataFim = new DateTime(2019, 03, 31)},
               new  ParameterDefault{dataIni = new DateTime(2019, 04, 1), dataFim = new DateTime(2019, 04, 30)},
               new  ParameterDefault{dataIni = new DateTime(2019, 05, 1), dataFim = new DateTime(2019, 05, 31)},
               new ParameterDefault {dataIni = new DateTime(2019, 06, 1), dataFim = new DateTime(2019, 06, 30)},
               new ParameterDefault {dataIni = new DateTime(2019, 07, 1), dataFim = new DateTime(2019, 07, 31)},
           };

            var RelRetorno = new List<Object>();

            var teams = new List<String>() { "519", "520", "518", "354", "534", "543" };

            var list = new List<Projeto>(GlobalConfig.Instance.ConfiguracaoRedmine.AllowList);

            list.Add(new Projeto() { Id = "543", Identifier = "aghuse-sustentacao" });

            var leads = new List<object>();

            list.Where(x => teams.Contains(x.Id)).ToList().ForEach(team =>
            //list.Where(x => x.Id.Equals("543")).ToList().ForEach(team =>
            {

                datas.ForEach(data =>
                {
                    var parmTemp = new NameValueCollection
                    {
                        {RedmineKeys.CREATED_ON, String.Format("><{0}|{1}", ((DateTime) data.dataIni).ToString("yyyy-MM-dd"), ((DateTime) data.dataFim).ToString("yyyy-MM-dd"))},
                        {RedmineKeys.PROJECT_ID, String.Format("{0}", team.Id)},
                        {RedmineKeys.STATUS_ID, StatusRedmine.Fechado.GetHashCode().ToString()}
                    };
                    // var parmTempAnalise = new NameValueCollection(parmTemp) {{RedmineKeys.STATUS_ID, "*"}};
                    var projectosAnalise = _RedmineService.GetCicleTimeTeam(parmTemp, statusDe, statusPara, 5, 10, false);
                    // var parmTempClosed = new NameValueCollection(parmTemp) { { RedmineKeys.STATUS_ID, "closed" } };
                    //var trup = _RedmineService.GetThroughputRate(parmTemp);
                    var trps = new List<Throughput>();

                    projectosAnalise.UniversoChamados.ForEach(x =>
                    {
                        trps.Add(new Throughput()
                        {
                            IdChamado = x.Id,
                            ClosedOn = (DateTime)x.ClosedOn,
                            CreatedOn = (DateTime)x.CreatedOn,
                            Prioridade = (PrioridadeRedmine)x.Priority.Id,
                            Projeto = x.Project.Name,
                            Tipo = (TipoChamadoRedmine)x.Tracker.Id,

                        });
                    });



                    var trup = new ThroughputControl()
                    {
                        DataIni = data.dataIni,
                        DataFim = data.dataFim,
                        Throughputs = trps
                    };

                    leads.Add(new
                    {
                        Team = team.ToString(),
                        dataIni = ((DateTime)data.dataIni).ToString("dd/MM/yyyy"),
                        dataFim = ((DateTime)data.dataFim).ToString("dd/MM/yyyy"),
                        CicleTimeDiaString = projectosAnalise.GeneralAverageDaysString,
                        CicleTime = projectosAnalise.GeneralAverageDays,
                        CicleTimeDiaMediana = projectosAnalise.GeneralMedianaDaysString,
                        CicleTimeMediana = projectosAnalise.GeneralMedianaDays,
                      //  Min = projectosAnalise.LeadTimes.Min(x=>x.LeadBrutoDays),
                       // Max = projectosAnalise.LeadTimes.Max(x => x.LeadBrutoDays),
                        //Percentil90 = Math.Round(projectosAnalise.LeadTimes.Select(x => new { x.LeadBrutoDays }).OrderBy(x => x.LeadBrutoDays).Skip(Convert.ToInt32((projectosAnalise.LeadTimes.Count() * 0.9))).First().LeadBrutoDays, 2, MidpointRounding.AwayFromZero),
                        analise = projectosAnalise.ByLeadAnalyze,
                        ThroughputWeek = trup.ThroughputWeek,
                        ThroughputRate = trup.Throughput,
                        TotalChamadosFechados = trup.TotalChamados
                    });


                });

            });

            return leads;
            // return projectosAnalise;
        }
    }
}
