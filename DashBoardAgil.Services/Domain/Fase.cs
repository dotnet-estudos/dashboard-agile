using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DashBoardAgil.Services.Enums;

namespace DashBoardAgil.Services.Domain
{
    public class Fase
    {
        public int Id { get; set; }
        public int IdProcesso { get; set; }
        public int? IdFase { get; set; }
        public int Ordem { get; set; }
        public int? OrdemSubFase { get; set; }
        public string Descricao { get; set; }
        public string RGB { get; set; }
        public StatusRedmine? EStatusRedmine { get; set; }
        public bool StatusAbertura { get; set; }
        public bool StatusFechamento { get; set; }
        public Fase FasePai  { get; set; }
        public List<Fase> SubFases { get; set; }

    }
}
