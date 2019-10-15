using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using DashBoardAgil.CrossCutting;
using DashBoardAgil.Domain.Enums;
using DashBoardAgil.Domain.Interfaces.Abstracoes;

namespace DashBoardAgil.Domain.Models
{
    public class LeadTime : IdentificableIssue
    {
        //public int IdChamado { get; set; }
        //public DateTime CreatedOn { get; set; }
        //public string Projeto { get; set; }
        public StatusRedmine StatusDe { get; set; }
        public string StatusDeString => StatusDe.GetDisplayName();
        public StatusRedmine StatusPara { get; set; }
        public string StatusParaString => StatusPara.GetDisplayName();
        //public TipoChamadoRedmine Tipo { get; set; }
        //public string TipoString => this.Tipo.GetDisplayName();
        //public PrioridadeRedmine Prioridade { get; set; }
        //public string PrioridadeString => this.Prioridade.GetDisplayName();
        public double LeadUtil { get; set; }
        public double LeadUtilDays => Math.Round((LeadUtil /7.6), 2, MidpointRounding.AwayFromZero);
        public double LeadBruto { get; set; }
        public double LeadBrutoDays => Math.Round((LeadBruto/24),2, MidpointRounding.AwayFromZero);
        public double LeadDiaUtil { get; set; }

        private int? _Tempo;
        public virtual int Tempo
        {
            get
            {
                var intLead = Convert.ToInt32(this.LeadUtil);// int.Parse(this.LeadUtil.ToString(new CultureInfo("pt-br")));
                return _Tempo ?? intLead;
            }
            set { _Tempo = value; }
        }


    }
}
