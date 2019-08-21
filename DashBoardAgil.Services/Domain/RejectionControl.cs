using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using DashBoardAgil.CrossCutting;
using DashBoardAgil.Services.Enums;
using DashBoardAgil.Services.Extensions;

namespace DashBoardAgil.Services.Domain
{
    public class RejectionControl
    {
        public RejectionControl()
        {
            Rejections =  new List<Rejection>();
        }

        public DateTime? DataIni { get; set; }
        public DateTime? DataFim { get; set; }
        public List<Rejection> Rejections { get; set; }
        public List<Rejection> Rejected => Rejections.Where(x => x.HasRejection).ToList();
        public List<int> WeekList => CrossCutting.Util.GetWeekList(DataIni ?? new DateTime(DateTime.Now.Year, 1, 1), DataFim ?? DateTime.Now.Date);
        public int TotalChamados => Rejections.Count;
        public string RejectionsRate => Rejections.Any(x => x.HasRejection) ? (double.Parse(this.Rejections.Count(x => x.HasRejection).ToString()) / double.Parse(this.Rejections.Count.ToString())).ToString("P") : "0.0 %";
        public string RejectionsRateWeek => Rejections.Any(x => x.HasRejection) ? (double.Parse(this.Rejections.Count(x => x.HasRejection).ToString()) / double.Parse(this.WeekList.Count.ToString()) * 100   ).ToString() + "%" : "0.0 %";

        public object ByDay
        {
            get
            {
                var returno =   from rejc in Rejections.Where(x=>x.HasRejection).ToList()
                                orderby rejc.CreatedOn
                                group rejc by ((DateTime) rejc.CreatedOn).Date
                                into grp
                                select new { Dia = grp.Key.ToString("d"), total = grp.Count()};


                return returno;
            }
        }
        public List<object> ByTypeWeek
        {
            get
            {
                var retorno = new List<object>();

                var week = this.WeekList;

                var tipoChamado = from TipoChamadoRedmine e in Enum.GetValues(typeof(TipoChamadoRedmine))
                                  select new
                                  {
                                      Id = e.GetHashCode(),
                                      Nome = e.ToString(),
                                      tipo = e
                                  };

                var chamadosDiaTipo = (from chamado in Rejections.Where(x=>x.HasRejection)
                                       orderby chamado.ClosedOn descending
                                       group chamado by new { chamado.Tipo, ((DateTime)chamado.CreatedOn).Date } into grp
                                       select new { Dia = grp.Key.Date, Tipo = grp.Key.Tipo, Total = grp.Count() });


                tipoChamado.ToList().ForEach(tip =>
                {
                    var serie = new List<int>();
                    week.ForEach(w =>
                    {
                        var count = chamadosDiaTipo.Where(x => x.Tipo == tip.tipo && x.Dia.GetWeekOfYear() == w).Sum(x=>x.Total);
                        serie.Add(count);
                    });
                    retorno.Add(new { name = tip.Nome, data = serie.ToArray(), stack = "Rejeiçao", color = string.Format("rgba({0})", RedmineUtil.GetRgbRgbaByTipoChamado(tip.tipo, true)), });
                });

                return retorno;
            }
        }
        public List<object> ByType
        {
            get
            {
                var chamadosDiaTipo = (from chamado in Rejections.Where(x => x.HasRejection)
                                       orderby chamado.ClosedOn descending
                                       group chamado by new { chamado.Tipo, ((DateTime)chamado.CreatedOn).Date } into grp
                                       select new { Dia = grp.Key.Date, Tipo = grp.Key.Tipo, Total = grp.Count(), chamados = grp.Select(x=>x.IdChamado) });

                return new List<object> {chamadosDiaTipo};
            }
        }

    }
}
