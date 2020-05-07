using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DashBoardAgil.CrossCutting;
using DashBoardAgil.Services.Enums;
using DashBoardAgil.Services.Extensions;

namespace DashBoardAgil.Services.Domain
{
    public class ThroughputControl
    {
        public ThroughputControl()
        {
            Throughputs = new List<Throughput>();
            //this.WeekList = new List<int>();
        }

        public DateTime? DataIni { get; set; }
        public DateTime? DataFim { get; set; }
        public List<Throughput> Throughputs { get; set; }
        public int TotalDias => Throughputs.Count>0? int.Parse(((TimeSpan) (Throughputs.Max(x => x.ClosedOn) - Throughputs.Min(x => x.ClosedOn))).TotalDays.ToString("####")): 0;
        public int TotalChamados => Throughputs.Count;
        public List<int> WeekList => CrossCutting.Util.GetWeekList(DataIni ?? new DateTime(DateTime.Now.Year, 1, 1), DataFim ?? DateTime.Now.Date);
        public string Throughput => string.Format("{0:##.##}", double.Parse(this.TotalChamados.ToString())/double.Parse(this.TotalDias.ToString()));
        public string ThroughputWeek => string.Format("{0:##.##}", double.Parse(this.TotalChamados.ToString()) / double.Parse(this.WeekList.Count.ToString()));
        public object ByDay
        {
            get
            {
                var chamadosDia = from chamado in Throughputs
                    orderby chamado.ClosedOn descending
                    group chamado by ((DateTime) chamado.ClosedOn).Date
                    into grp
                    select
                        new {Dia = grp.Key.ToString("d"), Total = grp.Count(), Chamados = grp.Select(x => x.IdChamado)};

                return chamadosDia;

            }

        }
        public List<object> ByType
        {
            get
            {
                var chamadosDiaTipo = (from chamado in Throughputs
                                       orderby chamado.ClosedOn descending
                                       group chamado by new { chamado.Tipo, ((DateTime)chamado.ClosedOn).Date } into grp
                                       select new { Dia = grp.Key.Date, Tipo = grp.Key.Tipo, Total = grp.Count() });
                return new List<object>() { chamadosDiaTipo};
            }
        }
        public List<object> ByTypeWeek
        {
            get
            {
                var retorno = new List<object>();

                var week = WeekList;

                var tipoChamado = from TipoChamadoRedmine e in Enum.GetValues(typeof(TipoChamadoRedmine))
                                   select new
                                   {
                                       Id = e.GetHashCode(),
                                       Nome = e.ToString(),
                                       tipo = e
                                   };

                var chamadosDiaTipo =(from chamado in Throughputs
                                  orderby chamado.ClosedOn descending
                                  group chamado by new {chamado.Tipo, ((DateTime)chamado.ClosedOn).Date} into grp
                                  select new { Dia = grp.Key.Date,  Tipo = grp.Key.Tipo, Total = grp.Count() });



                var serieGeral = new List<object>();
                week.ForEach(w =>
                {
                    var listData = new List<int>();
                    var count = chamadosDiaTipo.Where(x => x.Dia.GetWeekOfYear() == w).Sum(x => x.Total);
                    listData.Add(w);
                    listData.Add(count);
                    serieGeral.Add(listData.ToArray());
                });

                retorno.Add(new
                {
                    regression = true,
                    regressionSettings = new
                    {
                        type = "linear",
                        color = "rgba(55,136,174,.9)",
                        name = string.Format("Tendência (Throughput)")
                    },
                    name = "Throughput Gereal",
                    data = serieGeral.ToArray(),
                    stack = "Gereal",
                    color = "rgba(55,136,174,.9)"
                });



                tipoChamado.ToList().ForEach(tip=>
                {
                    var serie = new List<object>();
                    week.ForEach(w =>
                    {
                        var listData = new List<int>();
                        var count = chamadosDiaTipo.Where(x => x.Tipo == tip.tipo && x.Dia.GetWeekOfYear() == w).Sum(x => x.Total);
                        listData.Add(w);
                        listData.Add(count);
                        serie.Add(listData.ToArray());

                    });
                    retorno.Add(new
                    {
                        //regression = true,
                        //regressionSettings = new
                        //{
                        //    type = "linear",
                        //    color = string.Format("rgba({0})", RedmineUtil.GetRgbRgbaByTipoChamado(tip.tipo, true, 0.9)),
                        //    name = string.Format("Tendência (Throughput): {0}", tip.Nome)
                        //},
                        name = tip.Nome, data = serie.ToArray(), stack = "Throughput", color= string.Format("rgba({0})", RedmineUtil.GetRgbRgbaByTipoChamado(tip.tipo, false)),
                    });
                });


              

             

                return retorno;
            }
        }
        public List<object> ByVersion
        {
            get
            {
                var byVersion = (from chamado in Throughputs
                    orderby chamado.ClosedOn descending
                    group chamado by chamado.Versao
                    into grp
                    select new {Versao = grp.Key, Total = grp.Count(), Chamados = grp.Select(x => x)}).ToList();

                return new List<object>(byVersion.Select(x=>new {version = x}));

            }
        }
    }
}
