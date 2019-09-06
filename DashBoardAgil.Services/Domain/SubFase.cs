using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DashBoardAgil.Services.Enums;


namespace DashBoardAgil.Services.Domain
{
    public class SubFase
    {
        public int SubFaseId { get; set; }
        public int FaseId { get; set; }
        public int Ordem { get; set; }
        public string Descricao { get; set; }
        public StatusRedmine StatusRedmine { get; set; }
        public SituacaoSubFase Situacao { get; set; }
        public virtual Fase Fase { get; set; }
    }
}
