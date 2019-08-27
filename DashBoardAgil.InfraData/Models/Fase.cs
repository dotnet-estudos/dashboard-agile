using System.Collections.Generic;

namespace DashBoardAgil.InfraData.Models
{
    public class Fase
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public string cor { get; set; }
        public virtual ICollection<SubFase> SubFases { get; set; }
    }
}