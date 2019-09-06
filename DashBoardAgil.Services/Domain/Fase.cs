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
        public int FaseId { get; set; }
        public string Descricao { get; set; }
        public string Cor { get; set; }
        public List<SubFase> SubFases { get; set; }
    }
}
