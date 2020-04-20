using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DashBoardAgil.CrossCutting.Enums;

namespace DashBoardAgil.Domain.Models
{
    public class Fase
    {
        public int FaseId { get; set; }
        public string Descricao { get; set; }
        public List<PFS> PFS { get; set; }
    }
}
