using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashBoardAgil.Services.Domain
{
    public class Processo
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public List<Fase> Fluxo { get; set; }
    }
}
