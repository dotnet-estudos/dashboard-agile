using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web.WebPages.Html;
using DashBoardAgil.CrossCutting;
using DashBoardAgil.DashBoardApi.Models;
using DashBoardAgil.Services;
using DashBoardAgil.Services.Domain;
using Microsoft.Ajax.Utilities;
using Redmine.Net.Api;
using Redmine.Net.Api.Types;
using WebGrease.Css.Extensions;

namespace DashBoardAgil.DashBoardApi.Controllers.Api
{
    public class DiaDevController : BaseApiController
    {
        public Object Get(string time, DateTime dataIni, DateTime dataFim)
        {
            if (time.IsNullOrWhiteSpace())
                return new { status = false, mensagem = "Time é Obrigatório", SlaControl = new SlaControl() };

            var _parametros = new NameValueCollection
                             {
                               // {RedmineKeys.PROJECT_ID, time}
              
                                {RedmineKeys.SPENT_ON, String.Format("><{0}|{1}", dataIni.ToString("yyyy-MM-dd"), dataFim.ToString("yyy-MM-dd")) }
                                , {RedmineKeys.SORT, "spent_on:desc" }
                                ,{"criteria", "user" }
                                 ,{"group_by", "hours"}
                              };

            var diaDevControl = new List<DiaDevControl>();
            var lancamentoTotais = new List<Alocacao>();

            GlobalConfig.Instance.ConfiguracaoRedmine.ProjetosAtivos().Where(x => x.Identifier.Equals(time) || time.Equals("0")).ForEach(
                    (team) =>
                    {
                        var parm = new NameValueCollection(_parametros);
                        parm.Add(RedmineKeys.PROJECT_ID, team.Identifier);

                        var lancamentos = _Redmine.GetObjects<TimeEntry>(parm);

                        var listAlocaco = lancamentos.Select(x => Global.ReflectObjec(x, new Alocacao(team))).ToList();
                        //listAlocaco.ForEach(x => x.Time = team);
                        lancamentoTotais.AddRange(listAlocaco);
                        diaDevControl.Add(new DiaDevControl { Lancamentos = listAlocaco, Time = team });

                    });



            var diaDevUnion = (from devCon in lancamentoTotais
                               orderby devCon.SpentOn descending
                               group devCon by new { devCon.SpentOn, devCon.Time, devCon.User }
                               into gr
                               select new { gr.Key.SpentOn, gr.Key.Time, gr.Key.User, Quantidade = gr.Count() }).ToList();

            var diaDevUni = (from di in diaDevUnion
                            orderby di.SpentOn descending
                            group di by di.SpentOn
                            into grpd
                            select new { dia = ((DateTime)grpd.Key).ToString("yyyy-MM-dd"), date = ((DateTime)grpd.Key), info = grpd.Select(x => new { x.Time.Identifier, Quantidade = grpd.Where(t => t.Time.Identifier == x.Time.Identifier).GroupBy(y => y.User).Count() }) }).ToList();



            var perDay = (from di in diaDevUnion.ToList()
                         orderby di.Time.Identifier
                         group di by new { di.Time, Quantidade = diaDevUnion.Where(t => t.Time.Identifier == di.Time.Identifier && t.SpentOn == di.SpentOn).GroupBy(y => y.User).Count() }
                         into grpd
                         select new { grpd.Key.Time, grpd.Key.Quantidade, TotalDias = grpd.GroupBy(x=>x.SpentOn).Count()  }).ToList();

            var perDay2 = from di in perDay
                          orderby di.Quantidade
                          group di by new { di.Time }
                into grpd
                          select new { grpd.Key.Time, Info = grpd.Select(y=> new { y.Quantidade, y.TotalDias} )};
            
            var filtro = new { time, dataIni, dataFim };

            return new { status = true, mensagem = "sucesso", filtro, DiaDevControl = diaDevControl, DiaDevUnion = diaDevUni, PerDay = perDay2 };
        }
    }
}
