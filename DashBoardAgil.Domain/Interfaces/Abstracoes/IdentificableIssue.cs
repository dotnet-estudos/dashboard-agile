using System;
using DashBoardAgil.CrossCutting;
using DashBoardAgil.Domain.Enums;

namespace DashBoardAgil.Domain.Interfaces.Abstracoes
{
    public abstract class IdentificableIssue
    {
        public int IdChamado { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ClosedOn { get; set; }
        public string Projeto { get; set; }
        //public StatusRedmine? StatusDe { get; set; }
        //public string StatusDeString => StatusDe!=null? StatusDe.GetDisplayName():String.Empty;
        //public StatusRedmine? StatusPara { get; set; }
        //public string StatusParaString => StatusPara!=null?  StatusPara.GetDisplayName() : string.Empty;
        public TipoChamadoRedmine Tipo { get; set; }
        public string TipoString => this.Tipo.GetDisplayName();
        public PrioridadeRedmine Prioridade { get; set; }
        public string PrioridadeString => this.Prioridade.GetDisplayName();
        public IdentificableName Versao { get; set; }
        public IdentificableName Status { get; set; }
       
    }
}
