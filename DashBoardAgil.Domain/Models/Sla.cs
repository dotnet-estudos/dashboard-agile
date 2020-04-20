using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DashBoardAgil.CrossCutting.Enums;

namespace DashBoardAgil.Domain.Models
{

    public class Sla
    {
        public int IdSla { get; set; }
        public string Identificador { get; set; }
        public string Descricao { get; set; }
        public StatusRedmine StatusDe { get; set; }
        public StatusRedmine StatusPara { get; set; }

    }
}
