using System.Collections.Generic;

namespace DashBoardAgil.Domain.Models
{
    public class Fase
    {
        public int FaseId { get; set; }
        public string Descricao { get; set; }
        public List<PFS> PFS { get; set; }
    }
}
