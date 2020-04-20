using System;
using System.Collections.Generic;
using System.Linq;
using DashBoardAgil.CrossCutting;
using DashBoardAgil.Domain.Interfaces.Abstracoes;
using Redmine.Net.Api.Types;

namespace DashBoardAgil.Domain.Models
{
    public class CumulativeFlowDiagramControl
    {
        public CumulativeFlowDiagramControl(Processo processo, List<CumulativeFlowDiagram> listChamados, List<Issue> chamados)
        {
            Processo = processo;
            ListChamados = listChamados;
            Chamados = chamados;
        }
        private List<CumulativeFlowDiagram> ListChamados { get;  }
        private List<Issue> Chamados { get; }
        private Processo Processo { get; set; }
        public List<CumulativeFlowDiagram> ComulativeFlow
        {
            get
            {
                var weekList = ListChamados.Select(x => x.Semana).Distinct();

                var retorno = new List<CumulativeFlowDiagram>();

                weekList.ToList().ForEach(week =>
                {
                    Processo.PFS.OrderByDescending(x => x.Ordem).ToList().ForEach(pfs =>
                    {
                        Func<CumulativeFlowDiagram, bool> funcValid = ch => ch.Semana == week &&  pfs.FaseId == ch.Fase.FaseId && ch.SubFase.SubFaseId == pfs.SubFaseId;

                        var add = ListChamados.Where(funcValid).Where(cham => !retorno.Where(funcValid).Select(y => y.IdChamado).Contains(cham.IdChamado));
                        retorno.AddRange(add.ToList());
                    });
                });

                return retorno;

            }
        }

        public dynamic ComulativeFlow2
        {
            get
            {
                return null;
            }
        }



    }
}