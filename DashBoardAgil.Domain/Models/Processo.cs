using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashBoardAgil.Domain.Models
{
    public class Processo
    {
        public int ProcessoId { get; set; }
        public string Nome { get; set; }
        public List<PFS> PFS { get; set; }
    }
}
