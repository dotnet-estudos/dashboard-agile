using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashBoardAgil.Domain.Models
{
    public class ProcessoTipo
    {
        [Key, Column(Order = 0)]
        public int ProcessoId { get; set; }
        [Key, Column(Order = 1)]
        public int TipoChamadoId { get; set; }

        public Processo Processo { get; set; }
        public TipoChamado TipoChamado { get; set; }
    }
}
