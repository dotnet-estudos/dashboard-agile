using System;
using System.Collections.Generic;
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
        }

        public DateTime? DataIni { get; set; }
        public DateTime? DataFim { get; set; }
        public List<Throughput> Throughputs { get; set; }
        public int TotalDias => int.Parse(((TimeSpan) (Throughputs.Max(x => x.ClosedOn) - Throughputs.Min(x => x.ClosedOn))).TotalDays.ToString("####"));
        public int TotalChamados => Throughputs.Count;
        public string Throughput => string.Format("{0:##.##}", double.Parse(this.TotalChamados.ToString())/double.Parse(this.TotalDias.ToString()));
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

                var week = CrossCutting.Util.GetWeekList(DataIni?? new DateTime(DateTime.Now.Year, 1, 1), DataFim?? DateTime.Now.Date);

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


                tipoChamado.ToList().ForEach(tip=>
                {
                    var serie = new List<int>();
                    week.ForEach(w =>
                    {
                        var count = chamadosDiaTipo.Where(x => x.Tipo == tip.tipo && x.Dia.GetWeekOfYear() == w).Sum(x => x.Total);
                        serie.Add(count);
                    });
                    retorno.Add(new { name= tip.Nome, data = serie.ToArray(), stack = "Throughput", color= string.Format("rgba({0})", RedmineUtil.GetRgbRgbaByTipoChamado(tip.tipo, false)), });
                });
                
                return retorno;
            }
        }
    }
}
