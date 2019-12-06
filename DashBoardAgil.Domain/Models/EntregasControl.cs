using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using DashBoardAgil.CrossCutting;
using Redmine.Net.Api;
using Redmine.Net.Api.Types;

namespace DashBoardAgil.Domain.Models
{
    
    public class EntregasControl
    {
        public EntregasControl(List<Chamado> chamados, List<Rejection> rejections, Projeto team, DateTime dataIni, DateTime dataFim)
        {
            _chamados = chamados;
            _rejections = rejections;
            Team = team;
            DataIni = dataIni;
            DataFim = dataFim;
        }

        public Projeto Team { get; set; }
        private readonly List<Chamado> _chamados;
        private readonly List<Rejection> _rejections;
        public DateTime DataIni { get; set; }
        public DateTime DataFim { get; set; }
        public int QuantidadeRejeitado => _rejections.Count(x => x.HasRejection);
        public dynamic GetChamadosEntregues
        {
            get
            {
                if (Team?.ProcessoId > 0 && Team?.Processo != null)
                {
                    var statusEntregue = Team.Processo.PFS.Where(x => x.Cliente).OrderBy(x => x.Ordem).Select(x => new {SubFase = x.SubFase, Status = x.StatusRedmine});

                    if (statusEntregue?.Any() ?? false)
                    {

                        Expression<Func<Journal, bool>> expJournals1st = x => ( (x.CreatedOn >= DataIni && x.CreatedOn <= DataFim) && 
                                                                                (x.Details.Any(d =>d.Name.Equals(RedmineKeys.STATUS_ID) && 
                                                                                                   d.Property.Equals(ConstantesCC.Details.Name_Attr) &&
                                                                                                    (
                                                                                                     statusEntregue.Select(red => red.Status.GetHashCode().ToString()).Contains(d.NewValue ?? "0") || 
                                                                                                     statusEntregue.Select(red => red.Status.GetHashCode().ToString()).Contains(d.OldValue ?? "0")
                                                                                                    )
                                                                                               )
                                                                                )
                                                                               );
                        Expression<Func<Journal, bool>> expJournals2nd = x => !((x.CreatedOn < DataIni) &&
                                                                   (x.Details.Any(d => d.Name.Equals(RedmineKeys.STATUS_ID) && d.Property.Equals(ConstantesCC.Details.Name_Attr) &&
                                                                                              (statusEntregue.Select(red => red.Status.GetHashCode().ToString()).Contains(d.NewValue ?? "0") ||
                                                                                               statusEntregue.Select(red => red.Status.GetHashCode().ToString()).Contains(d.OldValue ?? "0")))));


                        Func<Journal, bool> expConsolidado = expJournals1st.And(expJournals2nd).Compile();




                        var TotalEntregue = from act in _chamados
                                      orderby act.CreatedOn
                                      where act.Journals?.Any(expConsolidado) ?? false
                                      select new
                                      {
                                          act.Id,
                                          act.CreatedOn,
                                          aceite = act.Journals?.Any(expJournals1st.Compile()) ?? false,
                                          dataAceite = act.Journals?.FirstOrDefault(expJournals1st.Compile())?.CreatedOn ?? null
                                      };

                        var grpEntregue = from grpAceite in TotalEntregue
                                     orderby (DateTime)grpAceite.dataAceite
                                     where grpAceite.aceite
                                     group grpAceite by ((DateTime)grpAceite.dataAceite).Date
                                        into grp
                                     select new
                                     {
                                         Dia = grp.Key,
                                         TotalChamados = grp.Count(),
                                         Chamados = grp.Select(x => x.Id).ToArray()
                                     };

                        return new { TotalEntregue = grpEntregue.Sum(x => x.TotalChamados), QuantidadeValidadoDia = grpEntregue, StatusVerificados = statusEntregue };

                    }

                }
                return null;
            }
        }
    }
}
