using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashBoardAgil.Services.Domain
{
    public class ChamadoControl
    {
        public ChamadoControl()
        {
            this.ChamadoSemafaro = new List<Chamado>();
        }
        public List<Chamado> ChamadoSemafaro { get; set; }

        public List<Chamado> ChamadosIn
        {
            get { return ChamadoSemafaro.Where(x => x.Under).ToList(); }
        }

        public List<CalendarioChamado> ChamadosPorDia 
        {
            get
            {
                var retorno = from chamado in ChamadoSemafaro
                              group chamado by ((DateTime)chamado.CreatedOn).Date
                    into dates
                              select
                                  new
                                  {
                                      Data = dates.Key,
                                      Chamados = dates.Select(x => new Chamado() { Id = x.Id })
                                  };
                var list = new List<CalendarioChamado>();
                retorno.ToList().ForEach(x =>
                {
                    list.Add(new CalendarioChamado { Data = x.Data, Total = x.Chamados.Count(), TotalOut = x.Chamados.Count(y => !y.Under), ChamadosArray = x.Chamados.ToList().Select(ch => ch.Id.ToString()).ToArray() });
                });

                return list;
            }
        }

        public List<Chamado> ChamadosOver { get { return ChamadoSemafaro.Where(x => !x.Under).ToList(); } }

        public int TotalChamdos
        {
            get { return ChamadoSemafaro.Count; }
        }

        public int TotalChamdosIn
        {
            get { return ChamadosIn.Count; }
        }

        public int TotalChamadosOut
        {
            get { return ChamadosOver.Count; }
        }
    }
}
