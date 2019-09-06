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
        public string Descricao { get; set; }
        public List<PFS> PFS { get; set; }
    }
}
