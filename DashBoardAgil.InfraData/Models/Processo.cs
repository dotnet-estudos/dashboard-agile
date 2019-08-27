using System.Collections.Generic;

namespace DashBoardAgil.InfraData.Models
{
    public class Processo
    {
        public int Id { get; set; }
        public virtual ICollection<Fase> Fases { get; set; }
    }
}
