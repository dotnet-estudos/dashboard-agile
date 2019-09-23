using DashBoardAgil.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashBoardAgil.Domain.Models
{
    public class PFS
    {
        [Key, Column(Order = 0)]
        public int ProcessoId { get; set; }
        [Key, Column(Order = 1)]
        public int FaseId { get; set; }
        [Key, Column(Order = 2)]
        public int SubFaseId { get; set; }
        public virtual Processo Processo { get; set; }
        public virtual Fase Fase { get; set; }
        public virtual SubFase SubFase { get; set; }
        public StatusRedmine StatusRedmine { get; set; }
        public SituacaoSubFase Situacao { get; set; }
        public int Ordem { get; set; }
    }
}
