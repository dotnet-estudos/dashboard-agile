using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DashBoardAgil.Domain.Models
{
    public class ConsumoHorasControl 
    {
        public ConsumoHorasControl()
        {
            this.HorasAlocadas = new List<ConsumoHoras>();
            this.Projetos = new List<Projeto>();
        }
        public List<ConsumoHoras> HorasAlocadas { get; set; }
        public List<Projeto> Projetos { get; set; }
        public double TotalAlocado => HorasAlocadas.Sum(x => x.Horas);
        public object ByProject
        {
            get
            {
                var ret = from hrs in HorasAlocadas
                          orderby hrs.Id descending 
                          group hrs by new { hrs.Projeto.Name, hrs.Projeto.Id, Time= hrs.Team.Name } into grp
                          select new
                          {
                              Projeto = grp.Key,
                              TotalAlocado = grp.Sum(x=>x.Horas),
                              Alocacoes = grp.Select( consumoHoras=> (ConsumoHoras)consumoHoras),
                              GaugeGoogle = new { options = GetOptionByProject(grp.Key.Id), data = new { Label = grp.Key.Name, Value= grp.Sum(x => x.Horas) } }
                          };

                          return ret;
            }
        }
        private object GetOptionByProject(int id)
        {
            var proj = Projetos.FirstOrDefault(x => x.ProjetoId.ToString().Equals(id));
            if (proj != null && proj.HorasContratadas > 0)
            {
                var options = new
                {
                    min= 0,
                    max= proj.HorasContratadas,
                    width = 600,
                    height = 400,
                    redFrom = CrossCutting.Util.GetPercentFrom(90, proj.HorasContratadas),
                    redTo = CrossCutting.Util.GetPercentFrom(100, proj.HorasContratadas),
                    yellowFrom = CrossCutting.Util.GetPercentFrom(75, proj.HorasContratadas),
                    yellowTo = CrossCutting.Util.GetPercentFrom(90, proj.HorasContratadas),
                    greenFrom = 0,
                    greenTo = CrossCutting.Util.GetPercentFrom(75, proj.HorasContratadas),
                    minorTicks = 10,
                    animation = new { easing = "inAndOut", duration = 600 }  
                };

                return options;
            }
            return null;

        }
        public object ByDay
        {
            get
            {
                var ret = from hrs in HorasAlocadas
                          orderby hrs.Id descending
                          group hrs by (DateTime)hrs.DataAlocacao into grp
                          select new { Dia = grp.Key, TotalAlocado = grp.Sum(x => x.Horas), Alocacoes = grp.Select(ConsumoHoras => (ConsumoHoras)ConsumoHoras) };

                return ret;
            }
        }
    }
}
