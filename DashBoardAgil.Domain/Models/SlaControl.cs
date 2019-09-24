using System.Collections.Generic;

namespace DashBoardAgil.Domain.Models
{
    public class SlaControl : ChamadoControl
    {
        public SlaControl()
        {
            ChamadoSemafaro = new List<Chamado>();
        }

        public Sla Sla { get; set; }

        public Projeto Projeto { get; set; }
        
    }

  
}