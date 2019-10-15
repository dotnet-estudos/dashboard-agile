using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using DashBoardAgil.CrossCutting;
using DashBoardAgil.Domain.Enums;
using Newtonsoft.Json;
using Redmine.Net.Api;
using Redmine.Net.Api.Types;

namespace DashBoardAgil.Domain.Models
{
    public class DiaDevControl
    {
        public DiaDevControl()
        {
            
        }
        public DiaDevControl(List<Chamado> chamados )
        {
            Chamados = chamados;
        }
        public DiaDevControl(List<Chamado> chamados, DateTime dataIni, DateTime dataFim)
        {
            DataIni = dataIni;
            DataFim = dataFim;
            Chamados = chamados;

        }
        private List<Chamado> Chamados { get; set; }
        public Projeto Time { get; set; }
        public List<Alocacao> Lancamentos { get; set; }
        public DateTime DataIni { get; set; }
        public DateTime DataFim { get; set; }
        public int TotalDiaDev => LancamentosDia.Any() ? LancamentosDia.Sum(x => x.Quantidade) : 0;
        public int? DiaDevLimite => Time?.QuantidadeDesenvolvedores > 0 ? (Time?.QuantidadeDesenvolvedores*LancamentosDia.Count()) : 0;
        public int DiaDev => (DiaDevLimite??0) > 0 && TotalDiaDev > (DiaDevLimite??0) ? (DiaDevLimite??0) : TotalDiaDev;
        public List<LancamentosDiaDesenvolvedor> LancamentosDiaDesenvolvedor
        {
            get
            {
                var qtdDiaDev = from lanc in Lancamentos
                                orderby lanc.SpentOn
                                group lanc by new { lanc.SpentOn, lanc.User } into dias
                                select new LancamentosDiaDesenvolvedor { Dia = (DateTime)dias.Key.SpentOn,  User = new User() { Id = dias.Key.User.Id, FirstName = dias.Key.User.Name } , LancamentosDia = dias.Count(), TotalHoraDia = dias.Sum(x => x.Hours) };

                return qtdDiaDev.ToList();
            }
        }
        public List<LancamentosDia> LancamentosDia
        {
            get
            {
                var diaDev = from lac in this.LancamentosDiaDesenvolvedor.ToList()
                             orderby lac.Dia
                             group lac by lac.Dia
                    into d
                             select new LancamentosDia { Dia = (DateTime)d.Key, Quantidade = d.Count(), Desenvolvedores = d.Select(x => x.User).ToList() };

                return diaDev.ToList();
            }
        }
        public object GetGroupByTeamUser
        {
            get
            {

                return null;
            }
        }
        public object GetBacklogPerDay
        {
            get
            {
                if (Time?.ProcessoId > 0 && Time?.Processo != null)
                {
                    //var processo = JsonConvert.DeserializeObject<Processo>(Global.Instance.LerArquivo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"ProcessoBanco.json"))); //db.Processo.FirsrOrDefault(x=>x.ProcessoId == Time.ProcessoId);
                    var RedmineBackLoadStatus = Time.Processo.PFS?.Where(x => x.BackLog).OrderBy(x => x.Ordem).Select(x => new {SubFase = x.SubFase, status = x.StatusRedmine, situacao=x.Situacao});
                    

                    var dicBackLogDay = new Dictionary<DateTime, int>();

                    var dayBackLog = from day in Chamados
                                    orderby day.CreatedOn descending
                                    group day by ((DateTime) day.CreatedOn).Date
                                    into grp
                                    select new
                                    {
                                        dia = grp.Key,
                                        backlog = true,
                                        //VERIRICAR COMO FAZER O BACKLOG POR DIA, grp.Any(x=> RedmineBackLoadStatus.Any(s=>s.situacao == SituacaoSubFase.Abertura) && )
                                    };
                }

                return null;
            }
        }
        public object GetAceitePO
        {
            get
            {
                if (Time?.ProcessoId > 0 && Time?.Processo != null)
                {
                    //var processo = JsonConvert.DeserializeObject<Processo>(Global.Instance.LerArquivo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"ProcessoBanco.json"))); //db.Processo.FirsrOrDefault(x=>x.ProcessoId == Time.ProcessoId);
                    var redmineBackLoadStatus = Time.Processo.PFS?.Where(x => x.ValidadoPO).OrderBy(x => x.Ordem).Select(x => new { SubFase = x.SubFase, status = x.StatusRedmine,}).ToList();

                    if (redmineBackLoadStatus?.Any() ?? false)
                    {
                        var aceitPO = from act in Chamados
                            orderby act.CreatedOn
                            where act.Journals?.Any(x => ((x.CreatedOn >= DataIni && x.CreatedOn <= DataFim) &&
                                                         ( x.Details.Any(d => d.Name.Equals(RedmineKeys.STATUS_ID) && d.Property.Equals(ConstantesCC.Details.Name_Attr) &&
                                                                                     (redmineBackLoadStatus.Select(red => red.status.GetHashCode().ToString()).Contains(d.NewValue ?? "0") ||
                                                                                      redmineBackLoadStatus.Select(red => red.status.GetHashCode().ToString()).Contains(d.OldValue ?? "0"))) )) &&
                                                        !((x.CreatedOn < DataIni) &&
                                                         (x.Details.Any(d => d.Name.Equals(RedmineKeys.STATUS_ID) && d.Property.Equals(ConstantesCC.Details.Name_Attr) &&
                                                                                    (redmineBackLoadStatus.Select(red => red.status.GetHashCode().ToString()).Contains(d.NewValue ?? "0") ||
                                                                                     redmineBackLoadStatus.Select(red => red.status.GetHashCode().ToString()).Contains(d.OldValue ?? "0")))))

                                                          ) ?? false
                            select new
                            {
                                act.Id,
                                act.CreatedOn,
                                aceite = act.Journals?.Any(x => (x.CreatedOn >= DataIni && x.CreatedOn <= DataFim) && 
                                                                 x.Details.Any(d => d.Name.Equals(RedmineKeys.STATUS_ID) && d.Property.Equals(ConstantesCC.Details.Name_Attr) &&
                                                                                     (redmineBackLoadStatus.Select(red => red.status.GetHashCode().ToString()).Contains(d.NewValue ?? "0") ||
                                                                                      redmineBackLoadStatus.Select(red => red.status.GetHashCode().ToString()).Contains(d.OldValue ?? "0")))) ?? false,
                                dataAceite = act.Journals?.FirstOrDefault(x => (x.CreatedOn >= DataIni && x.CreatedOn <= DataFim) && 
                                                                                x.Details.Any(d => d.Name.Equals(RedmineKeys.STATUS_ID) && d.Property.Equals(ConstantesCC.Details.Name_Attr) &&
                                                                                      (redmineBackLoadStatus.Select(red => red.status.GetHashCode().ToString()).Contains(d.NewValue ?? "0") ||
                                                                                       redmineBackLoadStatus.Select(red => red.status.GetHashCode().ToString()).Contains(d.OldValue ?? "0"))))?.CreatedOn??null
                            };

                        var grpAct =    from grpAceite in aceitPO
                                        orderby (DateTime) grpAceite.dataAceite
                                        where grpAceite.aceite
                                        group grpAceite by ((DateTime) grpAceite.dataAceite).Date
                                        into grp
                                        select new
                                        {
                                            Dia = grp.Key,
                                            TotalChamados = grp.Count(),
                                            Chamados = grp.Select(x => x.Id).ToArray()
                                        };

                        return new {TotalValidado = grpAct.Sum(x => x.TotalChamados), QuantidadeValidadoDia = grpAct, StatusVerificados = redmineBackLoadStatus };

                    }

                }
                return null;
            }
        }
    }

    public class LancamentosDiaDesenvolvedor
    {
        public DateTime Dia { get; set; }
        public string DiaString => ((DateTime) this.Dia).ToString("dd/MM/yyyy");
        public User User { get; set; }
        public int  LancamentosDia { get; set; }
        public decimal TotalHoraDia { get; set; }


    }

    public class LancamentosDia
    {
        public DateTime Dia { get; set; }
        public string DiaString => ((DateTime)this.Dia).ToString("dd/MM/yyyy");
        public List<User> Desenvolvedores { get; set; }
        public String[] Devs => Desenvolvedores.Select(x => x.FirstName).OrderBy(x => x).ToArray();
        public int Quantidade { get; set; }
        public decimal TotalHoraDia { get; set; }
    }
}
